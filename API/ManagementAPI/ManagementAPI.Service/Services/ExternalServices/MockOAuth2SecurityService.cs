namespace ManagementAPI.Service.Services.ExternalServices
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects;

    [ExcludeFromCodeCoverage]
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ManagementAPI.Service.Services.IOAuth2SecurityService" />
    public class MockOAuth2SecurityService : IOAuth2SecurityService
    {
        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            return new RegisterUserResponse
            {
                UserId = Guid.NewGuid()
            };
        }

        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateRoleResponse> CreateRole(CreateRoleRequest request,
                                     CancellationToken cancellationToken)
        {
            return new CreateRoleResponse
                   {
                       RoleId = Guid.NewGuid()
                   };
        }

        /// <summary>
        /// Gets the name of the role by.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetRoleResponse> GetRoleByName(String roleName,
                                        CancellationToken cancellationToken)
        {
            return new GetRoleResponse
                   {
                       Id = Guid.NewGuid(),
                       Name = roleName,
                       NormalizedName = roleName.ToUpper()
                   };
        }
    }
}