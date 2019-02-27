namespace MobileAppConfigurationAPI
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Database;
    using Database.SeedData;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NLog.Extensions.Logging;
    using Shared.Extensions;
    using Shared.General;
    using StructureMap;
    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        #region Fields

        /// <summary>
        /// The management API read model connection string
        /// </summary>
        private static String MobileConfigurationConnectionString;

        #endregion

        #region Constructors

        public Startup(IHostingEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                                                                      .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
                                                                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional:true).AddEnvironmentVariables();

            Startup.Configuration = builder.Build();
            Startup.HostingEnvironment = env;

            // Get the DB Connection Strings
            Startup.MobileConfigurationConnectionString = Startup.Configuration.GetConnectionString(nameof(MobileConfiguration));
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              ILoggerFactory loggerFactory)
        {
            String nlogConfigFilename = "nlog.config";
            //if (string.Compare(Startup.HostingEnvironment.EnvironmentName, "Development", true) == 0)
            //{
            //    nlogConfigFilename = $"nlog.{Startup.HostingEnvironment.EnvironmentName}.config";
            //}

            loggerFactory.AddConsole();
            loggerFactory.ConfigureNLog(Path.Combine(Startup.HostingEnvironment.ContentRootPath, nlogConfigFilename));
            loggerFactory.AddNLog();

            ILogger logger = loggerFactory.CreateLogger("GolfHandicappingMobileApp");

            Logger.Initialise(logger);
            Logger.LogInformation("Hello from Logger.");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.AddExceptionHandler();
            app.AddRequestLogging();
            app.AddResponseLogging();

            // Setup the database
            this.InitialiseDatabase(app, env).Wait();

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Golf Handicapping Mobile Config API v1"); });
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

            container.Configure(config => { config.Populate(services); });

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

            String migrationsAssembly = typeof(MobileConfiguration).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<MobileConfiguration>(builder => builder.UseMySql(Startup.MobileConfigurationConnectionString,
                                                                                   sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
                    .AddTransient<MobileConfiguration>();

            services.AddMvcCore();

            services.AddSwaggerGen(c =>
                                   {
                                       c.SwaggerDoc("v1",
                                                    new Info
                                                    {
                                                        Title = "Golf Handicapping Mobile Config API",
                                                        Version = "v1"
                                                    });
                                   });
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
                MobileConfiguration mobileConfiguration = scope.ServiceProvider.GetRequiredService<MobileConfiguration>();

                DatabaseSeeding.InitialiseDatabase(mobileConfiguration);
            }
        }

        #endregion
    }
}