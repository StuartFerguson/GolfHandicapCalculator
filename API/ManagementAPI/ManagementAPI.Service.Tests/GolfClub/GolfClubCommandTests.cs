using System;
using ManagementAPI.Service.Commands;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.GolfClub
{
    public class GolfClubCommandTests
    {
        [Fact]
        public void CreateGolfClubCommand_CanBeCreated_IsCreated()
        {
            CreateGolfClubCommand command = CreateGolfClubCommand.Create(GolfClubTestData.CreateGolfClubRequest);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
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
    }
}
