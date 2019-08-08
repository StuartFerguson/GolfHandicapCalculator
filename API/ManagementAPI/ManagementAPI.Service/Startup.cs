using System;
using System.Reflection;
using ManagementAPI.Database;
using ManagementAPI.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shared.General;
using Swashbuckle.AspNetCore.Swagger;
using ESLogger = EventStore.ClientAPI;

namespace ManagementAPI.Service
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Bootstrapper;
    using BusinessLogic.Common;
    using Common;
    using Controllers;
    using Database;
    using Database.SeedData;
    using ManagementAPI.BusinessLogic.EventHandling;
    using ManagementAPI.BusinessLogic.Services.ExternalServices;
    using ManagementAPI.BusinessLogic.Services.ExternalServices.DataTransferObjects;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NLog.Extensions.Logging;
    using Shared.Exceptions;
    using Shared.Extensions;
    using Shared.General;
    using StructureMap;
    using Swashbuckle.AspNetCore.Filters;
    using Swashbuckle.AspNetCore.Swagger;

    [ExcludeFromCodeCoverage]
    public class Startup
    {
        #region Fields

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
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                                                                      .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
                                                                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional:true).AddEnvironmentVariables();

            Startup.Configuration = builder.Build();
            Startup.HostingEnvironment = env;

            // Get the DB Connection Strings
            Startup.ManagementAPIReadModelConnectionString = Startup.Configuration.GetConnectionString(nameof(ManagementAPIReadModel));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public static IConfigurationRoot Configuration { get; set; }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public static IContainer Container { get; private set; }

        /// <summary>
        /// Gets or sets the hosting environment.
        /// </summary>
        /// <value>
        /// The hosting environment.
        /// </value>
        public static IHostingEnvironment HostingEnvironment { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              ILoggerFactory loggerFactory)
        {
            String nlogConfigFilename = "nlog.config";
            if (string.Compare(Startup.HostingEnvironment.EnvironmentName, "Development", true) == 0)
            {
                nlogConfigFilename = $"nlog.{Startup.HostingEnvironment.EnvironmentName}.config";
            }

            loggerFactory.AddConsole();
            loggerFactory.ConfigureNLog(Path.Combine(Startup.HostingEnvironment.ContentRootPath, nlogConfigFilename));
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
            //if (!Startup.HostingEnvironment.IsEnvironment("IntegrationTest"))
            //{
                Task.WaitAll(Task.Run(async () =>
                                      {
                                          // Setup the database
                                          await this.InitialiseDatabase(app, env);

                                          // Setup the security service
                                          await this.InitialiseSecurityRoles(app);
                                      }));
            //}

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

            if (String.Compare(ConfigurationReader.GetValue("EventStoreSettings", "START_PROJECTIONS"),
                               Boolean.TrueString,
                               StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                app.PreWarm(true).Wait();
            }
            else
            {
                app.PreWarm();
            }
        }

        /// <summary>
        /// Initialises the security roles.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns></returns>
        private async Task InitialiseSecurityRoles(IApplicationBuilder app)
        {
            Logger.LogInformation($"In InitialiseSecurityRoles");

            List<String> rolesList = new List<String>();

            rolesList.Add(RoleNames.Developer);
            rolesList.Add(RoleNames.TestDataGenerator);
            rolesList.Add(RoleNames.ClubAdministrator);
            rolesList.Add(RoleNames.MatchSecretary);
            rolesList.Add(RoleNames.Player);

            using(IServiceScope scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                ISecurityService securityService = scope.ServiceProvider.GetRequiredService<ISecurityService>();

                foreach (String roleName in rolesList)
                {

                    Boolean createRole = false;
                    try
                    {
                        Logger.LogInformation($"Getting role {roleName}");
                        await securityService.GetRoleByName(roleName, CancellationToken.None);
                    }
                    catch(Exception nex)
                    {
                        Logger.LogError(nex);
                        if (nex.InnerException != null)
                        {
                            Logger.LogError(nex.InnerException);
                        }

                        Logger.LogInformation($"Role {roleName} not found");
                        createRole = true;
                    }

                    if (createRole)
                    {
                        Logger.LogInformation($"Creating role {roleName}");
                        CreateRoleRequest createRoleRequest = new CreateRoleRequest
                                                              {
                                                                  RoleName = roleName
                                                              };
                        await securityService.CreateRole(createRoleRequest, CancellationToken.None);

                        Logger.LogInformation($"Created role {roleName}");
                    }
                }
            }
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Startup.ConfigureMiddlewareServices(services);

            IContainer container = Startup.GetConfiguredContainer(services, Startup.HostingEnvironment);

            return container.GetInstance<IServiceProvider>();
        }

        /// <summary>
        /// Gets the configured container.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        /// <returns></returns>
        public static IContainer GetConfiguredContainer(IServiceCollection services,
                                                        IHostingEnvironment hostingEnvironment)
        {
            Container container = new Container();

            Dictionary<String, String[]> handlerEventTypesToSilentlyHandle = new Dictionary<String, String[]>();

            if (Startup.Configuration != null)
            {
                IConfigurationSection section = Startup.Configuration.GetSection("AppSettings:HandlerEventTypesToSilentlyHandle");

                if (section != null)
                {
                    Startup.Configuration.GetSection("AppSettings:HandlerEventTypesToSilentlyHandle").Bind(handlerEventTypesToSilentlyHandle);
                }
            }

            DomainEventTypesToSilentlyHandle eventTypesToSilentlyHandle = new DomainEventTypesToSilentlyHandle(handlerEventTypesToSilentlyHandle);

            //Can we create a static method in this class that returns IContainer?
            services.AddSingleton<IDomainEventTypesToSilentlyHandle>(eventTypesToSilentlyHandle);

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

            //if (Startup.HostingEnvironment.IsEnvironment("IntegrationTest"))
            //{
            //    services.AddDbContext<ManagementAPIReadModel>(builder => builder.UseInMemoryDatabase("ManagementAPIReadModel")).AddTransient<ManagementAPIReadModel>();
            //}
            //else
            //{
                String migrationsAssembly = typeof(ManagementAPIReadModel).GetTypeInfo().Assembly.GetName().Name;

                services.AddDbContext<ManagementAPIReadModel>(builder => builder.UseMySql(Startup.ManagementAPIReadModelConnectionString,
                                                                                          sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                        .AddTransient<ManagementAPIReadModel>();
            //}

            //services.AddAuthorization(Startup.ConfigurePolicies);

            services.AddAuthentication("Bearer").AddIdentityServerAuthentication(options =>
                                                                                 {
                                                                                     options.Authority =
                                                                                         ConfigurationReader.GetValue("SecurityConfiguration", "Authority");
                                                                                     options.RequireHttpsMetadata = false;
                                                                                     options.ApiName = ConfigurationReader.GetValue("SecurityConfiguration", "ApiName");
                                                                                 });

            services.AddMvcCore();

            services.AddSwaggerGen(c =>
                                   {
                                       c.SwaggerDoc("v1",
                                                    new Info
                                                    {
                                                        Title = "Golf Handicapping API",
                                                        Version = "v1"
                                                    });
                                       c.ExampleFilters();
                                   });

            services.AddSwaggerExamplesFromAssemblyOf<SwaggerJsonConverter>();
        }

        /// <summary>
        /// Configures the policies.
        /// </summary>
        /// <param name="policies">The policies.</param>
        //private static void ConfigurePolicies(AuthorizationOptions policies)
        //{
        //    #region Golf Club Policies

        //    policies.AddPolicy(PolicyNames.CreateGolfClubPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.ClubAdministrator, RoleNames.ClubAdministrator.ToUpper());
        //                           policy.RequireClaim(CustomClaims.GolfClubId);
        //                       });

        //    policies.AddPolicy(PolicyNames.GetGolfClubListPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.Player, RoleNames.Player.ToUpper());
        //                       });

        //    policies.AddPolicy(PolicyNames.GetSingleGolfClubPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.ClubAdministrator, RoleNames.ClubAdministrator.ToUpper());
        //                           policy.RequireClaim(CustomClaims.GolfClubId);
        //                       });

        //    policies.AddPolicy(PolicyNames.GetGolfClubMembersListPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.ClubAdministrator, RoleNames.ClubAdministrator.ToUpper());
        //                           policy.RequireClaim(CustomClaims.GolfClubId);
        //                       });

        //    policies.AddPolicy(PolicyNames.AddMeasuredCourseToGolfClubPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.ClubAdministrator,
        //                                              RoleNames.ClubAdministrator.ToUpper(),
        //                                              RoleNames.MatchSecretary,
        //                                              RoleNames.MatchSecretary.ToUpper());
        //                           policy.RequireClaim(CustomClaims.GolfClubId);
        //                       });

        //    policies.AddPolicy(PolicyNames.GetMeasuredCoursesPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.ClubAdministrator,
        //                                              RoleNames.ClubAdministrator.ToUpper(),
        //                                              RoleNames.MatchSecretary,
        //                                              RoleNames.MatchSecretary.ToUpper());
        //                           policy.RequireClaim(CustomClaims.GolfClubId);
        //                       });

        //    policies.AddPolicy(PolicyNames.AddTournamentDivisionToGolfClubPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.ClubAdministrator, RoleNames.ClubAdministrator.ToUpper());
        //                           policy.RequireClaim(CustomClaims.GolfClubId);
        //                       });

        //    policies.AddPolicy(PolicyNames.RequestClubMembershipPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.Player, RoleNames.Player.ToUpper());
        //                       });

        //    policies.AddPolicy(PolicyNames.CreateMatchSecretaryPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.ClubAdministrator, RoleNames.ClubAdministrator.ToUpper());
        //                           policy.RequireClaim(CustomClaims.GolfClubId);
        //                       });

        //    policies.AddPolicy(PolicyNames.GetClubUsersListPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.ClubAdministrator, RoleNames.ClubAdministrator.ToUpper());
        //                           policy.RequireClaim(CustomClaims.GolfClubId);
        //                       });

        //    #endregion

        //    #region Player Policies

        //    policies.AddPolicy(PolicyNames.GetPlayerPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.Player, RoleNames.Player.ToUpper());
        //                       });

        //    policies.AddPolicy(PolicyNames.GetPlayerMembershipsPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.Player, RoleNames.Player.ToUpper());
        //                       });

        //    #endregion

        //    #region Tournament Policies

        //    policies.AddPolicy(PolicyNames.CreateTournamentPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.ClubAdministrator,
        //                                              RoleNames.ClubAdministrator.ToUpper(),
        //                                              RoleNames.MatchSecretary,
        //                                              RoleNames.MatchSecretary.ToUpper());
        //                           policy.RequireClaim(CustomClaims.GolfClubId);
        //                       });

        //    policies.AddPolicy(PolicyNames.RecordPlayerScoreForTournamentPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.Player, RoleNames.Player.ToUpper());
        //                           policy.RequireClaim(CustomClaims.PlayerId);
        //                       });

        //    policies.AddPolicy(PolicyNames.PlayerTournamentSignUpPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.Player, RoleNames.Player.ToUpper());
        //                           policy.RequireClaim(CustomClaims.PlayerId);
        //                       });

        //    policies.AddPolicy(PolicyNames.CompleteTournamentPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.ClubAdministrator,
        //                                              RoleNames.ClubAdministrator.ToUpper(),
        //                                              RoleNames.MatchSecretary,
        //                                              RoleNames.MatchSecretary.ToUpper());
        //                       });

        //    policies.AddPolicy(PolicyNames.CancelTournamentPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.ClubAdministrator,
        //                                              RoleNames.ClubAdministrator.ToUpper(),
        //                                              RoleNames.MatchSecretary,
        //                                              RoleNames.MatchSecretary.ToUpper());
        //                       });

        //    policies.AddPolicy(PolicyNames.ProduceTournamentResultPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.ClubAdministrator,
        //                                              RoleNames.ClubAdministrator.ToUpper(),
        //                                              RoleNames.MatchSecretary,
        //                                              RoleNames.MatchSecretary.ToUpper());
        //                       });

        //    policies.AddPolicy(PolicyNames.GetTournamentListPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.ClubAdministrator,
        //                                              RoleNames.ClubAdministrator.ToUpper(),
        //                                              RoleNames.MatchSecretary,
        //                                              RoleNames.MatchSecretary.ToUpper());
        //                       });

        //    #endregion

        //    #region Developer Controller Policies

        //    policies.AddPolicy(PolicyNames.DeveloperControllerPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.Developer,
        //                                              RoleNames.Developer.ToUpper(),
        //                                              RoleNames.TestDataGenerator,
        //                                              RoleNames.TestDataGenerator.ToUpper());
        //                       });

        //    #endregion

        //    #region Handicap Calculation Process Policies

        //    policies.AddPolicy(PolicyNames.ProcessHandicapCalculationsPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.ClubAdministrator,
        //                                              RoleNames.ClubAdministrator.ToUpper(),
        //                                              RoleNames.MatchSecretary,
        //                                              RoleNames.MatchSecretary.ToUpper());
        //                       });

        //    policies.AddPolicy(PolicyNames.GetHandicapCalculationProcessStatusPolicy,
        //                       policy =>
        //                       {
        //                           policy.AddAuthenticationSchemes("Bearer");
        //                           policy.RequireAuthenticatedUser();
        //                           policy.RequireRole(RoleNames.ClubAdministrator,
        //                                              RoleNames.ClubAdministrator.ToUpper(),
        //                                              RoleNames.MatchSecretary,
        //                                              RoleNames.MatchSecretary.ToUpper());
        //                       });

        //    #endregion
        //}

        /// <summary>
        /// Initialises the database.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="environment">The environment.</param>
        private async Task InitialiseDatabase(IApplicationBuilder app,
                                              IHostingEnvironment environment)
        {
            using(IServiceScope scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                ManagementAPIReadModel managementApiReadModel = scope.ServiceProvider.GetRequiredService<ManagementAPIReadModel>();

                SeedingType seedingType = Startup.Configuration.GetValue<SeedingType>("SeedingType");

                DatabaseSeeding.InitialiseDatabase(managementApiReadModel, seedingType);
            }
        }

        #endregion
    }
}