namespace ManagementAPI.BusinessLogic.Services.ApplicationServices
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ITournamentApplicationService
    {
        Task SignUpPlayerForTournament(Guid tournamentId,
                                       Guid playerId,
                                       CancellationToken cancellationToken);
    }
}