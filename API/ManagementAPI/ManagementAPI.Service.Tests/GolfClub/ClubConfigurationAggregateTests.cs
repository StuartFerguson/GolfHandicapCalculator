using System;
using System.IO;
using System.Linq;
using ManagementAPI.GolfClub;
using Shared.Exceptions;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.GolfClub
{
    public class GolfClubAggregateTests
    {
        #region Create Tests
        
        [Fact]
        public void GolfClubAggregate_CanBeCreated_IsCreated()
        {
            GolfClubAggregate aggregate = GolfClubAggregate.Create(GolfClubTestData.AggregateId);

            aggregate.ShouldNotBeNull();
            aggregate.AggregateId.ShouldBe(GolfClubTestData.AggregateId);
        }

        [Fact]
        public void GolfClubAggregate_CanBeCreated_EmptyAggregateid_ErrorThrown()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                GolfClubAggregate aggregate =
                    GolfClubAggregate.Create(Guid.Empty);
            });
        }

        #endregion

        #region Create Golf Club Tests

        [Fact]
        public void GolfClubAggregate_CreateGolfClub_GolfClubCreated()
        {
            GolfClubAggregate aggregate = GolfClubAggregate.Create(GolfClubTestData.AggregateId);

            aggregate.CreateGolfClub(GolfClubTestData.Name, GolfClubTestData.AddressLine1, GolfClubTestData.AddressLine2,
                GolfClubTestData.Town, GolfClubTestData.Region, GolfClubTestData.PostalCode, GolfClubTestData.TelephoneNumber,
                GolfClubTestData.Website, GolfClubTestData.EmailAddress);

            aggregate.ShouldNotBeNull();
            aggregate.AggregateId.ShouldBe(GolfClubTestData.AggregateId);
            aggregate.Name.ShouldBe(GolfClubTestData.Name);
            aggregate.AddressLine1.ShouldBe(GolfClubTestData.AddressLine1);
            aggregate.AddressLine2.ShouldBe(GolfClubTestData.AddressLine2);
            aggregate.Town.ShouldBe(GolfClubTestData.Town);
            aggregate.Region.ShouldBe(GolfClubTestData.Region);
            aggregate.PostalCode.ShouldBe(GolfClubTestData.PostalCode);
            aggregate.TelephoneNumber.ShouldBe(GolfClubTestData.TelephoneNumber);
            aggregate.Website.ShouldBe(GolfClubTestData.Website);
            aggregate.EmailAddress.ShouldBe(GolfClubTestData.EmailAddress);
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
        public void GolfClubAggregate_CreateGolfClub_InvalidData_ErrorThrown(String name, String addressLine1, String town,String region, String postalCode)
        {
            GolfClubAggregate aggregate = GolfClubAggregate.Create(GolfClubTestData.AggregateId);

            Should.Throw<ArgumentNullException>(() =>
            {
                aggregate.CreateGolfClub(name, addressLine1, GolfClubTestData.AddressLine2,
                    town, region, postalCode, GolfClubTestData.TelephoneNumber,
                    GolfClubTestData.Website, GolfClubTestData.EmailAddress);
            });
        }

        [Fact]
        public void GolfClubAggregate_CreateGolfClub_DuplicateCreateGolfClubCalled_ErrorThrown()
        {
            GolfClubAggregate aggregate = GolfClubAggregate.Create(GolfClubTestData.AggregateId);

            Should.NotThrow(() =>
            {
                aggregate.CreateGolfClub(GolfClubTestData.Name,
                    GolfClubTestData.AddressLine1, GolfClubTestData.AddressLine2,
                    GolfClubTestData.Town, GolfClubTestData.Region,
                    GolfClubTestData.PostalCode, GolfClubTestData.TelephoneNumber,
                    GolfClubTestData.Website, GolfClubTestData.EmailAddress);
            });

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CreateGolfClub(GolfClubTestData.Name,
                    GolfClubTestData.AddressLine1, GolfClubTestData.AddressLine2,
                    GolfClubTestData.Town, GolfClubTestData.Region,
                    GolfClubTestData.PostalCode, GolfClubTestData.TelephoneNumber,
                    GolfClubTestData.Website, GolfClubTestData.EmailAddress);
            });
        }

        #endregion

        #region Add Measured Course Tests

        [Fact]
        public void GolfClubAggregate_AddMeasuredCourse_MeasuredCourseWithHolesAdded()
        {
            GolfClubAggregate aggregate = GolfClubTestData.GetCreatedGolfClubAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = GolfClubTestData.GetMeasuredCourseToAdd();

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
        public void GolfClubAggregate_AddMeasuredCourse_ClubNotCreated_ErrorThrown()
        {
            GolfClubAggregate aggregate = GolfClubTestData.GetEmptyGolfClubAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = GolfClubTestData.GetMeasuredCourseToAdd();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.AddMeasuredCourse(measuredCourseDataTransferObject);
            });

        }

        [Fact]
        public void GolfClubAggregate_AddMeasuredCourse_DuplicateMeasuredCourse_ErrorThrown()
        {
            GolfClubAggregate aggregate = GolfClubTestData.GetCreatedGolfClubAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = GolfClubTestData.GetMeasuredCourseToAdd();

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
        public void GolfClubAggregate_AddMeasuredCourse_InvalidCourseData_ErrorThrown(String name, String teeColour, Int32 standardScratchScore)
        {
            GolfClubAggregate aggregate = GolfClubTestData.GetCreatedGolfClubAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = GolfClubTestData.GetMeasuredCourseToAdd();
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
        public void GolfClubAggregate_AddMeasuredCourse_InvalidNumberOfHoles_ErrorThrown(Int32 numberHoles)
        {
            GolfClubAggregate aggregate = GolfClubTestData.GetCreatedGolfClubAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = GolfClubTestData.GetMeasuredCourseToAdd(numberHoles);            

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
        public void GolfClubAggregate_AddMeasuredCourse_InvalidHoleData_MissingHoleNumber_ErrorThrown(Int32 holeNumber)
        {
            GolfClubAggregate aggregate = GolfClubTestData.GetCreatedGolfClubAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = GolfClubTestData.GetMeasuredCourseToAddWithMissingHoles(holeNumber);            

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
        public void GolfClubAggregate_AddMeasuredCourse_InvalidHoleData_MissingStrokeIndex_ErrorThrown(Int32 strokeIndex)
        {
            GolfClubAggregate aggregate = GolfClubTestData.GetCreatedGolfClubAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = GolfClubTestData.GetMeasuredCourseToAddWithMissingStrokeIndex(strokeIndex);            

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
        public void GolfClubAggregate_AddMeasuredCourse_InvalidHoleData_InvalidData_ErrorThrown(Int32 lengthInYards, Int32 par)
        {
            GolfClubAggregate aggregate = GolfClubTestData.GetCreatedGolfClubAggregate();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = GolfClubTestData.GetMeasuredCourseToAdd();
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
        public void GolfClubAggregate_GetMeasuredCourse_MeasuredCourseWithHolesReturned()
        {
            GolfClubAggregate aggregate = GolfClubTestData.GetGolfClubAggregateWithMeasuredCourse();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = GolfClubTestData.GetMeasuredCourseToAdd();

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
        public void GolfClubAggregate_GetMeasuredCourse_MeasuredCourseNotFound_ErrorThrown()
        {
            GolfClubAggregate aggregate = GolfClubTestData.GetGolfClubAggregateWithMeasuredCourse();

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = GolfClubTestData.GetMeasuredCourseToAdd();

            Should.Throw<NotFoundException>(() =>
            {
                aggregate.GetMeasuredCourse(GolfClubTestData.InvalidMeasuredCourseId);
            });
        }

        #endregion

        #region Create Admin Security User Tests

        [Fact]
        public void GolfClubAggregate_CreateAdminSecurityUser_AdminSecurityUserCreated()
        {
            GolfClubAggregate aggregate = GolfClubTestData.GetCreatedGolfClubAggregate();

            aggregate.CreateAdminSecurityUser(GolfClubTestData.AdminSecurityUserId);

            aggregate.AdminSecurityUserId.ShouldBe(GolfClubTestData.AdminSecurityUserId);
            aggregate.HasAdminSecurityUserBeenCreated.ShouldBeTrue();
        }

        [Fact]
        public void GolfClubAggregate_CreateAdminSecurityUser_InvalidData_ErrorThrown()
        {
            GolfClubAggregate aggregate = GolfClubTestData.GetCreatedGolfClubAggregate();

            Should.Throw<ArgumentNullException>(() =>
            {
                aggregate.CreateAdminSecurityUser(Guid.Empty);
            });
        }

        [Fact]
        public void GolfClubAggregate_CreateAdminSecurityUser_ClubNotCreated_ErrorThrown()
        {
            GolfClubAggregate aggregate = GolfClubTestData.GetEmptyGolfClubAggregate();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CreateAdminSecurityUser(GolfClubTestData.AdminSecurityUserId);
            });
        }

        [Fact]
        public void GolfClubAggregate_CreateAdminSecurityUser_AdminSecurityUserAlreadyCreated_ErrorThrown()
        {
            GolfClubAggregate aggregate = GolfClubTestData.GetCreatedGolfClubAggregateWithAdminUser();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CreateAdminSecurityUser(GolfClubTestData.AdminSecurityUserId);
            });
        }

        #endregion
    }
}
