using System.Diagnostics.CodeAnalysis;
using ManagementAPI.Service.Services;
using StructureMap;

namespace ManagementAPI.Service.Bootstrapper
{
    using Services.ExternalServices;

    [ExcludeFromCodeCoverage]
    public class DevelopmentRegistry : Registry
    {
        public DevelopmentRegistry()
        {
            For<IOAuth2SecurityService>().Use<MockOAuth2SecurityService>().Singleton();
        }
    }
}