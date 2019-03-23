using System;
using ManagementAPI.Service.Commands;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.Tournament
{
    public class TournamentCommandTests
    {
        [Fact]
        public void CreateTournamentCommand_CanBeCreated_IsCreated()
        {
            CreateTournamentCommand command = CreateTournamentCommand.Create(TournamentTestData.GolfClubId, TournamentTestData.CreateTournamentRequest);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.GolfClubId.ShouldBe(TournamentTestData.GolfClubId);
            command.CreateTournamentRequest.ShouldNotBeNull();
            command.CreateTournamentRequest.ShouldBe(TournamentTestData.CreateTournamentRequest); 
        }

        [Fact]
        public void RecordMemberTournamentScoreCommand_CanBeCreated_IsCreated()
        {
            RecordPlayerTournamentScoreCommand command =
                RecordPlayerTournamentScoreCommand.Create(TournamentTestData.PlayerId, TournamentTestData.AggregateId, TournamentTestData.RecordMemberTournamentScoreRequest);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.TournamentId.ShouldBe(TournamentTestData.AggregateId);
            command.PlayerId.ShouldBe(TournamentTestData.PlayerId);
            command.RecordPlayerTournamentScoreRequest.ShouldNotBeNull();
            command.RecordPlayerTournamentScoreRequest.ShouldBe(TournamentTestData.RecordMemberTournamentScoreRequest); 
        }

        [Fact]
        public void CompleteTournamentCommand_CanBeCreated_IsCreated()
        {
            CompleteTournamentCommand command = CompleteTournamentCommand.Create(TournamentTestData.AggregateId);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.TournamentId.ShouldBe(TournamentTestData.AggregateId);
        }

        [Fact]
        public void CancelTournamentCommand_CanBeCreated_IsCreated()
        {
            CancelTournamentCommand command =
                CancelTournamentCommand.Create(TournamentTestData.AggregateId, TournamentTestData.CancelTournamentRequest);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.TournamentId.ShouldBe(TournamentTestData.AggregateId);
            command.CancelTournamentRequest.ShouldNotBeNull();
            command.CancelTournamentRequest.ShouldBe(TournamentTestData.CancelTournamentRequest); 
        }

        [Fact]
        public void ProduceTournamentResultCommand_CanBeCreated_IsCreated()
        {
            ProduceTournamentResultCommand command =
                ProduceTournamentResultCommand.Create(TournamentTestData.AggregateId);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.TournamentId.ShouldBe(TournamentTestData.AggregateId);
        }

        [Fact]
        public void SignUpForTournamentCommand_CanBeCreated_IsCreated()
        {
            SignUpForTournamentCommand command = SignUpForTournamentCommand.Create(TournamentTestData.AggregateId, TournamentTestData.PlayerId);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.TournamentId.ShouldBe(TournamentTestData.AggregateId);
            command.PlayerId.ShouldBe(TournamentTestData.PlayerId);
        }
    }
}
