using System;
using System.Collections.Generic;
using System.Text;
using ManagementAPI.IntegrationTests.DataTransferObjects;
using ManagementAPI.Service.DataTransferObjects;

namespace ManagementAPI.IntegrationTests
{
    public class IntegrationTestsTestData
    {
        public static CreateClubConfigurationRequest CreateClubConfigurationRequest = new CreateClubConfigurationRequest
        {
            Name = "Name",
            AddressLine1 = "Address Line 1",
            EmailAddress = "1@2.com",
            PostalCode = "TE57 1NG",
            Town = "Test Town",
            Website = "www.website.com",
            Region = "Test Region",
            TelephoneNumber = "123456789",
            AddressLine2 = "Address Line 2"
        };

        public static AddMeasuredCourseToClubRequest AddMeasuredCourseToClubRequest =
            new AddMeasuredCourseToClubRequest()
            {
                MeasuredCourseId = Guid.Parse("47708163-7E3A-4C61-B1C3-64C2CDFC1170"),
                Name = "Test Course",
                StandardScratchScore = 70,
                TeeColour = "White",
                Holes = new List<DataTransferObjects.HoleDataTransferObject>
                {
                    new HoleDataTransferObject {HoleNumber = 1, LengthInYards = 348, Par = 4, StrokeIndex = 10},
                    new HoleDataTransferObject {HoleNumber = 2, LengthInYards = 402, Par = 4, StrokeIndex = 4},
                    new HoleDataTransferObject {HoleNumber = 3, LengthInYards = 207, Par = 3, StrokeIndex = 14},
                    new HoleDataTransferObject {HoleNumber = 4, LengthInYards = 405, Par = 4, StrokeIndex = 8},
                    new HoleDataTransferObject {HoleNumber = 5, LengthInYards = 428, Par = 4, StrokeIndex = 2},
                    new HoleDataTransferObject {HoleNumber = 6, LengthInYards = 477, Par = 5, StrokeIndex = 12},
                    new HoleDataTransferObject {HoleNumber = 7, LengthInYards = 186, Par = 4, StrokeIndex = 16},
                    new HoleDataTransferObject {HoleNumber = 8, LengthInYards = 397, Par = 4, StrokeIndex = 6},
                    new HoleDataTransferObject {HoleNumber = 9, LengthInYards = 130, Par = 3, StrokeIndex = 18},
                    new HoleDataTransferObject {HoleNumber = 10, LengthInYards = 399, Par = 4, StrokeIndex = 3},
                    new HoleDataTransferObject {HoleNumber = 11, LengthInYards = 401, Par = 4, StrokeIndex = 13},
                    new HoleDataTransferObject {HoleNumber = 12, LengthInYards = 421, Par = 4, StrokeIndex = 1},
                    new HoleDataTransferObject {HoleNumber = 13, LengthInYards = 530, Par = 5, StrokeIndex = 11},
                    new HoleDataTransferObject {HoleNumber = 14, LengthInYards = 196, Par = 3, StrokeIndex = 5},
                    new HoleDataTransferObject {HoleNumber = 15, LengthInYards = 355, Par = 4, StrokeIndex = 7},
                    new HoleDataTransferObject {HoleNumber = 16, LengthInYards = 243, Par = 4, StrokeIndex = 15},
                    new HoleDataTransferObject {HoleNumber = 17, LengthInYards = 286, Par = 4, StrokeIndex = 17},
                    new HoleDataTransferObject {HoleNumber = 18, LengthInYards = 399, Par = 4, StrokeIndex = 9}
                }

            };

        public static CreateTournamentRequest CreateTournamentRequest = new CreateTournamentRequest
        {
            Name = "Test Competition",
            MemberCategory = 1,
            TournamentDate = DateTime.Today,
            Format = 1
        };

        public static RecordMemberTournamentScoreRequest RecordMemberTournamentScoreRequest =
            new RecordMemberTournamentScoreRequest
            {
                MemberId = Guid.Parse("9F14D8A4-D8F7-4E32-9600-C3F038E662F6"),
                HoleScores = new Dictionary<Int32, Int32>()
                {
                    {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
                    {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
                }
            };

        public static CancelTournamentRequest CancelTournamentRequest = new CancelTournamentRequest
        {
            CancellationReason = "Test Reason"
        };
    }
}
