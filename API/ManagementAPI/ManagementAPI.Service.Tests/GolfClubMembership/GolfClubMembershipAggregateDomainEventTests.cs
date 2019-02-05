using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ManagementAPI.GolfClubMembership.DomainEvents;
using Shouldly;

namespace ManagementAPI.Service.Tests.GolfClubMembership
{
    public class GolfClubMembershipAggregateDomainEventTests
    {
        [Fact]
        public void ClubMembershipRequestAcceptedEvent_CanBeCreated_IsCreated()
        {
            ClubMembershipRequestAcceptedEvent clubMembershipRequestAcceptedEvent = ClubMembershipRequestAcceptedEvent.Create(GolfClubMembershipTestData.AggregateId,
                GolfClubMembershipTestData.MembershipId, GolfClubMembershipTestData.PlayerId, 
                GolfClubMembershipTestData.PlayerFullName, GolfClubMembershipTestData.PlayerDateOfBirth,
                GolfClubMembershipTestData.PlayerGender, GolfClubMembershipTestData.AcceptedDateAndTime,
                GolfClubMembershipTestData.MembershipNumber);

            clubMembershipRequestAcceptedEvent.ShouldNotBeNull();
            clubMembershipRequestAcceptedEvent.AggregateId.ShouldBe(GolfClubMembershipTestData.AggregateId);
            clubMembershipRequestAcceptedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            clubMembershipRequestAcceptedEvent.EventId.ShouldNotBe(Guid.Empty);
            clubMembershipRequestAcceptedEvent.PlayerId.ShouldBe(GolfClubMembershipTestData.PlayerId);
            clubMembershipRequestAcceptedEvent.PlayerGender.ShouldBe(GolfClubMembershipTestData.PlayerGender);
            clubMembershipRequestAcceptedEvent.PlayerId.ShouldBe(GolfClubMembershipTestData.PlayerId);
            clubMembershipRequestAcceptedEvent.PlayerDateOfBirth.ShouldBe(GolfClubMembershipTestData.PlayerDateOfBirth);
            clubMembershipRequestAcceptedEvent.AcceptedDateAndTime.ShouldBe(GolfClubMembershipTestData.AcceptedDateAndTime);
            clubMembershipRequestAcceptedEvent.MembershipNumber.ShouldBe(GolfClubMembershipTestData.MembershipNumber);
        }

        [Fact]
        public void ClubMembershipRequestRejectedEvent_CanBeCreated_IsCreated()
        {
            ClubMembershipRequestRejectedEvent clubMembershipRequestRejectedEvent = ClubMembershipRequestRejectedEvent.Create(GolfClubMembershipTestData.AggregateId,
                GolfClubMembershipTestData.MembershipId, GolfClubMembershipTestData.PlayerId, 
                GolfClubMembershipTestData.PlayerFullName, GolfClubMembershipTestData.PlayerDateOfBirth,
                GolfClubMembershipTestData.PlayerGender, 
                GolfClubMembershipTestData.RejectionReason,
                GolfClubMembershipTestData.RejectionDateAndTime);

            clubMembershipRequestRejectedEvent.ShouldNotBeNull();
            clubMembershipRequestRejectedEvent.AggregateId.ShouldBe(GolfClubMembershipTestData.AggregateId);
            clubMembershipRequestRejectedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            clubMembershipRequestRejectedEvent.EventId.ShouldNotBe(Guid.Empty);
            clubMembershipRequestRejectedEvent.EventId.ShouldNotBe(Guid.Empty);
            clubMembershipRequestRejectedEvent.PlayerGender.ShouldBe(GolfClubMembershipTestData.PlayerGender);
            clubMembershipRequestRejectedEvent.PlayerId.ShouldBe(GolfClubMembershipTestData.PlayerId);
            clubMembershipRequestRejectedEvent.PlayerDateOfBirth.ShouldBe(GolfClubMembershipTestData.PlayerDateOfBirth);
            clubMembershipRequestRejectedEvent.RejectionReason.ShouldBe(GolfClubMembershipTestData.RejectionReason);
            clubMembershipRequestRejectedEvent.RejectionDateAndTime.ShouldBe(GolfClubMembershipTestData.RejectionDateAndTime);
        }
    }
}
