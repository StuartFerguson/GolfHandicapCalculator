namespace ManagementAPI.Service.Services.DomainServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GolfClubMembership;
    using Player;
    using Shared.EventStore;
    using Shared.Exceptions;
    using Tournament;

    public class TournamentApplicationService : ITournamentApplicationService
    {
        private readonly IAggregateRepository<TournamentAggregate> TournamentRepository;

        private readonly IAggregateRepository<PlayerAggregate> PlayerRepository;
        
        public TournamentApplicationService(IAggregateRepository<TournamentAggregate> tournamentRepository,
                                            IAggregateRepository<PlayerAggregate> playerRepository)
        {
            this.TournamentRepository = tournamentRepository;
            this.PlayerRepository = playerRepository;
        }

        public async Task SignUpPlayerForTournament(Guid tournamentId,
                                              Guid playerId, CancellationToken cancellationToken)
        {
            // Validate the tournament Id
            TournamentAggregate tournament = await this.TournamentRepository.GetLatestVersion(tournamentId, cancellationToken);

            PlayerAggregate player = await this.PlayerRepository.GetLatestVersion(playerId, cancellationToken);

            if (!player.HasBeenRegistered)
            {
                throw new InvalidOperationException("A player must be registered to sign up for a club tournament");
            }
            
            try
            {
                List<ClubMembershipDataTransferObject> memberships = player.GetClubMemberships();

                ClubMembershipDataTransferObject membership = memberships.SingleOrDefault(m => m.GolfClubId == tournament.GolfClubId);

                if (membership == null)
                {
                    throw new NotFoundException();
                }
            }
            catch(NotFoundException nex)
            {
                throw new InvalidOperationException("A player must be a member of the club to sign up for a club tournament");
            }

            tournament.SignUpForTournament(playerId);

            await this.TournamentRepository.SaveChanges(tournament, cancellationToken);

        }
    }
}