namespace ManagementAPI.Service.IntegrationTests.Common
{
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        #region Methods

        /// <summary>
        /// Adds the golf club administrator.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        public static WebApplicationFactory<Startup> AddGolfClubAdministrator(this WebApplicationFactory<Startup> factory)
        {
            return factory.WithWebHostBuilder(builder =>
                                              {
                                                  builder.ConfigureServices(services =>
                                                                            {
                                                                                services.AddMvc(options =>
                                                                                                {
                                                                                                    options.Filters.Add(new AllowAnonymousFilter());
                                                                                                    options.Filters.Add(new FakeGolfClubAdminUserFilter());
                                                                                                });
                                                                            });
                                              });
        }

        #endregion
    }
}