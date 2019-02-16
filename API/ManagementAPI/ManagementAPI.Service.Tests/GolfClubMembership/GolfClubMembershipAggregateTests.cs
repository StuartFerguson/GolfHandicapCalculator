namespace ManagementAPI.Service.Tests.GolfClubMembership
{
    using System;
    using System.Collections.Generic;
    using ManagementAPI.GolfClubMembership;
    using Shared.Exceptions;
    using Shouldly;
    using Xunit;

    public class GolfClubMembershipAggregateTests
    {
        #region Methods

        [Fact]
        public void GolfClubMembershipAggregate_CanBeCreated_EmptyAggregateId_ErrorThrown()
        {
            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    GolfClubMembershipAggregate aggregate = GolfClubMembershipAggregate.Create(Guid.Empty);
                                                });
        }

        [Fact]
        public void GolfClubMembershipAggregate_CanBeCreated_IsCreated()
        {
            GolfClubMembershipAggregate aggregate = GolfClubMembershipAggregate.Create(GolfClubMembershipTestData.AggregateId);

            aggregate.ShouldNotBeNull();
            aggregate.AggregateId.ShouldBe(GolfClubMembershipTestData.AggregateId);
        }

        [Fact]
        public void GolfClubMembershipAggregate_RequestMembership_DuplicatePlayerMembershipRequest_RequestRejected()
        {
            GolfClubMembershipAggregate aggregate = GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregateWithMembershipRequested();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.RequestMembership(GolfClubMembershipTestData.PlayerId,
                                                                                    GolfClubMembershipTestData.PlayerFullName,
                                                                                    GolfClubMembershipTestData.PlayerDateOfBirth,
                                                                                    GolfClubMembershipTestData.PlayerGender,
                                                                                    GolfClubMembershipTestData.RequestDateAndTime);
                                                    });
        }

        [Theory]
        [InlineData(false, "Player Name", true, "M", typeof(ArgumentNullException))]
        [InlineData(true, "", true, "M", typeof(ArgumentNullException))]
        [InlineData(true, null, true, "M", typeof(ArgumentNullException))]
        [InlineData(true, "Player Name", false, "M", typeof(ArgumentNullException))]
        [InlineData(true, "Player Name", true, "", typeof(ArgumentNullException))]
        [InlineData(true, "Player Name", true, null, typeof(ArgumentNullException))]
        [InlineData(true, "Player Name", true, "MM", typeof(ArgumentOutOfRangeException))]
        [InlineData(true, "Player Name", true, "X", typeof(ArgumentOutOfRangeException))]
        public void GolfClubMembershipAggregate_RequestMembership_InvalidData_ErrorThrown(Boolean validPlayerId,
                                                                                          String playerFullName,
                                                                                          Boolean validDateOfBirth,
                                                                                          String playerGender,
                                                                                          Type exceptionType)
        {
            Guid playerId = validPlayerId ? GolfClubMembershipTestData.PlayerId : Guid.Empty;
            DateTime dateOfBirth = validDateOfBirth ? GolfClubMembershipTestData.PlayerDateOfBirth : DateTime.MinValue;

            GolfClubMembershipAggregate aggregate = GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregate();

            Should.Throw(() => { aggregate.RequestMembership(playerId, playerFullName, dateOfBirth, playerGender, GolfClubMembershipTestData.RequestDateAndTime); },
                         exceptionType);
        }

        [Fact]
        public void GolfClubMembershipAggregate_RequestMembership_MembershipRequested()
        {
            GolfClubMembershipAggregate aggregate = GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregate();

            aggregate.RequestMembership(GolfClubMembershipTestData.PlayerId,
                                        GolfClubMembershipTestData.PlayerFullName,
                                        GolfClubMembershipTestData.PlayerDateOfBirth,
                                        GolfClubMembershipTestData.PlayerGender,
                                        GolfClubMembershipTestData.RequestDateAndTime);

            MembershipDataTransferObject membership = aggregate.GetMembership(GolfClubMembershipTestData.PlayerId,
                                                                              GolfClubMembershipTestData.PlayerDateOfBirth,
                                                                              GolfClubMembershipTestData.PlayerGender);

            membership.MembershipId.ShouldNotBe(Guid.Empty);
            membership.PlayerId.ShouldBe(GolfClubMembershipTestData.PlayerId);
            membership.PlayerFullName.ShouldBe(GolfClubMembershipTestData.PlayerFullName);
            membership.PlayerDateOfBirth.ShouldBe(GolfClubMembershipTestData.PlayerDateOfBirth);
            membership.PlayerGender.ShouldBe(GolfClubMembershipTestData.PlayerGender);
            membership.AcceptedDateAndTime.ShouldBe(GolfClubMembershipTestData.RequestDateAndTime);
            membership.AcceptedDateAndTime.ShouldNotBe(DateTime.MinValue);
            membership.MembershipNumber.ShouldNotBeNullOrEmpty();
            membership.Status.ShouldBe(1);
        }

        [Fact]
        public void GolfClubMembershipAggregate_RequestMembership_NoSpaceForNewMembers_RequestRejected()
        {
            GolfClubMembershipAggregate aggregate = GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregateWithNumberOfAcceptedMembershipRequests(500);

            aggregate.RequestMembership(GolfClubMembershipTestData.PlayerId,
                                        GolfClubMembershipTestData.PlayerFullName,
                                        GolfClubMembershipTestData.PlayerDateOfBirth,
                                        GolfClubMembershipTestData.PlayerGender,
                                        GolfClubMembershipTestData.RequestDateAndTime);

            MembershipDataTransferObject membership = aggregate.GetMembership(GolfClubMembershipTestData.PlayerId,
                                                                              GolfClubMembershipTestData.PlayerDateOfBirth,
                                                                              GolfClubMembershipTestData.PlayerGender);

            membership.MembershipId.ShouldNotBe(Guid.Empty);
            membership.PlayerId.ShouldBe(GolfClubMembershipTestData.PlayerId);
            membership.PlayerFullName.ShouldBe(GolfClubMembershipTestData.PlayerFullName);
            membership.PlayerDateOfBirth.ShouldBe(GolfClubMembershipTestData.PlayerDateOfBirth);
            membership.PlayerGender.ShouldBe(GolfClubMembershipTestData.PlayerGender);
            membership.RejectedDateAndTime.ShouldBe(GolfClubMembershipTestData.RequestDateAndTime);
            membership.AcceptedDateAndTime.ShouldBe(DateTime.MinValue);
            membership.RequestedDateAndTime.ShouldBe(DateTime.MinValue);
            membership.RejectionReason.ShouldBe($"No more space at club for Player Id {GolfClubMembershipTestData.PlayerId}");
            membership.Status.ShouldBe(2);
        }

        [Fact]
        public void GolfClubMembershipAggregate_RequestMembership_PlayerToOld_RequestRejected()
        {
            GolfClubMembershipAggregate aggregate = GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregate();

            // Get the highest valid age
            DateTime dateOfBirth = DateTime.Now.AddYears(-90);

            // Now add a day to make it invalid
            dateOfBirth = dateOfBirth.AddDays(-1);

            aggregate.RequestMembership(GolfClubMembershipTestData.PlayerId,
                                        GolfClubMembershipTestData.PlayerFullName,
                                        dateOfBirth,
                                        GolfClubMembershipTestData.PlayerGender,
                                        GolfClubMembershipTestData.RequestDateAndTime);

            MembershipDataTransferObject membership = aggregate.GetMembership(GolfClubMembershipTestData.PlayerId, dateOfBirth, GolfClubMembershipTestData.PlayerGender);

            membership.MembershipId.ShouldNotBe(Guid.Empty);
            membership.PlayerId.ShouldBe(GolfClubMembershipTestData.PlayerId);
            membership.PlayerFullName.ShouldBe(GolfClubMembershipTestData.PlayerFullName);
            membership.PlayerDateOfBirth.ShouldBe(dateOfBirth);
            membership.PlayerGender.ShouldBe(GolfClubMembershipTestData.PlayerGender);
            membership.RejectedDateAndTime.ShouldBe(GolfClubMembershipTestData.RequestDateAndTime);
            membership.AcceptedDateAndTime.ShouldBe(DateTime.MinValue);
            membership.RequestedDateAndTime.ShouldBe(DateTime.MinValue);
            membership.RejectionReason.ShouldBe($"Player Id {GolfClubMembershipTestData.PlayerId} is too old.");
            membership.Status.ShouldBe(2);
        }

        [Fact]
        public void GolfClubMembershipAggregate_RequestMembership_PlayerToYoung_RequestRejected()
        {
            GolfClubMembershipAggregate aggregate = GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregate();

            // Get the lowest valid age
            DateTime dateOfBirth = DateTime.Now.AddYears(-13);

            // Now add a day to make it invalid
            dateOfBirth = dateOfBirth.AddDays(1);

            aggregate.RequestMembership(GolfClubMembershipTestData.PlayerId,
                                        GolfClubMembershipTestData.PlayerFullName,
                                        dateOfBirth,
                                        GolfClubMembershipTestData.PlayerGender,
                                        GolfClubMembershipTestData.RequestDateAndTime);

            MembershipDataTransferObject membership = aggregate.GetMembership(GolfClubMembershipTestData.PlayerId, dateOfBirth, GolfClubMembershipTestData.PlayerGender);

            membership.MembershipId.ShouldNotBe(Guid.Empty);
            membership.PlayerId.ShouldBe(GolfClubMembershipTestData.PlayerId);
            membership.PlayerFullName.ShouldBe(GolfClubMembershipTestData.PlayerFullName);
            membership.PlayerDateOfBirth.ShouldBe(dateOfBirth);
            membership.PlayerGender.ShouldBe(GolfClubMembershipTestData.PlayerGender);
            membership.RejectedDateAndTime.ShouldBe(GolfClubMembershipTestData.RequestDateAndTime);
            membership.AcceptedDateAndTime.ShouldBe(DateTime.MinValue);
            membership.RequestedDateAndTime.ShouldBe(DateTime.MinValue);
            membership.RejectionReason.ShouldBe($"Player Id {GolfClubMembershipTestData.PlayerId} is too young.");
            membership.Status.ShouldBe(2);
        }

        [Fact]
        public void GolfClubMembershipAggregate_GetMembership_MembershipReturned()
        {
            GolfClubMembershipAggregate aggregate = GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregateWithMembershipRequested();

            MembershipDataTransferObject membership = aggregate.GetMembership(GolfClubMembershipTestData.PlayerId, GolfClubMembershipTestData.PlayerDateOfBirth, GolfClubMembershipTestData.PlayerGender);

            membership.MembershipId.ShouldNotBe(Guid.Empty);
            membership.PlayerId.ShouldBe(GolfClubMembershipTestData.PlayerId);
            membership.PlayerFullName.ShouldBe(GolfClubMembershipTestData.PlayerFullName);
            membership.PlayerDateOfBirth.ShouldBe(GolfClubMembershipTestData.PlayerDateOfBirth);
            membership.PlayerGender.ShouldBe(GolfClubMembershipTestData.PlayerGender);
            membership.AcceptedDateAndTime.ShouldBe(GolfClubMembershipTestData.RequestDateAndTime);
            membership.RejectedDateAndTime.ShouldBe(DateTime.MinValue);
            membership.RequestedDateAndTime.ShouldBe(DateTime.MinValue);
            membership.RejectionReason.ShouldBeNullOrEmpty();
            membership.Status.ShouldBe(1);
        }

        [Fact]
        public void GolfClubMembershipAggregate_GetMembership_NoMemberships_ErrorThrown()
        {
            GolfClubMembershipAggregate aggregate = GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregate();

            Should.Throw<NotFoundException>(() =>
                         {
                             aggregate.GetMembership(GolfClubMembershipTestData.PlayerId,
                                                     GolfClubMembershipTestData.PlayerDateOfBirth,
                                                     GolfClubMembershipTestData.PlayerGender);
                         });
        }

        [Fact]
        public void GolfClubMembershipAggregate_GetMemberships_MembershipsReturned()
        {
            GolfClubMembershipAggregate aggregate = GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregateWithMultipleMembershipRequested();

            List<MembershipDataTransferObject> memberships = aggregate.GetMemberships();

            memberships.Count.ShouldBe(2);
        }

        [Fact]
        public void GolfClubMembershipAggregate_GetMemberships_NoMemberships_ErrorThrown()
        {
            GolfClubMembershipAggregate aggregate = GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregate();

            Should.Throw<NotFoundException>(() =>
                                            {
                                                aggregate.GetMemberships();
                                            });
        }

        #endregion
    }
}