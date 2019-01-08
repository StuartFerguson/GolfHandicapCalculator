using System;
using System.IO;
using System.Linq;
using ManagementAPI.ClubConfiguration;
using ManagementAPI.GolfClub;
using Shared.EventStore;
using Shared.Exceptions;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.ClubConfiguration
{
    public class ClubConfigurationAggregateTests
    {
        #region Create Tests
        
        [Fact]
        public void ClubConfigurationAggregate_CanBeCreated_IsCreated()
        {
            GolfClubAggregate aggregate = GolfClubAggregate.Create(ClubConfigurationTestData.AggregateId);

            aggregate.ShouldNotBeNull();
            aggregate.AggregateId.ShouldBe(ClubConfigurationTestData.AggregateId);
        }

        [Fact]
        public void ClubConfigurationAggregate_CanBeCreated_EmptyAggregateid_ErrorThrown()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                GolfClubAggregate aggregate =
                    GolfClubAggregate.Create(Guid.Empty);
            });
        }

        #endregion

        #region Create Club Configuration Tests

        [Fact]
        public void ClubConfigurationAggregate_CreateClubConfiguration_ClubConfigurationCreated()
        {
            GolfClubAggregate aggregate = GolfClubAggregate.Create(ClubConfigurationTestData.AggregateId);

            aggregate.CreateGolfClub(ClubConfigurationTestData.Name, ClubConfigurationTestData.AddressLine1, ClubConfigurationTestData.AddressLine2,
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
            GolfClubAggregate aggregate = GolfClubAggregate.Create(ClubConfigurationTestData.AggregateId);

            Should.Throw<ArgumentNullException>(() =>
            {
                aggregate.CreateGolfClub(name, addressLine1, ClubConfigurationTestData.AddressLine2,
                    town, region, postalCode, ClubConfigurationTestData.TelephoneNumber,
                    ClubConfigurationTestData.Website, ClubConfigurationTestData.EmailAddress);
            });
        }

        [Fact]
        public void ClubConfigurationAggregate_CreateClubConfiguration_DuplicateCreateClubConfigurationCalled_ErrorThrown()
        {
            GolfClubAggregate aggregate = GolfClubAggregate.Create(ClubConfigurationTestData.AggregateId);

            Should.NotThrow(() =>
            {
                aggregate.CreateGolfClub(ClubConfigurationTestData.Name,
                    ClubConfigurationTestData.AddressLine1, ClubConfigurationTestData.AddressLine2,
                    ClubConfigurationTestData.Town, ClubConfigurationTestData.Region,
                    ClubConfigurationTestData.PostalCode, ClubConfigurationTestData.TelephoneNumber,
                    ClubConfigurationTestData.Website, ClubConfigurationTestData.EmailAddress);
            });

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CreateGolfClub(ClubConfigurationTestData.Name,
                    ClubConfigurationTestData.AddressLine1, ClubConfigurationTestData.AddressLine2,
                    ClubConfigurationTestData.Town, ClubConfigurationTestData.Region,
                    ClubConfigurationTestData.PostalCode, ClubConfigurationTestData.TelephoneNumber,
                    ClubConfigurationTestData.Website, ClubConfigurationTestData.EmailAddress);
            });
        }

        #endregion

        #region Add Measured Course Tests

        [Fact]
        public void ClubConfigurationAggregate_AddMeasuredCourse_MeasuredCourseWithHolesAdded()
        {
            GolfClubAggregate aggregate = ClubConfigurationTestData.GetCreatedClubConfigurationAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = ClubConfigurationTestData.GetMeasuredCourseToAdd();

            aggregate.AddMeasuredCourse(measuredCourseDataTransferObject);

            var measuredCourse = aggregate.GetMeasuredCourse(measuredCourseDataTransferObject.MeasuredCourseId);

            measuredCourse.ShouldNotBeNull();
            measuredCourse.Name.ShouldBe(measuredCourseDataTransferObject.Name);
            measuredCourse.StandardScratchScore.ShouldBe(measuredCourseDataTransferObject.StandardScratchScore);
            measuredCourse.TeeColour.ShouldBe(measuredCourseDataTransferObject.TeeColour);
            measuredCourse.Holes.Count.ShouldBe(measuredCourseDataTransferObject.Holes.Count);
            
            var resultHoles = measuredCourse.Holes.OrderBy(m => m.HoleNumber);

            foreach (var holeDataTransferObject in resultHoles)
            {
                var orignalHole = measuredCourseDataTransferObject.Holes.Single(h => h.HoleNumber == holeDataTransferObject.HoleNumber);

                holeDataTransferObject.HoleNumber.ShouldBe(orignalHole.HoleNumber);
                holeDataTransferObject.LengthInMeters.ShouldBe(orignalHole.LengthInMeters);
                holeDataTransferObject.LengthInYards.ShouldBe(orignalHole.LengthInYards);
                holeDataTransferObject.Par.ShouldBe(orignalHole.Par);
                holeDataTransferObject.StrokeIndex.ShouldBe(orignalHole.StrokeIndex);
            }
        }

        [Fact]
        public void ClubConfigurationAggregate_AddMeasuredCourse_ClubNotCreated_ErrorThrown()
        {
            GolfClubAggregate aggregate = ClubConfigurationTestData.GetEmptyClubConfigurationAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = ClubConfigurationTestData.GetMeasuredCourseToAdd();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.AddMeasuredCourse(measuredCourseDataTransferObject);
            });

        }

        [Fact]
        public void ClubConfigurationAggregate_AddMeasuredCourse_DuplicateMeasuredCourse_ErrorThrown()
        {
            GolfClubAggregate aggregate = ClubConfigurationTestData.GetCreatedClubConfigurationAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = ClubConfigurationTestData.GetMeasuredCourseToAdd();

            aggregate.AddMeasuredCourse(measuredCourseDataTransferObject);

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.AddMeasuredCourse(measuredCourseDataTransferObject);
            });
        }

        [Theory]
        [InlineData("", "teeColour", 70)]
        [InlineData(null, "teeColour", 70)]
        [InlineData("name", "", 70)]
        [InlineData("name", null, 70)]
        [InlineData("name", "teeColour", 0)]
        [InlineData("name", "teeColour", -70)]
        public void ClubConfigurationAggregate_AddMeasuredCourse_InvalidCourseData_ErrorThrown(String name, String teeColour, Int32 standardScratchScore)
        {
            GolfClubAggregate aggregate = ClubConfigurationTestData.GetCreatedClubConfigurationAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = ClubConfigurationTestData.GetMeasuredCourseToAdd();
            measuredCourseDataTransferObject.Name = name;
            measuredCourseDataTransferObject.TeeColour = teeColour;
            measuredCourseDataTransferObject.StandardScratchScore = standardScratchScore;

            Should.Throw<ArgumentNullException>(() =>
            {
                aggregate.AddMeasuredCourse(measuredCourseDataTransferObject);
            });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        [InlineData(14)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(17)]
        [InlineData(19)]
        public void ClubConfigurationAggregate_AddMeasuredCourse_InvalidNumberOfHoles_ErrorThrown(Int32 numberHoles)
        {
            GolfClubAggregate aggregate = ClubConfigurationTestData.GetCreatedClubConfigurationAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = ClubConfigurationTestData.GetMeasuredCourseToAdd(numberHoles);            

            Should.Throw<InvalidDataException>(() =>
            {
                aggregate.AddMeasuredCourse(measuredCourseDataTransferObject);
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        [InlineData(14)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(17)]
        [InlineData(18)]
        public void ClubConfigurationAggregate_AddMeasuredCourse_InvalidHoleData_MissingHoleNumber_ErrorThrown(Int32 holeNumber)
        {
            GolfClubAggregate aggregate = ClubConfigurationTestData.GetCreatedClubConfigurationAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = ClubConfigurationTestData.GetMeasuredCourseToAddWithMissingHoles(holeNumber);            

            Should.Throw<InvalidDataException>(() =>
            {
                aggregate.AddMeasuredCourse(measuredCourseDataTransferObject);
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        [InlineData(14)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(17)]
        [InlineData(18)]
        public void ClubConfigurationAggregate_AddMeasuredCourse_InvalidHoleData_MissingStrokeIndex_ErrorThrown(Int32 strokeIndex)
        {
            GolfClubAggregate aggregate = ClubConfigurationTestData.GetCreatedClubConfigurationAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = ClubConfigurationTestData.GetMeasuredCourseToAddWithMissingStrokeIndex(strokeIndex);            

            Should.Throw<InvalidDataException>(() =>
            {
                aggregate.AddMeasuredCourse(measuredCourseDataTransferObject);
            });
        }

        [Theory]
        [InlineData(0, 3)]
        [InlineData(-1, 3)]
        [InlineData(400, 0)]
        [InlineData(400, -1)]
        public void ClubConfigurationAggregate_AddMeasuredCourse_InvalidHoleData_InvalidData_ErrorThrown(Int32 lengthInYards, Int32 par)
        {
            GolfClubAggregate aggregate = ClubConfigurationTestData.GetCreatedClubConfigurationAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = ClubConfigurationTestData.GetMeasuredCourseToAdd();
            measuredCourseDataTransferObject.Holes.First().LengthInYards = lengthInYards;
            measuredCourseDataTransferObject.Holes.First().Par = par;
            Should.Throw<InvalidDataException>(() =>
            {
                aggregate.AddMeasuredCourse(measuredCourseDataTransferObject);
            });
        }

        #endregion

        #region Get Measured Course Tests

        [Fact]
        public void ClubConfigurationAggregate_GetMeasuredCourse_MeasuredCourseWithHolesReturned()
        {
            GolfClubAggregate aggregate = ClubConfigurationTestData.GetClubConfigurationAggregateWithMeasuredCourse();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = ClubConfigurationTestData.GetMeasuredCourseToAdd();

            var measuredCourse = aggregate.GetMeasuredCourse(measuredCourseDataTransferObject.MeasuredCourseId);

            measuredCourse.ShouldNotBeNull();
            measuredCourse.Name.ShouldBe(measuredCourseDataTransferObject.Name);
            measuredCourse.StandardScratchScore.ShouldBe(measuredCourseDataTransferObject.StandardScratchScore);
            measuredCourse.TeeColour.ShouldBe(measuredCourseDataTransferObject.TeeColour);
            measuredCourse.Holes.Count.ShouldBe(measuredCourseDataTransferObject.Holes.Count);
            
            var resultHoles = measuredCourse.Holes.OrderBy(m => m.HoleNumber);

            foreach (var holeDataTransferObject in resultHoles)
            {
                var orignalHole = measuredCourseDataTransferObject.Holes.Single(h => h.HoleNumber == holeDataTransferObject.HoleNumber);

                holeDataTransferObject.HoleNumber.ShouldBe(orignalHole.HoleNumber);
                holeDataTransferObject.LengthInMeters.ShouldBe(orignalHole.LengthInMeters);
                holeDataTransferObject.LengthInYards.ShouldBe(orignalHole.LengthInYards);
                holeDataTransferObject.Par.ShouldBe(orignalHole.Par);
                holeDataTransferObject.StrokeIndex.ShouldBe(orignalHole.StrokeIndex);
            }
        }

        [Fact]
        public void ClubConfigurationAggregate_GetMeasuredCourse_MeasuredCourseNotFound_ErrorThrown()
        {
            GolfClubAggregate aggregate = ClubConfigurationTestData.GetClubConfigurationAggregateWithMeasuredCourse();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = ClubConfigurationTestData.GetMeasuredCourseToAdd();

            Should.Throw<NotFoundException>(() =>
            {
                aggregate.GetMeasuredCourse(ClubConfigurationTestData.InvalidMeasuredCourseId);
            });
        }

        #endregion

        #region Create Admin Security User Tests

        [Fact]
        public void ClubConfigurationAggregate_CreateAdminSecurityUser_AdminSecurityUserCreated()
        {
            GolfClubAggregate aggregate = ClubConfigurationTestData.GetCreatedClubConfigurationAggregate();

            aggregate.CreateAdminSecurityUser(ClubConfigurationTestData.AdminSecurityUserId);

            aggregate.AdminSecurityUserId.ShouldBe(ClubConfigurationTestData.AdminSecurityUserId);
            aggregate.HasAdminSecurityUserBeenCreated.ShouldBeTrue();
        }

        [Fact]
        public void ClubConfigurationAggregate_CreateAdminSecurityUser_InvalidData_ErrorThrown()
        {
            GolfClubAggregate aggregate = ClubConfigurationTestData.GetCreatedClubConfigurationAggregate();

            Should.Throw<ArgumentNullException>(() =>
            {
                aggregate.CreateAdminSecurityUser(Guid.Empty);
            });
        }

        [Fact]
        public void ClubConfigurationAggregate_CreateAdminSecurityUser_ClubNotCreated_ErrorThrown()
        {
            GolfClubAggregate aggregate = ClubConfigurationTestData.GetEmptyClubConfigurationAggregate();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CreateAdminSecurityUser(ClubConfigurationTestData.AdminSecurityUserId);
            });
        }

        [Fact]
        public void ClubConfigurationAggregate_CreateAdminSecurityUser_AdminSecurityUserAlreadyCreated_ErrorThrown()
        {
            GolfClubAggregate aggregate = ClubConfigurationTestData.GetCreatedClubConfigurationAggregateWithAdminUser();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CreateAdminSecurityUser(ClubConfigurationTestData.AdminSecurityUserId);
            });
        }

        #endregion
    }
}
