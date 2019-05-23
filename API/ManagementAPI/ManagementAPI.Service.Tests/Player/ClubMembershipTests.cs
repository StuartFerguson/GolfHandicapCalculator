namespace ManagementAPI.Service.Tests.Player
{
    using System;
    using ManagementAPI.Player;
    using Shouldly;
    using Xunit;

    public class ClubMembershipTests
    {
        #region Methods

        [Fact]
        public void ClubMembership_Approve_MembershipApproved()
        {
            ClubMembership clubMembership = ClubMembership.Create();
            clubMembership.Approve(PlayerTestData.GolfClubId, PlayerTestData.MembershipId, PlayerTestData.MembershipNumber, PlayerTestData.MembershipAcceptedDateTime);

            clubMembership.MembershipId.ShouldBe(PlayerTestData.MembershipId);
            clubMembership.MembershipNumber.ShouldBe(PlayerTestData.MembershipNumber);
            clubMembership.AcceptedDateTime.ShouldBe(PlayerTestData.MembershipAcceptedDateTime);
            clubMembership.RejectionReason.ShouldBeNullOrEmpty();
            clubMembership.RejectedDateTime.ShouldBe(DateTime.MinValue);
            clubMembership.Status.ShouldBe(ClubMembership.MembershipStatus.Approved);
        }

        [Fact]
        public void ClubMembership_Create_IsCreated()
        {
            ClubMembership clubMembership = ClubMembership.Create();
            clubMembership.ShouldNotBeNull();
            clubMembership.Status.ShouldBe(ClubMembership.MembershipStatus.Pending);
        }

        [Fact]
        public void ClubMembership_Reject_MembershipRejected()
        {
            ClubMembership clubMembership = ClubMembership.Create();
            clubMembership.Reject(PlayerTestData.GolfClubId, PlayerTestData.MembershipId, PlayerTestData.RejectionReason, PlayerTestData.MembershipRejectedDateTime);

            clubMembership.MembershipId.ShouldBe(PlayerTestData.MembershipId);
            clubMembership.MembershipNumber.ShouldBeNullOrEmpty();
            clubMembership.AcceptedDateTime.ShouldBe(DateTime.MinValue);
            clubMembership.RejectionReason.ShouldBe(PlayerTestData.RejectionReason);
            clubMembership.RejectedDateTime.ShouldBe(PlayerTestData.MembershipRejectedDateTime);
            clubMembership.Status.ShouldBe(ClubMembership.MembershipStatus.Rejected);
        }

        #endregion
    }
}