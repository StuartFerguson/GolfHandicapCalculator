using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ManagementAPI.Database;
using ManagementAPI.Database.SeedData;
using ManagementAPI.Service.Bootstrapper;
using ManagementAPI.Service.CommandHandlers;
using ManagementAPI.Service.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using Shared.CommandHandling;
using Shared.EventStore;
using Shared.Extensions;
using Shared.General;
using StructureMap;
using Swashbuckle.AspNetCore.Swagger;
using ESLogger = EventStore.ClientAPI;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ManagementAPI.Service
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        #region Properties
 
        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public static IConfigurationRoot Configuration { get; set; }
 
        /// <summary>
        /// Gets or sets the hosting environment.
        /// </summary>
        /// <value>
        /// The hosting environment.
        /// </value>
        public static IHostingEnvironment HostingEnvironment { get; set; }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public static IContainer Container { get; private set; }

        /// <summary>
        /// The management API read model connection string
        /// </summary>
        private static String ManagementAPIReadModelConnectionString;

        #endregion

        #region Constructors
 
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public Startup(IHostingEnvironment env)
        {
            IConfigurationBuilder builder =
                new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional:true)
                    .AddEnvironmentVariables();
 
            Startup.Configuration = builder.Build();
            Startup.HostingEnvironment = env;

            // Get the DB Connection Strings
            ManagementAPIReadModelConnectionString = Startup.Configuration.GetConnectionString(nameof(ManagementAPIReadModel));
        }

        #endregion

        #region Public Methods

        #region public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)        
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            String nlogConfigFilename = $"nlog.config";
            if (String.Compare(HostingEnvironment.EnvironmentName, "Development", true) == 0)
            {
                nlogConfigFilename = $"nlog.{HostingEnvironment.EnvironmentName}.config";
            }

            loggerFactory.AddConsole();
            loggerFactory.ConfigureNLog(Path.Combine(HostingEnvironment.ContentRootPath, nlogConfigFilename));
            loggerFactory.AddNLog();

            ILogger logger = loggerFactory.CreateLogger("GolfHandicapping");

            Logger.Initialise(logger);
            Logger.LogInformation("Hello from Logger.");

            ConfigurationReader.Initialise(Startup.Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Setup the database
            if (!HostingEnvironment.IsEnvironment("IntegrationTest"))
            {
                this.InitialiseDatabase(app, env).Wait();
            }

            app.AddExceptionHandler();
            app.AddRequestLogging();
            app.AddResponseLogging();

            app.UseAuthentication();

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Golf Handicapping API v1");
            });
        }
        #endregion

        #region public static IContainer GetConfiguredContainer(IServiceCollection services, IHostingEnvironment hostingEnvironment)        
        /// <summary>
        /// Gets the configured container.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        /// <returns></returns>
        public static IContainer GetConfiguredContainer(IServiceCollection services, IHostingEnvironment hostingEnvironment)
        {
            Container container = new Container();
            
            container.Configure(config =>
            {
                config.AddRegistry<CommonRegistry>();

                //if (HostingEnvironment.IsDevelopment())
                //{
                //    config.AddRegistry<DevelopmentRegistry>();
                //}

                config.Populate(services);
            });

            Startup.Container = container;

            return container;
        }
        #endregion

        #region public IServiceProvider ConfigureServices(IServiceCollection services)        
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ConfigureMiddlewareServices(services);
            
            IContainer container = GetConfiguredContainer(services, HostingEnvironment);
            
            return container.GetInstance<IServiceProvider>();
        }
        #endregion

        #endregion

        #region Private Methods

        #region private static void ConfigureMiddlewareServices(IServiceCollection services)        
        /// <summary>
        /// Configures the middleware services.
        /// </summary>
        /// <param name="services">The services.</param>
        private static void ConfigureMiddlewareServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            if (HostingEnvironment.IsEnvironment("IntegrationTest"))
            {
                services.AddDbContext<ManagementAPIReadModel>(builder =>
                        builder.UseInMemoryDatabase("ManagementAPIReadModel"))
                    .AddTransient<ManagementAPIReadModel>();
            }
            else
            {
                String migrationsAssembly = typeof(ManagementAPIReadModel).GetTypeInfo().Assembly.GetName().Name;

                services.AddDbContext<ManagementAPIReadModel>(builder =>
                        builder.UseMySql(ManagementAPIReadModelConnectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                    .AddTransient<ManagementAPIReadModel>();
            }

            services.AddAuthorization(ConfigurePolicies);

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = ConfigurationReader.GetValue("SecurityConfiguration", "Authority");
                    options.RequireHttpsMetadata = false;
                    options.ApiName = ConfigurationReader.GetValue("SecurityConfiguration", "ApiName");
                });

            services.AddMvcCore();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Golf Handicapping API", Version = "v1" });
            });
        }
        #endregion

        #region private async Task InitialiseDatabase(IApplicationBuilder app, IHostingEnvironment environment)
        /// <summary>
        /// Initialises the database.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="environment">The environment.</param>
        private async Task InitialiseDatabase(IApplicationBuilder app, IHostingEnvironment environment)
        {
            using(IServiceScope scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                ManagementAPIReadModel managementApiReadModel = scope.ServiceProvider.GetRequiredService<ManagementAPIReadModel>();

                SeedingType seedingType = Configuration.GetValue<SeedingType>("SeedingType");
                
                DatabaseSeeding.InitialiseDatabase(managementApiReadModel, seedingType);                
            }
        }
        #endregion

        #region private static void ConfigurePolicies(AuthorizationOptions policies)        

        /// <summary>
        /// Configures the policies.
        /// </summary>
        /// <param name="policies">The policies.</param>
        private static void ConfigurePolicies(AuthorizationOptions policies)
        {
            #region Golf Club Policies

            policies.AddPolicy(PolicyNames.CreateGolfClubPolicy, policy =>
            {
                policy.AddAuthenticationSchemes("Bearer");
                policy.RequireAuthenticatedUser();
                policy.RequireRole(new[]
                {
                    RoleNames.ClubAdministrator, RoleNames.ClubAdministrator.ToUpper(),
                });
                policy.RequireClaim(CustomClaims.GolfClubId);
            });

            policies.AddPolicy(PolicyNames.GetGolfClubListPolicy, policy =>
            {
                policy.AddAuthenticationSchemes("Bearer");
                policy.RequireAuthenticatedUser();
                policy.RequireRole(new[]
                {
                    RoleNames.Player, RoleNames.Player.ToUpper()
                });
            });

            policies.AddPolicy(PolicyNames.GetSingleGolfClubPolicy, policy =>
            {
                policy.AddAuthenticationSchemes("Bearer");
                policy.RequireAuthenticatedUser();
                policy.RequireRole(new[]
                {
                    RoleNames.ClubAdministrator, RoleNames.ClubAdministrator.ToUpper(),
                });
                policy.RequireClaim(CustomClaims.GolfClubId);
            });

            policies.AddPolicy(PolicyNames.AddMeasuredCourseToGolfClubPolicy, policy =>
            {
                policy.AddAuthenticationSchemes("Bearer");
                policy.RequireAuthenticatedUser();
                policy.RequireRole(new[]
                {
                    RoleNames.ClubAdministrator, RoleNames.ClubAdministrator.ToUpper(),
                    RoleNames.MatchSecretary, RoleNames.MatchSecretary.ToUpper()
                });
                policy.RequireClaim(CustomClaims.GolfClubId);
            });

            policies.AddPolicy(PolicyNames.RequestClubMembershipPolicy, policy =>
            {
                policy.AddAuthenticationSchemes("Bearer");
                policy.RequireAuthenticatedUser();
                policy.RequireRole(new[]
                {
                    RoleNames.Player, RoleNames.Player.ToUpper()
                });
            });

            #endregion

            #region Player Policies
            
            #endregion

            #region Tournament Policies

            policies.AddPolicy(PolicyNames.CreateTournamentPolicy, policy =>
            {
                policy.AddAuthenticationSchemes("Bearer");
                policy.RequireAuthenticatedUser();
                policy.RequireRole(new[]
                {
                    RoleNames.ClubAdministrator, RoleNames.ClubAdministrator.ToUpper(),
                    RoleNames.MatchSecretary, RoleNames.MatchSecretary.ToUpper()
                });
                policy.RequireClaim(CustomClaims.GolfClubId);
            });

            policies.AddPolicy(PolicyNames.RecordPlayerScoreForTournamentPolicy, policy =>
            {
                policy.AddAuthenticationSchemes("Bearer");
                policy.RequireAuthenticatedUser();
                policy.RequireRole(new[]
                {
                    RoleNames.Player, RoleNames.Player.ToUpper()
                });
            });

            policies.AddPolicy(PolicyNames.CompleteTournamentPolicy, policy =>
            {
                policy.AddAuthenticationSchemes("Bearer");
                policy.RequireAuthenticatedUser();
                policy.RequireRole(new[]
                {
                    RoleNames.ClubAdministrator, RoleNames.ClubAdministrator.ToUpper(),
                    RoleNames.MatchSecretary, RoleNames.MatchSecretary.ToUpper()
                });
            });

            policies.AddPolicy(PolicyNames.CancelTournamentPolicy, policy =>
            {
                policy.AddAuthenticationSchemes("Bearer");
                policy.RequireAuthenticatedUser();
                policy.RequireRole(new[]
                {
                    RoleNames.ClubAdministrator, RoleNames.ClubAdministrator.ToUpper(),
                    RoleNames.MatchSecretary, RoleNames.MatchSecretary.ToUpper()
                });
            });

            policies.AddPolicy(PolicyNames.ProduceTournamentResultPolicy, policy =>
            {
                policy.AddAuthenticationSchemes("Bearer");
                policy.RequireAuthenticatedUser();
                policy.RequireRole(new[]
                {
                    RoleNames.ClubAdministrator, RoleNames.ClubAdministrator.ToUpper(),
                    RoleNames.MatchSecretary, RoleNames.MatchSecretary.ToUpper()
                });
            });

            #endregion
        }

        #endregion
        
        #endregion
    }
}
