using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.Services.DataTransferObjects;

namespace ManagementAPI.Service.Services
{
    public interface IOAuth2SecurityService
    {
        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken);
    }
}
