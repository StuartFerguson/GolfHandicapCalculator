namespace ManagementAPI.Service.Tests.GolfClub
{
    using System;
    using BusinessLogic.Commands;
    using Shouldly;
    using Xunit;

    public class GolfClubCommandTests
    {
        #region Methods

        [Fact]
        public void AddMeasuredCourseToClubCommand_CanBeCreated_IsCreated()
        {
            AddMeasuredCourseToClubCommand command = AddMeasuredCourseToClubCommand.Create(GolfClubTestData.AggregateId, GolfClubTestData.AddMeasuredCourseToClubRequest);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.AddMeasuredCourseToClubRequest.ShouldNotBeNull();
            command.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            command.AddMeasuredCourseToClubRequest.ShouldBe(GolfClubTestData.AddMeasuredCourseToClubRequest);
        }

        [Fact]
        public void AddTournamentDivisionToGolfClubCommand_CanBeCreated_IsCreated()
        {
            AddTournamentDivisionToGolfClubCommand command =
                AddTournamentDivisionToGolfClubCommand.Create(GolfClubTestData.AggregateId, GolfClubTestData.AddTournamentDivisionToGolfClubRequest);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            command.AddTournamentDivisionToGolfClubRequest.ShouldNotBeNull();
            command.AddTournamentDivisionToGolfClubRequest.ShouldBe(GolfClubTestData.AddTournamentDivisionToGolfClubRequest);
        }

        [Fact]
        public void CreateGolfClubCommand_CanBeCreated_IsCreated()
        {
            CreateGolfClubCommand command = CreateGolfClubCommand.Create(GolfClubTestData.AggregateId,
                                                                         GolfClubTestData.GolfClubAdministratorSecurityUserId,
                                                                         GolfClubTestData.CreateGolfClubRequest);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            command.SecurityUserId.ShouldBe(GolfClubTestData.GolfClubAdministratorSecurityUserId);
            command.CreateGolfClubRequest.ShouldNotBeNull();
            command.CreateGolfClubRequest.ShouldBe(GolfClubTestData.CreateGolfClubRequest);
        }

        [Fact]
        public void CreateMatchSecretaryCommand_CanBeCreated_IsCreated()
        {
            CreateMatchSecretaryCommand command = CreateMatchSecretaryCommand.Create(GolfClubTestData.AggregateId, GolfClubTestData.CreateMatchSecretaryRequest);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            command.CreateMatchSecretaryRequest.ShouldNotBeNull();
            command.CreateMatchSecretaryRequest.ShouldBe(GolfClubTestData.CreateMatchSecretaryRequest);
        }

        [Fact]
        public void RequestClubMembershipCommand_CanBeCreated_IsCreated()
        {
            RequestClubMembershipCommand command = RequestClubMembershipCommand.Create(GolfClubTestData.PlayerId, GolfClubTestData.AggregateId);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.PlayerId.ShouldBe(GolfClubTestData.PlayerId);
            command.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
        }

        #endregion
    }
}