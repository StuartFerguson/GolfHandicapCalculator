using System;
using ManagementAPI.Service.Commands;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.ClubConfiguration
{
    public class ClubConfigurationCommandTests
    {
        [Fact]
        public void ClubConfigurationCommand_CanBeCreated_IsCreated()
        {
            CreateClubConfigurationCommand command = CreateClubConfigurationCommand.Create(ClubConfigurationTestData.CreateClubConfigurationRequest);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.CreateClubConfigurationRequest.ShouldNotBeNull();
            command.CreateClubConfigurationRequest.ShouldBe(ClubConfigurationTestData.CreateClubConfigurationRequest);
        }

        [Fact]
        public void AddMeasuredCourseToClubCommand_CanBeCreated_IsCreated()
        {
            AddMeasuredCourseToClubCommand command = AddMeasuredCourseToClubCommand.Create(ClubConfigurationTestData.AggregateId, ClubConfigurationTestData.AddMeasuredCourseToClubRequest);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.AddMeasuredCourseToClubRequest.ShouldNotBeNull();
            command.ClubConfigurationId.ShouldBe(ClubConfigurationTestData.AggregateId);
            command.AddMeasuredCourseToClubRequest.ShouldBe(ClubConfigurationTestData.AddMeasuredCourseToClubRequest);
        }
    }
}
