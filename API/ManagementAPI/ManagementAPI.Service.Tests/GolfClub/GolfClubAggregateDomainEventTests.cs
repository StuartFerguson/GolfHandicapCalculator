using System;
using ManagementAPI.GolfClub.DomainEvents;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.GolfClub
{
    public class GolfClubAggregateDomainEventTests
    {
        [Fact]
        public void GolfClubCreatedEvent_CanBeCreated_IsCreated()
        {
            GolfClubCreatedEvent golfClubCreatedEvent = GolfClubCreatedEvent.Create(GolfClubTestData.AggregateId,
                GolfClubTestData.Name, 
                GolfClubTestData.AddressLine1,
                GolfClubTestData.AddressLine2,
                GolfClubTestData.Town,
                GolfClubTestData.Region,
                GolfClubTestData.PostalCode,
                GolfClubTestData.TelephoneNumber,
                GolfClubTestData.Website,
                GolfClubTestData.EmailAddress);

            golfClubCreatedEvent.ShouldNotBeNull();
            golfClubCreatedEvent.AggregateId.ShouldBe(GolfClubTestData.AggregateId);
            golfClubCreatedEvent.Name.ShouldBe(GolfClubTestData.Name);
            golfClubCreatedEvent.AddressLine1.ShouldBe(GolfClubTestData.AddressLine1);
            golfClubCreatedEvent.AddressLine2.ShouldBe(GolfClubTestData.AddressLine2);
            golfClubCreatedEvent.Town.ShouldBe(GolfClubTestData.Town);
            golfClubCreatedEvent.Region.ShouldBe(GolfClubTestData.Region);
            golfClubCreatedEvent.PostalCode.ShouldBe(GolfClubTestData.PostalCode);
            golfClubCreatedEvent.TelephoneNumber.ShouldBe(GolfClubTestData.TelephoneNumber);
            golfClubCreatedEvent.Website.ShouldBe(GolfClubTestData.Website);
            golfClubCreatedEvent.EmailAddress.ShouldBe(GolfClubTestData.EmailAddress);
            golfClubCreatedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            golfClubCreatedEvent.EventId.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public void MeasuredCourseAddedEvent_CanBeCreated_IsCreated()
        {
            MeasuredCourseAddedEvent measuredCourseAddedEvent = MeasuredCourseAddedEvent.Create(GolfClubTestData.AggregateId, 
                GolfClubTestData.MeasuredCourseId, GolfClubTestData.MeasuredCourseName, GolfClubTestData.TeeColour,
                GolfClubTestData.StandardScratchScore);

            measuredCourseAddedEvent.ShouldNotBeNull();
            measuredCourseAddedEvent.AggregateId.ShouldBe(GolfClubTestData.AggregateId);
            measuredCourseAddedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            measuredCourseAddedEvent.EventId.ShouldNotBe(Guid.Empty);
            measuredCourseAddedEvent.MeasuredCourseId.ShouldBe(GolfClubTestData.MeasuredCourseId);
            measuredCourseAddedEvent.Name.ShouldBe(GolfClubTestData.MeasuredCourseName);
            measuredCourseAddedEvent.TeeColour.ShouldBe(GolfClubTestData.TeeColour);
            measuredCourseAddedEvent.StandardScratchScore.ShouldBe(GolfClubTestData.StandardScratchScore);            
        }

        [Fact]
        public void HoleAddedToMeasuredCourseEvent_CanBeCreated_IsCreated()
        {
            HoleAddedToMeasuredCourseEvent holeAddedToMeasuredCourseEvent = HoleAddedToMeasuredCourseEvent.Create(
                GolfClubTestData.AggregateId,
                GolfClubTestData.MeasuredCourseId, GolfClubTestData.HoleNumber,
                GolfClubTestData.LengthInYards,
                GolfClubTestData.LengthInMeters, GolfClubTestData.HolePar,
                GolfClubTestData.HoleStrokeIndex);

            holeAddedToMeasuredCourseEvent.ShouldNotBeNull();
            holeAddedToMeasuredCourseEvent.AggregateId.ShouldBe(GolfClubTestData.AggregateId);
            holeAddedToMeasuredCourseEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            holeAddedToMeasuredCourseEvent.EventId.ShouldNotBe(Guid.Empty);
            holeAddedToMeasuredCourseEvent.MeasuredCourseId.ShouldBe(GolfClubTestData.MeasuredCourseId);
            holeAddedToMeasuredCourseEvent.HoleNumber.ShouldBe(GolfClubTestData.HoleNumber);
            holeAddedToMeasuredCourseEvent.LengthInYards.ShouldBe(GolfClubTestData.LengthInYards);
            holeAddedToMeasuredCourseEvent.LengthInMeters.ShouldBe(GolfClubTestData.LengthInMeters);
            holeAddedToMeasuredCourseEvent.Par.ShouldBe(GolfClubTestData.HolePar);
            holeAddedToMeasuredCourseEvent.StrokeIndex.ShouldBe(GolfClubTestData.HoleStrokeIndex);            
        }

        [Fact]
        public void AdminSecurityUserCreatedEvent_CanBeCreated_IsCreated()
        {
            AdminSecurityUserCreatedEvent adminSecurityUserCreatedEvent = AdminSecurityUserCreatedEvent.Create(
                GolfClubTestData.AggregateId,
                GolfClubTestData.AdminSecurityUserId);

            adminSecurityUserCreatedEvent.ShouldNotBeNull();
            adminSecurityUserCreatedEvent.AggregateId.ShouldBe(GolfClubTestData.AggregateId);
            adminSecurityUserCreatedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            adminSecurityUserCreatedEvent.EventId.ShouldNotBe(Guid.Empty);
            adminSecurityUserCreatedEvent.AdminSecurityUserId.ShouldBe(GolfClubTestData.AdminSecurityUserId);
        }
    }
}
