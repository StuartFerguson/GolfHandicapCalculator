using System.Collections.Generic;

namespace ManagementAPI.Service.Client.v2
{
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using DataTransferObjects.Responses.v2;

    public interface IGolfClubAdministratorClient
    {
        /// <summary>
        /// Registers the golf club administrator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<RegisterClubAdministratorResponse> RegisterGolfClubAdministrator(RegisterClubAdministratorRequest request,
                                           CancellationToken cancellationToken);
    }
}
