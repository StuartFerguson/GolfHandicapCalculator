using System.Diagnostics.CodeAnalysis;
using StructureMap;

namespace ManagementAPI.Service.Bootstrapper
{
    using BusinessLogic.Services.ExternalServices;

    [ExcludeFromCodeCoverage]
    public class DevelopmentRegistry : Registry
    {
        public DevelopmentRegistry()
        {
            For<ISecurityService>().Use<MockSecurityService>().Singleton();
        }
    }
}