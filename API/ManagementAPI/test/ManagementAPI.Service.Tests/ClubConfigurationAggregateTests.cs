using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests
{
    public class ClubConfigurationAggregateTests
    {
        [Fact]
        public void ClubConfigurationAggregate_CanBeCreated_IsCreated()
        {
            ClubConfigurationAggregate.ClubConfigurationAggregate aggregate = ClubConfigurationAggregate.ClubConfigurationAggregate.Create(ClubConfigurationTestData.AggregateId);

            aggregate.ShouldNotBeNull();
            aggregate.AggregateId.ShouldBe(ClubConfigurationTestData.AggregateId);
        }

        [Fact]
        public void ClubConfigurationAggregate_CreateClubConfiguration_ClubConfigurationCreated()
        {
            ClubConfigurationAggregate.ClubConfigurationAggregate aggregate = ClubConfigurationAggregate.ClubConfigurationAggregate.Create(ClubConfigurationTestData.AggregateId);

            aggregate.CreateClubConfiguration(ClubConfigurationTestData.Name, ClubConfigurationTestData.AddressLine1, ClubConfigurationTestData.AddressLine2,
                ClubConfigurationTestData.Town, ClubConfigurationTestData.Region, ClubConfigurationTestData.PostalCode, ClubConfigurationTestData.TelephoneNumber,
                ClubConfigurationTestData.Website, ClubConfigurationTestData.EmailAddress);

            aggregate.ShouldNotBeNull();
            aggregate.AggregateId.ShouldBe(ClubConfigurationTestData.AggregateId);
            aggregate.Name.ShouldBe(ClubConfigurationTestData.Name);
            aggregate.AddressLine1.ShouldBe(ClubConfigurationTestData.AddressLine1);
            aggregate.AddressLine2.ShouldBe(ClubConfigurationTestData.AddressLine2);
            aggregate.Town.ShouldBe(ClubConfigurationTestData.Town);
            aggregate.Region.ShouldBe(ClubConfigurationTestData.Region);
            aggregate.PostalCode.ShouldBe(ClubConfigurationTestData.PostalCode);
            aggregate.TelephoneNumber.ShouldBe(ClubConfigurationTestData.TelephoneNumber);
            aggregate.Website.ShouldBe(ClubConfigurationTestData.Website);
            aggregate.EmailAddress.ShouldBe(ClubConfigurationTestData.EmailAddress);
            aggregate.HasBeenCreated.ShouldBeTrue();
        }

        [Theory]
        [InlineData("", "addressline1", "town", "region", "postalcode")]
        [InlineData(null, "addressline1", "town", "region", "postalcode")]
        [InlineData("name", "", "town", "region", "postalcode")]
        [InlineData("name", null, "town", "region", "postalcode")]
        [InlineData("name", "addressline1", "", "region", "postalcode")]
        [InlineData("name", "addressline1", null, "region", "postalcode")]
        [InlineData("name", "addressline1", "town", "", "postalcode")]
        [InlineData("name", "addressline1", "town", null, "postalcode")]
        [InlineData("name", "addressline1", "town", "region", "")]
        [InlineData("name", "addressline1", "town", "region", null)]
        public void ClubConfigurationAggregate_CreateClubConfiguration_InvalidData_ErrorThrown(String name, String addressLine1, String town,String region, String postalCode)
        {
            ClubConfigurationAggregate.ClubConfigurationAggregate aggregate = ClubConfigurationAggregate.ClubConfigurationAggregate.Create(ClubConfigurationTestData.AggregateId);

            Should.Throw<ArgumentNullException>(() =>
            {
                aggregate.CreateClubConfiguration(name, addressLine1, ClubConfigurationTestData.AddressLine2,
                    town, region, postalCode, ClubConfigurationTestData.TelephoneNumber,
                    ClubConfigurationTestData.Website, ClubConfigurationTestData.EmailAddress);
            });
        }

        [Fact]
        public void ClubConfigurationAggregate_CreateClubConfiguration_DuplicateCreateClubConfigurationCalled_ErrorThrown()
        {
            ClubConfigurationAggregate.ClubConfigurationAggregate aggregate = ClubConfigurationAggregate.ClubConfigurationAggregate.Create(ClubConfigurationTestData.AggregateId);

            Should.NotThrow(() =>
            {
                aggregate.CreateClubConfiguration(ClubConfigurationTestData.Name,
                    ClubConfigurationTestData.AddressLine1, ClubConfigurationTestData.AddressLine2,
                    ClubConfigurationTestData.Town, ClubConfigurationTestData.Region,
                    ClubConfigurationTestData.PostalCode, ClubConfigurationTestData.TelephoneNumber,
                    ClubConfigurationTestData.Website, ClubConfigurationTestData.EmailAddress);
            });

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CreateClubConfiguration(ClubConfigurationTestData.Name,
                    ClubConfigurationTestData.AddressLine1, ClubConfigurationTestData.AddressLine2,
                    ClubConfigurationTestData.Town, ClubConfigurationTestData.Region,
                    ClubConfigurationTestData.PostalCode, ClubConfigurationTestData.TelephoneNumber,
                    ClubConfigurationTestData.Website, ClubConfigurationTestData.EmailAddress);
            });
        }
    }


}
