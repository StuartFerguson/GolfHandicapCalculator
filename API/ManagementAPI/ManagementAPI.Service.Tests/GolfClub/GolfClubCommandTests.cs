using System;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.DataTransferObjects;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.GolfClub
{
    public class GolfClubCommandTests
    {
        [Fact]
        public void CreateGolfClubCommand_CanBeCreated_IsCreated()
        {
            CreateGolfClubCommand command = CreateGolfClubCommand.Create(GolfClubTestData.AggregateId, GolfClubTestData.GolfClubAdministratorSecurityUserId, GolfClubTestData.CreateGolfClubRequest);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            command.SecurityUserId.ShouldBe(GolfClubTestData.GolfClubAdministratorSecurityUserId);
            command.CreateGolfClubRequest.ShouldNotBeNull();
            command.CreateGolfClubRequest.ShouldBe(GolfClubTestData.CreateGolfClubRequest);
        }

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
        public void RequestClubMembershipCommand_CanBeCreated_IsCreated()
        {
            RequestClubMembershipCommand command = RequestClubMembershipCommand.Create(GolfClubTestData.PlayerId, GolfClubTestData.AggregateId);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.PlayerId.ShouldBe(GolfClubTestData.PlayerId);
            command.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
        }
    }
}
