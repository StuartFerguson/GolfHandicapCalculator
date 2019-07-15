using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Services
{
    using System;
    using ExternalServices.DataTransferObjects;

    public interface ISecurityService
    {
        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<CreateRoleResponse> CreateRole(CreateRoleRequest request,
                                            CancellationToken cancellationToken);
        /// <summary>
        /// Gets the name of the role by.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetRoleResponse> GetRoleByName(String roleName,
                                            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetUserResponse> GetUserById(Guid userId,
                                            CancellationToken cancellationToken);
    }
}
