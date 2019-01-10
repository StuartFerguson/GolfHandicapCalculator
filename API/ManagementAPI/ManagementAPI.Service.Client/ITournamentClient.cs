using System;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;

namespace ManagementAPI.Service.Client
{
    public interface ITournamentClient
    {
        /// <summary>
        /// Creates the tournament.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<CreateTournamentResponse> CreateTournament(String passwordToken, CreateTournamentRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Records the player score.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RecordPlayerScore(String passwordToken, Guid tournamentId, RecordMemberTournamentScoreRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Completes the tournament.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task CompleteTournament(String passwordToken, Guid tournamentId, CancellationToken cancellationToken);

        /// <summary>
        /// Cancels the tournament.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task CancelTournament(String passwordToken, Guid tournamentId, CancelTournamentRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Produces the tournament result.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task ProduceTournamentResult(String passwordToken, Guid tournamentId, CancellationToken cancellationToken);
    }
}