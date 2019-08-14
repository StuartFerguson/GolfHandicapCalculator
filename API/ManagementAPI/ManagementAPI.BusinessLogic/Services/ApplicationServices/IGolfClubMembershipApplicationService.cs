namespace ManagementAPI.BusinessLogic.Services.ApplicationServices
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IGolfClubMembershipApplicationService
    {
        Task RequestClubMembership(Guid playerId, Guid golfClubId, CancellationToken cancellationToken);
    }
}