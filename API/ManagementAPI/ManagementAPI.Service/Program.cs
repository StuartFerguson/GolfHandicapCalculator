using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ManagementAPI.Service
{
    using Microsoft.AspNetCore;
    using Microsoft.Extensions.Logging;

    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(String[] args)
        {
            Program.CreateWebHostBuilder(args).Build().Run();
        }
 
        public static IWebHostBuilder CreateWebHostBuilder(String[] args)
        {
            Console.Title = "Golf Handicapping Management API";

            String environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationRoot config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                  .AddJsonFile("hosting.json", optional: false)
                                                                  .AddJsonFile($"hosting.{environmentName}.json", optional: true)
                                                                  .AddEnvironmentVariables()
                                                                  .Build();

            return WebHost.CreateDefaultBuilder()
                          .UseKestrel()
                          .UseConfiguration(config)
                          .UseContentRoot(Directory.GetCurrentDirectory())
                          .UseStartup<Startup>();
        }
    }
}
