using System;
using System.Linq;
using ManagementAPI.ClubConfiguration.DomainEvents;
using Shouldly;
using Xunit;
using Xunit.Sdk;

namespace ManagementAPI.Service.Tests
{
    public class ClubConfigurationAggregateDomainEventTests
    {
        [Fact]
        public void ClubConfigurationCreatedEvent_CanBeCreated_IsCreated()
        {
            ClubConfigurationCreatedEvent clubConfigurationCreatedEvent = ClubConfigurationCreatedEvent.Create(ClubConfigurationTestData.AggregateId,
                ClubConfigurationTestData.Name, 
                ClubConfigurationTestData.AddressLine1,
                ClubConfigurationTestData.AddressLine2,
                ClubConfigurationTestData.Town,
                ClubConfigurationTestData.Region,
                ClubConfigurationTestData.PostalCode,
                ClubConfigurationTestData.TelephoneNumber,
                ClubConfigurationTestData.Website,
                ClubConfigurationTestData.EmailAddress);

            clubConfigurationCreatedEvent.ShouldNotBeNull();
            clubConfigurationCreatedEvent.AggregateId.ShouldBe(ClubConfigurationTestData.AggregateId);
            clubConfigurationCreatedEvent.Name.ShouldBe(ClubConfigurationTestData.Name);
            clubConfigurationCreatedEvent.AddressLine1.ShouldBe(ClubConfigurationTestData.AddressLine1);
            clubConfigurationCreatedEvent.AddressLine2.ShouldBe(ClubConfigurationTestData.AddressLine2);
            clubConfigurationCreatedEvent.Town.ShouldBe(ClubConfigurationTestData.Town);
            clubConfigurationCreatedEvent.Region.ShouldBe(ClubConfigurationTestData.Region);
            clubConfigurationCreatedEvent.PostalCode.ShouldBe(ClubConfigurationTestData.PostalCode);
            clubConfigurationCreatedEvent.TelephoneNumber.ShouldBe(ClubConfigurationTestData.TelephoneNumber);
            clubConfigurationCreatedEvent.Website.ShouldBe(ClubConfigurationTestData.Website);
            clubConfigurationCreatedEvent.EmailAddress.ShouldBe(ClubConfigurationTestData.EmailAddress);
            clubConfigurationCreatedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            clubConfigurationCreatedEvent.EventId.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public void MeasuredCourseAddedEvent_CanBeCreated_IsCreated()
        {
            MeasuredCourseAddedEvent measuredCourseAddedEvent = MeasuredCourseAddedEvent.Create(ClubConfigurationTestData.AggregateId, 
                ClubConfigurationTestData.MeasuredCourseId, ClubConfigurationTestData.MeasuredCourseName, ClubConfigurationTestData.TeeColour,
                ClubConfigurationTestData.StandardScratchScore);

            measuredCourseAddedEvent.ShouldNotBeNull();
            measuredCourseAddedEvent.AggregateId.ShouldBe(ClubConfigurationTestData.AggregateId);
            measuredCourseAddedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            measuredCourseAddedEvent.EventId.ShouldNotBe(Guid.Empty);
            measuredCourseAddedEvent.MeasuredCourseId.ShouldBe(ClubConfigurationTestData.MeasuredCourseId);
            measuredCourseAddedEvent.Name.ShouldBe(ClubConfigurationTestData.MeasuredCourseName);
            measuredCourseAddedEvent.TeeColour.ShouldBe(ClubConfigurationTestData.TeeColour);
            measuredCourseAddedEvent.StandardScratchScore.ShouldBe(ClubConfigurationTestData.StandardScratchScore);            
        }

        [Fact]
        public void HoleAddedToMeasuredCourseEvent_CanBeCreated_IsCreated()
        {
            HoleAddedToMeasuredCourseEvent holeAddedToMeasuredCourseEvent = HoleAddedToMeasuredCourseEvent.Create(
                ClubConfigurationTestData.AggregateId,
                ClubConfigurationTestData.MeasuredCourseId, ClubConfigurationTestData.HoleNumber,
                ClubConfigurationTestData.LengthInYards,
                ClubConfigurationTestData.LengthInMeters, ClubConfigurationTestData.HolePar,
                ClubConfigurationTestData.HoleStrokeIndex);

            holeAddedToMeasuredCourseEvent.ShouldNotBeNull();
            holeAddedToMeasuredCourseEvent.AggregateId.ShouldBe(ClubConfigurationTestData.AggregateId);
            holeAddedToMeasuredCourseEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            holeAddedToMeasuredCourseEvent.EventId.ShouldNotBe(Guid.Empty);
            holeAddedToMeasuredCourseEvent.MeasuredCourseId.ShouldBe(ClubConfigurationTestData.MeasuredCourseId);
            holeAddedToMeasuredCourseEvent.HoleNumber.ShouldBe(ClubConfigurationTestData.HoleNumber);
            holeAddedToMeasuredCourseEvent.LengthInYards.ShouldBe(ClubConfigurationTestData.LengthInYards);
            holeAddedToMeasuredCourseEvent.LengthInMeters.ShouldBe(ClubConfigurationTestData.LengthInMeters);
            holeAddedToMeasuredCourseEvent.Par.ShouldBe(ClubConfigurationTestData.HolePar);
            holeAddedToMeasuredCourseEvent.StrokeIndex.ShouldBe(ClubConfigurationTestData.HoleStrokeIndex);            
        }

        [Fact]
        public void AdminSecurityUserCreatedEvent_CanBeCreated_IsCreated()
        {
            AdminSecurityUserCreatedEvent adminSecurityUserCreatedEvent = AdminSecurityUserCreatedEvent.Create(
                ClubConfigurationTestData.AggregateId,
                ClubConfigurationTestData.AdminSecurityUserId);

            adminSecurityUserCreatedEvent.ShouldNotBeNull();
            adminSecurityUserCreatedEvent.AggregateId.ShouldBe(ClubConfigurationTestData.AggregateId);
            adminSecurityUserCreatedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            adminSecurityUserCreatedEvent.EventId.ShouldNotBe(Guid.Empty);
            adminSecurityUserCreatedEvent.AdminSecurityUserId.ShouldBe(ClubConfigurationTestData.AdminSecurityUserId);
        }
    }
}
