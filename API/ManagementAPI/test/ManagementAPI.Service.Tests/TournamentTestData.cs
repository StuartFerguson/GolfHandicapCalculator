using System;
using System.Collections.Generic;
using System.Text;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.DataTransferObjects;
using ManagementAPI.TournamentAggregate;

namespace ManagementAPI.Service.Tests
{
    public class TournamentTestData
    {
        public static Guid AggregateId = Guid.Parse("15650BE2-4F7F-40D9-B5F8-A099A713E959");
        public static DateTime TournamentDate = new DateTime(2018,4,1);
        public static Guid ClubConfigurationId = Guid.Parse("CD64A469-9593-49D6-988D-3842C532D23E");
        public static Guid MeasuredCourseId= Guid.Parse("B2F334C2-03D3-48DB-9C6F-45FB1133F071");
        public static String Name = "Test Tournament";
        public static Int32 MemberCategory = 2;
        public static MemberCategory MemberCategoryEnum = TournamentAggregate.MemberCategory.Gents;
        public static Int32 TournamentFormat = 1;
        public static TournamentFormat TournamentFormatEnum = TournamentAggregate.TournamentFormat.Strokeplay;
        public static Guid MemberId = Guid.Parse("9F14D8A4-D8F7-4E32-9600-C3F038E662F6");

        public static Dictionary<Int32, Int32> HoleScores = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
        };

        public static Dictionary<Int32, Int32> HoleScoresNotAllPresent = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}
        };

        public static Dictionary<Int32, Int32> HoleScoresMissingHoles = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {19, 4}
        };

        public static Dictionary<Int32, Int32> HoleScoresExtraScores = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18,4},
            {19,4}
        };

        public static Dictionary<Int32, Int32> HoleScoresMissingHole(Int32 holeNumber)
        {
            var holeScores = new Dictionary<Int32, Int32>()
            {
                {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
                {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
            };

            holeScores.Remove(holeNumber);

            return holeScores;
        }

        public static Dictionary<Int32, Int32> HoleScoresNegativeScore(Int32 holeNumber)
        {
            var holeScores = new Dictionary<Int32, Int32>()
            {
                {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
                {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
            };

            holeScores[holeNumber] = -1;

            return holeScores;
        }



        public static TournamentAggregate.TournamentAggregate GetEmptyTournamentAggregate()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentAggregate.TournamentAggregate.Create(AggregateId);

            return aggregate;
        }

        public static TournamentAggregate.TournamentAggregate GetCreatedTournamentAggregate()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentAggregate.TournamentAggregate.Create(AggregateId);

            aggregate.CreateTournament(TournamentDate, ClubConfigurationId, MeasuredCourseId, Name, MemberCategoryEnum, TournamentFormatEnum);

            return aggregate;
        }

        public static CreateTournamentRequest CreateTournamentRequest = new CreateTournamentRequest
        {
            Name = Name,
            MemberCategory = MemberCategory,
            MeasuredCourseId = MeasuredCourseId,
            ClubConfigurationId = ClubConfigurationId,
            TournamentDate = TournamentDate,
            Format = TournamentFormat
        };

        public static CreateTournamentCommand GetCreateTournamentCommand()
        {
            return CreateTournamentCommand.Create(CreateTournamentRequest);
        }

        public static RecordMemberTournamentScoreRequest RecordMemberTournamentScoreRequest = new RecordMemberTournamentScoreRequest
        {
            MemberId = MemberId,
            HoleScores = HoleScores,
            TournamentId = AggregateId
        };

        public static RecordMemberTournamentScoreCommand GetRecordMemberTournamentScoreCommand()
        {
            return RecordMemberTournamentScoreCommand.Create(RecordMemberTournamentScoreRequest);
        }
    }
}
