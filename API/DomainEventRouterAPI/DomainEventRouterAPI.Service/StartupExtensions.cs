namespace DomainEventRouterAPI.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;

    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        public static async Task PreWarm(this IApplicationBuilder applicationBuilder)
        {
            try
            {
                Startup.Container.AssertConfigurationIsValid();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;

            }
        }
    }
}