using System;
using System.Collections.Generic;
using System.Linq;
using ManagementAPI.Service.DataTransferObjects;
using ManagementAPI.Tournament;

namespace ManagementAPI.Service.Tests.Tournament
{
    using BusinessLogic.Commands;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using ManagementAPI.GolfClubMembership.DomainEvents;
    using ManagementAPI.Tournament.DomainEvents;

    public class TournamentTestData
    {
        public static Guid AggregateId = Guid.Parse("15650BE2-4F7F-40D9-B5F8-A099A713E959");
        public static DateTime TournamentDate = new DateTime(2018,4,1);
        public static Guid GolfClubId = Guid.Parse("CD64A469-9593-49D6-988D-3842C532D23E");
        public static Guid MeasuredCourseId= Guid.Parse("B2F334C2-03D3-48DB-9C6F-45FB1133F071");
        public static Int32 MeasuredCourseSSS = 70;
        public static String Name = "Test Tournament";
        public static Int32 MemberCategory = 2;
        public static ManagementAPI.Tournament.PlayerCategory PlayerCategoryEnum = ManagementAPI.Tournament.PlayerCategory.Gents;
        public static Int32 TournamentFormat = 1;
        public static ManagementAPI.Tournament.TournamentFormat TournamentFormatEnum = ManagementAPI.Tournament.TournamentFormat.Strokeplay;
        public static Guid PlayerId = Guid.Parse("9F14D8A4-D8F7-4E32-9600-C3F038E662F6");
        public static Int32 PlayingHandicap = 6;
        public static Int32 HandicapCategory = 2;
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

        public static Dictionary<Int32, Int32> HoleScoresNoReturn = new Dictionary<Int32, Int32>()
                                                            {
                                                                {1, 0}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
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

        public static Int32 Division = 1;


        public static Dictionary<Int32, Int32> HoleScoresMissingHole(Int32 holeNumber)
        {
            Dictionary<Int32, Int32> holeScores = new Dictionary<Int32, Int32>()
            {
                {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
                {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
            };

            holeScores.Remove(holeNumber);

            return holeScores;
        }

        public static Dictionary<Int32, Int32> HoleScoresNegativeScore(Int32 holeNumber)
        {
            Dictionary<Int32, Int32> holeScores = new Dictionary<Int32, Int32>()
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


        public static TournamentAggregate GetEmptyTournamentAggregate()
        {
            TournamentAggregate aggregate = TournamentAggregate.Create(TournamentTestData.AggregateId);

            return aggregate;
        }

        public static TournamentAggregate GetCreatedTournamentAggregate()
        {
            TournamentAggregate aggregate = TournamentAggregate.Create(TournamentTestData.AggregateId);

            aggregate.CreateTournament(TournamentTestData.TournamentDate, TournamentTestData.GolfClubId, TournamentTestData.MeasuredCourseId, TournamentTestData.MeasuredCourseSSS, TournamentTestData.Name, TournamentTestData.PlayerCategoryEnum, TournamentTestData.TournamentFormatEnum);

            return aggregate;
        }
        
        public static TournamentAggregate GetCreatedTournamentAggregateWithPlayerSignedUp()
        {
            TournamentAggregate aggregate = TournamentAggregate.Create(TournamentTestData.AggregateId);

            aggregate.CreateTournament(TournamentTestData.TournamentDate, TournamentTestData.GolfClubId, TournamentTestData.MeasuredCourseId, TournamentTestData.MeasuredCourseSSS, TournamentTestData.Name, TournamentTestData.PlayerCategoryEnum, TournamentTestData.TournamentFormatEnum);

            aggregate.SignUpForTournament(TournamentTestData.PlayerId);

            return aggregate;
        }

        public static TournamentAggregate GetCreatedTournamentWithScoresRecordedAggregate()
        {
            TournamentAggregate aggregate = TournamentAggregate.Create(TournamentTestData.AggregateId);

            aggregate.CreateTournament(TournamentTestData.TournamentDate, TournamentTestData.GolfClubId, TournamentTestData.MeasuredCourseId, TournamentTestData.MeasuredCourseSSS, TournamentTestData.Name, TournamentTestData.PlayerCategoryEnum, TournamentTestData.TournamentFormatEnum);

            aggregate.SignUpForTournament(TournamentTestData.PlayerId);

            aggregate.RecordPlayerScore(TournamentTestData.PlayerId, TournamentTestData.PlayingHandicap, TournamentTestData.HoleScores);

            return aggregate;
        }
        
        public static TournamentAggregate GetCompletedTournamentAggregate(Int32 category1Scores = 1, Int32 category2Scores = 2, Int32 category3Scores = 7,
            Int32 category4Scores = 20, Int32 category5Scores = 5, Int32 bufferorbetter=5)
        {
            TournamentAggregate aggregate = TournamentAggregate.Create(TournamentTestData.AggregateId);

            aggregate.CreateTournament(TournamentTestData.TournamentDate, TournamentTestData.GolfClubId, TournamentTestData.MeasuredCourseId, TournamentTestData.MeasuredCourseSSS, TournamentTestData.Name, TournamentTestData.PlayerCategoryEnum, TournamentTestData.TournamentFormatEnum);

            List<GeneratedPlayerScore> scoresToRecord = TournamentTestData.GenerateScores(category1Scores,category2Scores,category3Scores,category4Scores, category5Scores, bufferorbetter);
            foreach (GeneratedPlayerScore playerScoreForTest in scoresToRecord)
            {
                aggregate.SignUpForTournament(playerScoreForTest.PlayerId);
                aggregate.RecordPlayerScore(playerScoreForTest.PlayerId, playerScoreForTest.Handicap, playerScoreForTest.HoleScores);
            }

            aggregate.CompleteTournament(TournamentTestData.CompletedDateTime);

            return aggregate;
        }

        public static TournamentAggregate GetCancelledTournamentAggregate()
        {
            TournamentAggregate aggregate = TournamentAggregate.Create(TournamentTestData.AggregateId);

            aggregate.CreateTournament(TournamentTestData.TournamentDate, TournamentTestData.GolfClubId, TournamentTestData.MeasuredCourseId, TournamentTestData.MeasuredCourseSSS, TournamentTestData.Name, TournamentTestData.PlayerCategoryEnum, TournamentTestData.TournamentFormatEnum);

            aggregate.SignUpForTournament(TournamentTestData.PlayerId);

            aggregate.RecordPlayerScore(TournamentTestData.PlayerId, TournamentTestData.PlayingHandicap, TournamentTestData.HoleScores);

            aggregate.CancelTournament(TournamentTestData.CancelledDateTime, TournamentTestData.CancellationReason);

            return aggregate;
        }

        public static TournamentAggregate GetCompletedTournamentAggregateWithCSSCalculatedAggregate(Int32 category1Scores = 1, Int32 category2Scores = 2, Int32 category3Scores = 7,
            Int32 category4Scores = 20, Int32 category5Scores = 5, Int32 bufferorbetter=5, ManagementAPI.Tournament.TournamentFormat tournamentFormat = ManagementAPI.Tournament.TournamentFormat.Strokeplay)
        {
            TournamentAggregate aggregate = TournamentAggregate.Create(TournamentTestData.AggregateId);

            aggregate.CreateTournament(TournamentTestData.TournamentDate, TournamentTestData.GolfClubId, TournamentTestData.MeasuredCourseId, TournamentTestData.MeasuredCourseSSS, TournamentTestData.Name, TournamentTestData.PlayerCategoryEnum, tournamentFormat);

            List<GeneratedPlayerScore> scoresToRecord = TournamentTestData.GenerateScores(category1Scores,category2Scores,category3Scores,category4Scores, category5Scores, bufferorbetter);
            foreach (GeneratedPlayerScore playerScoreForTest in scoresToRecord)
            {
                aggregate.SignUpForTournament(playerScoreForTest.PlayerId);
                aggregate.RecordPlayerScore(playerScoreForTest.PlayerId, playerScoreForTest.Handicap, playerScoreForTest.HoleScores);
            }

            aggregate.CompleteTournament(TournamentTestData.CompletedDateTime);

            aggregate.CalculateCSS();

            return aggregate;
        }

        public static CreateTournamentRequest CreateTournamentRequest = new CreateTournamentRequest
        {
            Name = TournamentTestData.Name,
            MemberCategory = TournamentTestData.MemberCategory,
            MeasuredCourseId = TournamentTestData.MeasuredCourseId,
            TournamentDate = TournamentTestData.TournamentDate,
            Format = TournamentTestData.TournamentFormat
        };

        public static CreateTournamentResponse CreateTournamentResponse = new CreateTournamentResponse
        {
            TournamentId = TournamentTestData.AggregateId
        };

        public static CreateTournamentCommand GetCreateTournamentCommand()
        {
            return CreateTournamentCommand.Create( TournamentTestData.AggregateId, TournamentTestData.GolfClubId, TournamentTestData.CreateTournamentRequest);
        }

        public static RecordPlayerTournamentScoreRequest RecordMemberTournamentScoreRequest = new RecordPlayerTournamentScoreRequest
        {
            HoleScores = TournamentTestData.HoleScores
        };

        public static RecordPlayerTournamentScoreCommand GetRecordPlayerTournamentScoreCommand()
        {
            return RecordPlayerTournamentScoreCommand.Create( TournamentTestData.PlayerId, TournamentTestData.AggregateId, TournamentTestData.RecordMemberTournamentScoreRequest);
        }

        public static SignUpForTournamentCommand GetSignUpForTournamentCommand()
        {
            return SignUpForTournamentCommand.Create(TournamentTestData.AggregateId, TournamentTestData.PlayerId);
        }
        
        public static CompleteTournamentCommand GetCompleteTournamentCommand()
        {
            return CompleteTournamentCommand.Create(TournamentTestData.GolfClubId, TournamentTestData.AggregateId);
        }

        public static CancelTournamentRequest CancelTournamentRequest = new CancelTournamentRequest
        {
            CancellationReason = TournamentTestData.CancellationReason
        };

        public static Int32 DivisionPosition = 1;

        public static Decimal Last9HolesScore = 36;

        public static Decimal Last6HolesScore = 28.5m;

        public static Decimal Last3HolesScore = 15.4m;

        public static DateTime ResultDate = new DateTime(2018,5,1);

        public static Int32 PlayerCategory = 1;

        public static String GolfClubName = "Test Golf Club";

        public static GetTournamentListResponse GetTournamentListResponse = new GetTournamentListResponse
                                                                            {
                                                                                Tournaments = new List<GetTournamentResponse>
                                                                                              {
                                                                                                  new GetTournamentResponse
                                                                                                  {

                                                                                                  }
                                                                                              }
                                                                            };

        public static Guid PlayerId2 = Guid.Parse("A3023B3C-8842-4E2C-9A08-1FA3F1378307");

        public static CancelTournamentCommand GetCancelTournamentCommand()
        {
            return CancelTournamentCommand.Create(TournamentTestData.GolfClubId, TournamentTestData.AggregateId, TournamentTestData.CancelTournamentRequest);
        }

        public static ProduceTournamentResultCommand GetProduceTournamentResultCommand()
        {
            return ProduceTournamentResultCommand.Create(TournamentTestData.GolfClubId, TournamentTestData.AggregateId);
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

        private static List<GeneratedPlayerScore> GenerateScores(Int32 category1Scores, Int32 category2Scores, Int32 category3Scores,
            Int32 category4Scores, Int32 category5Scores, Int32 bufferorbetter)
        {
            Random random = new Random();

            List<GeneratedPlayerScore> scores = new List<GeneratedPlayerScore>();

            for (Int32 i = 0; i < category1Scores; i++)
            {
                GeneratedPlayerScore generatedMemberScore = new GeneratedPlayerScore
                {
                    PlayerId = Guid.NewGuid(),
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
                GeneratedPlayerScore generatedMemberScore = new GeneratedPlayerScore
                {
                    PlayerId = Guid.NewGuid(),
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
                GeneratedPlayerScore generatedMemberScore = new GeneratedPlayerScore
                {
                    PlayerId = Guid.NewGuid(),
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
                GeneratedPlayerScore generatedMemberScore = new GeneratedPlayerScore
                {
                    PlayerId = Guid.NewGuid(),
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
                GeneratedPlayerScore generatedMemberScore = new GeneratedPlayerScore
                {
                    PlayerId = Guid.NewGuid(),
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
                Int32 index = random.Next(0, scores.Count);

                while (scores[index].HoleScores[18] == 4 || scores[index].Handicap > 28)
                {
                    index = random.Next(0, scores.Count);
                }

                scores[index].HoleScores[18] = 4;

                indicies.Add(index);
            }
            return scores;
        }

        public static TournamentCreatedEvent GetTournamentCreatedEvent()
        {
            return TournamentCreatedEvent.Create(TournamentTestData.AggregateId, TournamentTestData.TournamentDate, TournamentTestData.GolfClubId, TournamentTestData.MeasuredCourseId,
                                                 TournamentTestData.MeasuredCourseSSS, TournamentTestData.Name, TournamentTestData.PlayerCategory,
                                                 TournamentTestData.TournamentFormat);
        }

        public static PlayerScorePublishedEvent GetPlayerScorePublishedEvent()
        {
            return PlayerScorePublishedEvent.Create(TournamentTestData.AggregateId,
                                                    TournamentTestData.PlayerId,
                                                    TournamentTestData.PlayingHandicap,
                                                    TournamentTestData.HoleScores,
                                                    TournamentTestData.GolfClubId,
                                                    TournamentTestData.MeasuredCourseId,
                                                    TournamentTestData.GrossScore,
                                                    TournamentTestData.NetScore,
                                                    TournamentTestData.CSS);
        }

        public static TournamentResultForPlayerScoreProducedEvent GetTournamentResultForPlayerScoreProducedEvent()
        {
            return TournamentResultForPlayerScoreProducedEvent.Create(TournamentTestData.AggregateId,
                                                                      TournamentTestData.PlayerId,
                                                                      TournamentTestData.Division,
                                                                      TournamentTestData.DivisionPosition,
                                                                      TournamentTestData.GrossScore,
                                                                      TournamentTestData.PlayingHandicap,
                                                                      TournamentTestData.NetScore,
                                                                      TournamentTestData.Last9HolesScore,
                                                                      TournamentTestData.Last6HolesScore,
                                                                      TournamentTestData.Last3HolesScore);
        }

        public static PlayerSignedUpEvent GetPlayerSignedUpEvent()
        {
            return PlayerSignedUpEvent.Create(TournamentTestData.AggregateId, TournamentTestData.PlayerId);
        }

        public static PlayerScoreRecordedEvent GetPlayerScoreRecordedEvent()
        {
            return PlayerScoreRecordedEvent.Create(TournamentTestData.AggregateId,
                                                   TournamentTestData.PlayerId,
                                                   TournamentTestData.PlayingHandicap,
                                                   TournamentTestData.HoleScores);
        }

        public static TournamentResultProducedEvent GetTournamentResultProducedEvent()
        {
            return TournamentResultProducedEvent.Create(TournamentTestData.AggregateId, TournamentTestData.ResultDate);
        }

        public static TournamentCancelledEvent GetTournamentCancelledEvent()
        {
            return TournamentCancelledEvent.Create(TournamentTestData.AggregateId, TournamentTestData.CancelledDateTime, TournamentTestData.CancellationReason);
        }

        public static TournamentCompletedEvent GetTournamentCompletedEvent()
        {
            return TournamentCompletedEvent.Create(TournamentTestData.AggregateId, TournamentTestData.CompletedDateTime);
        }
    }

    public class GeneratedPlayerScore
    {
        public Guid PlayerId { get; set; }
        public Int32 Handicap { get; set; }
        public Dictionary<Int32, Int32> HoleScores { get; set; }

        public Int32 GetGrossScore()
        {
            return this.HoleScores.Values.Sum();
        }

        public Int32 GetNetScore()
        {
            return this.HoleScores.Values.Sum() - this.Handicap;
        }

        public Boolean BufferOrBeter(Int32 sss)
        {
            Int32 category = 0;

            if (this.Handicap <= 5) category = 1;
            if (this.Handicap >= 6 && this.Handicap <= 12) category = 2;
            if (this.Handicap >= 13 && this.Handicap <= 20) category = 3;
            if (this.Handicap >= 21 && this.Handicap <= 28) category = 4;
            if (this.Handicap >= 29 && this.Handicap <= 36) category = 5;

            return (this.HoleScores.Values.Sum() - this.Handicap) - sss <= category;
        }
    }
}
