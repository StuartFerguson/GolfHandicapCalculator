using System;
using System.Collections.Generic;
using System.Text;
using ManagementAPI.Service.Commands;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.Player
{
    public class PlayerCommandTests
    {
        [Fact]
        public void RegisterPlayerCommand_CanBeCreated_IsCreated()
        {
            RegisterPlayerCommand command = RegisterPlayerCommand.Create(PlayerTestData.RegisterPlayerRequest);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.RegisterPlayerRequest.ShouldNotBeNull();
            command.RegisterPlayerRequest.ShouldBe(PlayerTestData.RegisterPlayerRequest);
        }

        [Fact]
        public void AddAcceptedMembershipToPlayerCommand_CanBeCreated_IsCreated()
        {
            AddAcceptedMembershipToPlayerCommand command = AddAcceptedMembershipToPlayerCommand.Create(
                PlayerTestData.AggregateId, PlayerTestData.GolfClubId, PlayerTestData.MembershipId,
                PlayerTestData.MembershipNumber,
                PlayerTestData.MembershipAcceptedDateTime);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.PlayerId.ShouldBe(PlayerTestData.AggregateId);
            command.GolfClubId.ShouldBe(PlayerTestData.GolfClubId);
            command.MembershipId.ShouldBe(PlayerTestData.MembershipId);
            command.MembershipNumber.ShouldBe(PlayerTestData.MembershipNumber);
            command.AcceptedDateTime.ShouldBe(PlayerTestData.MembershipAcceptedDateTime);
        }

        [Fact]
        public void AddRejectedMembershipToPlayerCommand_CanBeCreated_IsCreated()
        {
            AddRejectedMembershipToPlayerCommand command = AddRejectedMembershipToPlayerCommand.Create(
                PlayerTestData.AggregateId, PlayerTestData.GolfClubId, PlayerTestData.MembershipId,
                PlayerTestData.RejectionReason,
                PlayerTestData.MembershipRejectedDateTime);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.PlayerId.ShouldBe(PlayerTestData.AggregateId);
            command.GolfClubId.ShouldBe(PlayerTestData.GolfClubId);
            command.MembershipId.ShouldBe(PlayerTestData.MembershipId);
            command.RejectionReason.ShouldBe(PlayerTestData.RejectionReason);
            command.RejectedDateTime.ShouldBe(PlayerTestData.MembershipRejectedDateTime);
        }
    }
}
