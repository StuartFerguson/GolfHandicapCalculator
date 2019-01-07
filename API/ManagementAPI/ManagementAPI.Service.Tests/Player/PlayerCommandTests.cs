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
        public void PlayerClubMembershipRequestCommand_CanBeCreated_IsCreated()
        {
            PlayerClubMembershipRequestCommand command = PlayerClubMembershipRequestCommand.Create(PlayerTestData.AggregateId, PlayerTestData.ClubId);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.PlayerId.ShouldBe(PlayerTestData.AggregateId);
            command.ClubId.ShouldBe(PlayerTestData.ClubId);
        }

        [Fact]
        public void ApprovePlayerMembershipRequestCommand_CanBeCreated_IsCreated()
        {
            ApprovePlayerMembershipRequestCommand command = ApprovePlayerMembershipRequestCommand.Create(PlayerTestData.AggregateId, PlayerTestData.ClubId);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.PlayerId.ShouldBe(PlayerTestData.AggregateId);
            command.ClubId.ShouldBe(PlayerTestData.ClubId);
        }

        [Fact]
        public void RejectPlayerMembershipRequestCommand_CanBeCreated_IsCreated()
        {
            RejectPlayerMembershipRequestCommand command = RejectPlayerMembershipRequestCommand.Create(PlayerTestData.AggregateId, PlayerTestData.ClubId, PlayerTestData.RejectMembershipRequestRequest);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.PlayerId.ShouldBe(PlayerTestData.AggregateId);
            command.ClubId.ShouldBe(PlayerTestData.ClubId);
            command.RejectMembershipRequestRequest.ShouldBe(PlayerTestData.RejectMembershipRequestRequest);
        }
    }
}
