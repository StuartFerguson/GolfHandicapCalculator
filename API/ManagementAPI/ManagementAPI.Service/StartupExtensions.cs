namespace ManagementAPI.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using EventStore.ClientAPI.Common.Log;
    using EventStore.ClientAPI.Projections;
    using EventStore.ClientAPI.SystemData;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Shared.EventStore;
    using Shared.General;

    [ExcludeFromCodeCoverage]
    /// <summary>
    /// 
    /// </summary>
    public static class StartupExtensions
    {
        #region Methods

        /// <summary>
        /// Pres the warm.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <param name="startProjections">if set to <c>true</c> [start projections].</param>
        /// <returns></returns>
        public static async Task PreWarm(this IApplicationBuilder applicationBuilder,
                                         Boolean startProjections = false)
        {
            try
            {
                if (startProjections)
                {
                    await StartupExtensions.StartEventStoreProjections();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Starts the event store projections.
        /// </summary>
        /// <returns></returns>
        private static async Task StartEventStoreProjections()
        {
            // TODO: Refactor
            // This method is a brutal way of getting projections to be run when the API starts up
            // This needs refactored into a more pleasant function
            String connString = Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionString");
            String connectionName = Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionName");
            Int32 httpPort = Startup.Configuration.GetValue<Int32>("EventStoreSettings:HttpPort");

            EventStoreConnectionSettings settings = EventStoreConnectionSettings.Create(connString, connectionName, httpPort);

            // Do continuous first
            String continuousProjectionsFolder = ConfigurationReader.GetValue("EventStoreSettings", "ContinuousProjectionsFolder");
            String[] files = Directory.GetFiles(continuousProjectionsFolder, "*.js");

            // TODO: Improve this later to get status of projection before trying to create
            //foreach (String file in files)
            //{
            //    String withoutExtension = Path.GetFileNameWithoutExtension(file);
            //    String projectionBody = File.ReadAllText(file);
            //    Boolean emitEnabled = continuousProjectionsFolder.ToLower().Contains("emitenabled");

            //    await Retry.For(async () =>
            //                    {
            //                        DnsEndPoint d = new DnsEndPoint(settings.IpAddress, settings.HttpPort);

            //                        ProjectionsManager projectionsManager = new ProjectionsManager(new ConsoleLogger(), d, TimeSpan.FromSeconds(5));

            //                        UserCredentials userCredentials = new UserCredentials(settings.UserName, settings.Password);
                                    
            //                        String projectionStatus = await projectionsManager.GetStatusAsync(withoutExtension, userCredentials);

            //                        await projectionsManager.CreateContinuousAsync(withoutExtension, projectionBody, userCredentials);
            //                        Logger.LogInformation("After CreateContinuousAsync");
            //                        if (emitEnabled)
            //                        {
            //                            await projectionsManager.AbortAsync(withoutExtension, userCredentials);
            //                            await projectionsManager.UpdateQueryAsync(withoutExtension, projectionBody, emitEnabled, userCredentials);
            //                            await projectionsManager.EnableAsync(withoutExtension, userCredentials);
            //                        }
            //                    });
            //}
        }

        #endregion
    }
}