using System;
using System.Threading;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Services
{
    public interface IGolfClubMembershipApplicationService
    {
        Task RequestClubMembership(Guid playerId, Guid golfClubId, CancellationToken cancellationToken);
    }
}