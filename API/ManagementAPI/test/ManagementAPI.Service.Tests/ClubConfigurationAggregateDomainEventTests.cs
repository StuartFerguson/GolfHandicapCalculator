using System;
using System.Linq;
using ManagementAPI.ClubConfiguration.DomainEvents;
using Shouldly;
using Xunit;

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
    }
}
