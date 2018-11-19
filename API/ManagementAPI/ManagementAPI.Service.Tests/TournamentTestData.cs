using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        public static Int32 MeasuredCourseSSS = 70;
        public static String Name = "Test Tournament";
        public static Int32 MemberCategory = 2;
        public static MemberCategory MemberCategoryEnum = TournamentAggregate.MemberCategory.Gents;
        public static Int32 TournamentFormat = 1;
        public static TournamentFormat TournamentFormatEnum = TournamentAggregate.TournamentFormat.Strokeplay;
        public static Guid MemberId = Guid.Parse("9F14D8A4-D8F7-4E32-9600-C3F038E662F6");
        public static Int32 PlayingHandicap = 6;
        public static Int32 Adjustment = 1;
        public static Int32 CSS = 71;
        public static Int32 GrossScore = 76;
        public static Int32 NetScore = 70;
        public static List<Decimal> Adjustments = new List<Decimal>
        {
            -0.2m
        };

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

        public static DateTime CompletedDateTime = new DateTime(2018,11,5);

        public static DateTime CancelledDateTime = new DateTime(2018,11,6);
        public static String CancellationReason = "Cancelled";


        public static TournamentAggregate.TournamentAggregate GetEmptyTournamentAggregate()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentAggregate.TournamentAggregate.Create(AggregateId);

            return aggregate;
        }

        public static TournamentAggregate.TournamentAggregate GetCreatedTournamentAggregate()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentAggregate.TournamentAggregate.Create(AggregateId);

            aggregate.CreateTournament(TournamentDate, ClubConfigurationId, MeasuredCourseId, MeasuredCourseSSS, Name, MemberCategoryEnum, TournamentFormatEnum);

            return aggregate;
        }

        public static TournamentAggregate.TournamentAggregate GetCreatedTournamentWithScoresRecordedAggregate()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentAggregate.TournamentAggregate.Create(AggregateId);

            aggregate.CreateTournament(TournamentDate, ClubConfigurationId, MeasuredCourseId,MeasuredCourseSSS, Name, MemberCategoryEnum, TournamentFormatEnum);

            aggregate.RecordMemberScore(MemberId, PlayingHandicap, HoleScores);

            return aggregate;
        }
        
        public static TournamentAggregate.TournamentAggregate GetCompletedTournamentAggregate(Int32 category1Scores = 1, Int32 category2Scores = 2, Int32 category3Scores = 7,
            Int32 category4Scores = 20, Int32 category5Scores = 5, Int32 bufferorbetter=5)
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentAggregate.TournamentAggregate.Create(AggregateId);

            aggregate.CreateTournament(TournamentDate, ClubConfigurationId, MeasuredCourseId, MeasuredCourseSSS, Name, MemberCategoryEnum, TournamentFormatEnum);

            var scoresToRecord = GenerateScores(category1Scores,category2Scores,category3Scores,category4Scores, category5Scores, bufferorbetter);
            foreach (var memberScoreForTest in scoresToRecord)
            {
                aggregate.RecordMemberScore(memberScoreForTest.MemberId, memberScoreForTest.Handicap, memberScoreForTest.HoleScores);
            }

            aggregate.CompleteTournament(CompletedDateTime);

            return aggregate;
        }

        public static TournamentAggregate.TournamentAggregate GetCancelledTournament()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentAggregate.TournamentAggregate.Create(AggregateId);

            aggregate.CreateTournament(TournamentDate, ClubConfigurationId, MeasuredCourseId, MeasuredCourseSSS, Name, MemberCategoryEnum, TournamentFormatEnum);

            aggregate.RecordMemberScore(MemberId, PlayingHandicap, HoleScores);

            aggregate.CancelTournament(CancelledDateTime, CancellationReason);

            return aggregate;
        }

        public static TournamentAggregate.TournamentAggregate GetCompletedTournamentAggregateWithCSSCalculated(Int32 category1Scores = 1, Int32 category2Scores = 2, Int32 category3Scores = 7,
            Int32 category4Scores = 20, Int32 category5Scores = 5, Int32 bufferorbetter=5)
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentAggregate.TournamentAggregate.Create(AggregateId);

            aggregate.CreateTournament(TournamentDate, ClubConfigurationId, MeasuredCourseId, MeasuredCourseSSS, Name, MemberCategoryEnum, TournamentFormatEnum);

            var scoresToRecord = GenerateScores(category1Scores,category2Scores,category3Scores,category4Scores, category5Scores, bufferorbetter);
            foreach (var memberScoreForTest in scoresToRecord)
            {
                aggregate.RecordMemberScore(memberScoreForTest.MemberId, memberScoreForTest.Handicap, memberScoreForTest.HoleScores);
            }

            aggregate.CompleteTournament(CompletedDateTime);

            aggregate.CalculateCSS();

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
            HoleScores = HoleScores
        };

        public static RecordMemberTournamentScoreCommand GetRecordMemberTournamentScoreCommand()
        {
            return RecordMemberTournamentScoreCommand.Create(AggregateId, RecordMemberTournamentScoreRequest);
        }
        
        public static CompleteTournamentCommand GetCompleteTournamentCommand()
        {
            return CompleteTournamentCommand.Create(AggregateId);
        }

        public static CancelTournamentRequest CancelTournamentRequest = new CancelTournamentRequest
        {
            CancellationReason = CancellationReason
        };

        public static CancelTournamentCommand GetCancelTournamentCommand()
        {
            return CancelTournamentCommand.Create(AggregateId, CancelTournamentRequest);
        }

        public static ProduceTournamentResultCommand GetProduceTournamentResultCommand()
        {
            return ProduceTournamentResultCommand.Create(AggregateId);
        }

        //public static List<MemberScoreForTest> GetScoreForCSSTests()
        //{
        //    List<MemberScoreForTest> result = new List<MemberScoreForTest>();

        //    #region Category 1 Scores
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 0, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 7}, {7, 3}, {8, 4}, {9, 3},
        //            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
        //        }
        //    });
        //    #endregion

        //    #region Category 2 Scores
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 6, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 4}, {2, 4}, {3, 4}, {4, 4}, {5, 4}, {6, 6}, {7, 4}, {8, 4}, {9, 4},
        //            {10, 4}, {11, 4}, {12, 4}, {13, 6}, {14, 4}, {15, 4}, {16, 4}, {17, 4}, {18, 8}
        //        }
        //    });

        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 6, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 4}, {2, 4}, {3, 4}, {4, 4}, {5, 4}, {6, 6}, {7, 4}, {8, 4}, {9, 4},
        //            {10, 4}, {11, 4}, {12, 4}, {13, 6}, {14, 4}, {15, 4}, {16, 4}, {17, 4}, {18, 8}
        //        }
        //    });
        //    #endregion

        //    #region Category 3 Scores
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 13, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 5}, {2, 5}, {3, 4}, {4, 5}, {5, 5}, {6, 6}, {7, 4}, {8, 5}, {9, 4},
        //            {10, 5}, {11, 5}, {12, 5}, {13, 6}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 13, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 5}, {2, 5}, {3, 4}, {4, 5}, {5, 5}, {6, 6}, {7, 4}, {8, 5}, {9, 4},
        //            {10, 5}, {11, 5}, {12, 5}, {13, 6}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 13, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 5}, {2, 5}, {3, 4}, {4, 5}, {5, 5}, {6, 6}, {7, 4}, {8, 5}, {9, 4},
        //            {10, 5}, {11, 5}, {12, 5}, {13, 6}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 13, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 5}, {2, 5}, {3, 4}, {4, 5}, {5, 5}, {6, 6}, {7, 4}, {8, 5}, {9, 4},
        //            {10, 5}, {11, 5}, {12, 5}, {13, 6}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 13, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 5}, {2, 5}, {3, 4}, {4, 5}, {5, 5}, {6, 6}, {7, 4}, {8, 5}, {9, 4},
        //            {10, 5}, {11, 5}, {12, 5}, {13, 6}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 13, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 5}, {2, 5}, {3, 4}, {4, 5}, {5, 5}, {6, 6}, {7, 4}, {8, 5}, {9, 4},
        //            {10, 5}, {11, 5}, {12, 5}, {13, 6}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 8}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 13, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 5}, {2, 5}, {3, 4}, {4, 5}, {5, 5}, {6, 6}, {7, 4}, {8, 5}, {9, 4},
        //            {10, 5}, {11, 5}, {12, 5}, {13, 6}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 8}
        //        }
        //    });
        //    #endregion

        //    #region Category 4 Scores

        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 22, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 6}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 6}, {8, 6}, {9, 5},
        //            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        //        }
        //    });


        //    #endregion

        //    #region Category 5 Scores
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 29, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 10}, {2, 10}, {3, 8}, {4, 10}, {5, 10}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
        //            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 29, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 10}, {2, 10}, {3, 8}, {4, 10}, {5, 10}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
        //            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 29, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 10}, {2, 10}, {3, 8}, {4, 10}, {5, 10}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
        //            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 29, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 10}, {2, 10}, {3, 8}, {4, 10}, {5, 10}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
        //            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
        //        }
        //    });
        //    result.Add(new MemberScoreForTest
        //    {
        //        MemberId = Guid.NewGuid(), PlayingHandicap = 29, HoleScores = new Dictionary<Int32, Int32>()
        //        {
        //            {1, 10}, {2, 10}, {3, 8}, {4, 10}, {5, 10}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
        //            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
        //        }
        //    });
        //    #endregion

        //    return result;
        //}

        private static List<GeneratedMemberScore> GenerateScores(Int32 category1Scores, Int32 category2Scores, Int32 category3Scores,
            Int32 category4Scores, Int32 category5Scores, Int32 bufferorbetter)
        {
            Random random = new Random();

            List<GeneratedMemberScore> scores = new List<GeneratedMemberScore>();

            for (Int32 i = 0; i < category1Scores; i++)
            {
                GeneratedMemberScore generatedMemberScore = new GeneratedMemberScore
                {
                    MemberId = Guid.NewGuid(),
                    Handicap = random.Next(0, 5),
                    HoleScores = new Dictionary<Int32, Int32>()
                    {
                        {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
                        {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18,4}
                    }
                };

                // Ensure the member has not played to their handicap
                generatedMemberScore.HoleScores[18] =
                    generatedMemberScore.HoleScores[18] + generatedMemberScore.Handicap + 2;

                scores.Add(generatedMemberScore);
            }

            for (Int32 i = 0; i < category2Scores; i++)
            {
                GeneratedMemberScore generatedMemberScore = new GeneratedMemberScore
                {
                    MemberId = Guid.NewGuid(),
                    Handicap = random.Next(6, 12),
                    HoleScores = new Dictionary<Int32, Int32>()
                    {
                        {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
                        {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18,4}
                    }
                };

                // Ensure the member has not played to their handicap
                generatedMemberScore.HoleScores[18] =
                    generatedMemberScore.HoleScores[18] + generatedMemberScore.Handicap + 3;

                scores.Add(generatedMemberScore);
            }

            for (Int32 i = 0; i < category3Scores; i++)
            {
                GeneratedMemberScore generatedMemberScore = new GeneratedMemberScore
                {
                    MemberId = Guid.NewGuid(),
                    Handicap = random.Next(13, 20),
                    HoleScores = new Dictionary<Int32, Int32>()
                    {
                        {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
                        {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18,4}
                    }
                };

                // Ensure the member has not played to their handicap
                generatedMemberScore.HoleScores[18] =
                    generatedMemberScore.HoleScores[18] + generatedMemberScore.Handicap + 4;

                scores.Add(generatedMemberScore);
            }

            for (Int32 i = 0; i < category4Scores; i++)
            {
                GeneratedMemberScore generatedMemberScore = new GeneratedMemberScore
                {
                    MemberId = Guid.NewGuid(),
                    Handicap = random.Next(21, 28),
                    HoleScores = new Dictionary<Int32, Int32>()
                    {
                        {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
                        {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18,4}
                    }
                };

                // Ensure the member has not played to their handicap
                generatedMemberScore.HoleScores[18] =
                    generatedMemberScore.HoleScores[18] + generatedMemberScore.Handicap + 5;

                scores.Add(generatedMemberScore);
            }

            for (Int32 i = 0; i < category5Scores; i++)
            {
                GeneratedMemberScore generatedMemberScore = new GeneratedMemberScore
                {
                    MemberId = Guid.NewGuid(),
                    Handicap = random.Next(29, 36),
                    HoleScores = new Dictionary<Int32, Int32>()
                    {
                        {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
                        {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18,4}
                    }
                };

                // Ensure the member has not played to their handicap
                generatedMemberScore.HoleScores[18] =
                    generatedMemberScore.HoleScores[18] + generatedMemberScore.Handicap + 6;

                scores.Add(generatedMemberScore);
            }
            
            List<Int32> indicies = new List<Int32>();
            // now set buffer or better
            for (Int32 i = 0; i < bufferorbetter; i++)
            {
                var index = random.Next(0, scores.Count);

                while (scores[index].HoleScores[18] == 4 || scores[index].Handicap > 28)
                {
                    index = random.Next(0, scores.Count);
                }

                scores[index].HoleScores[18] = 4;

                indicies.Add(index);
            }
            return scores;
        }
    }

    //public class MemberScoreForTest
    //{
    //    public Guid MemberId { get; set; }
    //    public Int32 PlayingHandicap { get; set; }
    //    public Dictionary<Int32, Int32> HoleScores  { get; set; }
    //}

    public class GeneratedMemberScore
    {
        public Guid MemberId { get; set; }
        public Int32 Handicap { get; set; }
        public Dictionary<Int32, Int32> HoleScores { get; set; }

        public Int32 GetGrossScore()
        {
            return HoleScores.Values.Sum();
        }

        public Int32 GetNetScore()
        {
            return HoleScores.Values.Sum() - Handicap;
        }

        public Boolean BufferOrBeter(Int32 sss)
        {
            Int32 category = 0;

            if (Handicap <= 5) category = 1;
            if (Handicap >= 6 && Handicap <= 12) category = 2;
            if (Handicap >= 13 && Handicap <= 20) category = 3;
            if (Handicap >= 21 && Handicap <= 28) category = 4;
            if (Handicap >= 29 && Handicap <= 36) category = 5;

            return (HoleScores.Values.Sum() - Handicap) - sss <= category;
        }
    }
}
