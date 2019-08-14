namespace ManagementAPI.BusinessLogic.Services.ApplicationServices
{
    using System;
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

        private readonly IAggregateRepository<GolfClubMembershipAggregate> ClubMembershipRepository;
        
        public TournamentApplicationService(IAggregateRepository<TournamentAggregate> tournamentRepository,
                                            IAggregateRepository<PlayerAggregate> playerRepository,
                                            IAggregateRepository<GolfClubMembershipAggregate> clubMembershipRepository)
        {
            this.TournamentRepository = tournamentRepository;
            this.PlayerRepository = playerRepository;
            this.ClubMembershipRepository = clubMembershipRepository;
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
                GolfClubMembershipAggregate golfClubMembership= await this.ClubMembershipRepository.GetLatestVersion(tournament.GolfClubId, cancellationToken);

                golfClubMembership.GetMembership(player.AggregateId, player.DateOfBirth, player.Gender);                
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