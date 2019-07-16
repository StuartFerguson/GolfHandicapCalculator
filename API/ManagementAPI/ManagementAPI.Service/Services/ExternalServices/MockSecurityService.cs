namespace ManagementAPI.Service.Services.ExternalServices
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects;

    [ExcludeFromCodeCoverage]
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ManagementAPI.Service.Services.ISecurityService" />
    public class MockSecurityService : ISecurityService
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

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetUserResponse> GetUserById(Guid userId,
                                      CancellationToken cancellationToken)
        {
            return new GetUserResponse
                   {
                       UserId = Guid.NewGuid(),
                       Email = "test1@testing.co.uk",
                       PhoneNumber = "123456",
                       UserName = "test1@testing.co.uk",
                       Claims = new Dictionary<String, String>(),
                       Roles = new List<String>
                               {
                                   "Golf Club Administrator"
                               }
                   };
        }
    }
}