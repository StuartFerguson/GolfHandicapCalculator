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
    using GolfClub.DomainEvents;
    using ManagementAPI.BusinessLogic.EventHandling;
    using ManagementAPI.BusinessLogic.Services.ExternalServices;
    using ManagementAPI.BusinessLogic.Services.ExternalServices.DataTransferObjects;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.JsonPatch.Operations;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NLog.Extensions.Logging;
    using Shared.Exceptions;
    using Shared.Extensions;
    using Shared.General;
    using StructureMap;
    using Swashbuckle.AspNetCore.Filters;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

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
                              ILoggerFactory loggerFactory,
                              IApiVersionDescriptionProvider provider)
        {
            String nlogConfigFilename = "nlog.config";
            //if (string.Compare(Startup.HostingEnvironment.EnvironmentName, "Development", true) == 0)
            //{
            //    nlogConfigFilename = $"nlog.{Startup.HostingEnvironment.EnvironmentName}.config";
            //}

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
                Task.WaitAll(Task.Run(async () =>
                                      {
                                          // Setup the database
                                          await this.InitialiseDatabase(app, env);
                                      }));

            app.AddExceptionHandler();
            app.AddRequestLogging();
            app.AddResponseLogging();

            app.UseAuthentication();

            app.UseMvc();
            app.UseSwagger();

            app.UseSwaggerUI(
                             options =>
                             {
                                 // build a swagger endpoint for each discovered API version
                                 foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                                 {
                                     options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                                 }
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
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            this.ConfigureMiddlewareServices(services);

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
                                });

            container.Populate(services);

            Startup.Container = container;

            return Startup.Container;
        }

        /// <summary>
        /// Configures the middleware services.
        /// </summary>
        /// <param name="services">The services.</param>
        private void ConfigureMiddlewareServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options =>
                                             {
                                                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                                                 options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                                                 options.SerializerSettings.Formatting = Formatting.Indented;
                                                 options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                                                 options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                                             }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            String migrationsAssembly = typeof(ManagementAPIReadModel).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ManagementAPIReadModel>(builder => builder.UseSqlServer(Startup.ManagementAPIReadModelConnectionString,
                                                                                          sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                    .AddTransient<ManagementAPIReadModel>();

            services.AddAuthentication("Bearer").AddIdentityServerAuthentication(options =>
                                                                                 {
                                                                                     options.Authority =
                                                                                         ConfigurationReader.GetValue("SecurityConfiguration", "Authority");
                                                                                     options.RequireHttpsMetadata = false;
                                                                                     options.ApiName = ConfigurationReader.GetValue("SecurityConfiguration", "ApiName");
                                                                                 });

            services.AddApiVersioning(
                                      options =>
                                      {
                                          // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                                          options.ReportApiVersions = true;
                                          options.DefaultApiVersion = new ApiVersion(1, 0);
                                          options.AssumeDefaultVersionWhenUnspecified = true;
                                          options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                                      });

            services.AddVersionedApiExplorer(
                                             options =>
                                             {
                                                 // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                                                 // note: the specified format code will format the version as "'v'major[.minor][-status]"
                                                 options.GroupNameFormat = "'v'VVV";

                                                 // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                                                 // can also be used to control the format of the API version in route templates
                                                 options.SubstituteApiVersionInUrl = true;
                                             });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(c =>
                                   {
                                       // add a custom operation filter which sets default values
                                       c.OperationFilter<SwaggerDefaultValues>();
                                       c.ExampleFilters();
                                   });

            services.AddSwaggerExamplesFromAssemblyOf<SwaggerJsonConverter>();
        }

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