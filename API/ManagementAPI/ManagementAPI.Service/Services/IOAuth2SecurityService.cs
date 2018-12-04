using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.Services.DataTransferObjects;

namespace ManagementAPI.Service.Services
{
    public interface IOAuth2SecurityService
    {
        /// <summary>
        /// Registers the player user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<RegisterUserResponse> RegisterPlayerUser(RegisterUserRequest request, CancellationToken cancellationToken);
    }
}
