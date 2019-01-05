using System;
using System.Collections.Generic;
using ManagementAPI.Service.Services;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.General
{
    public class HandicapAdjustmentCalculatorServiceTests
    {
        public List<Dictionary<Int32, Int32>> ScoreList = new List<Dictionary<Int32, Int32>>();

        public HandicapAdjustmentCalculatorServiceTests()
        {
            ScoreList.Add(Plus1HandicapHoleScoresCut);
            ScoreList.Add(ScratchHandicapHoleScoresCut);
            ScoreList.Add(SixHandicapHoleScoresCut);
            ScoreList.Add(ThirteenHandicapHoleScoresCut);
            ScoreList.Add(TwentyOneHandicapHoleScoresCut);
            ScoreList.Add(TwentyNineHandicapHoleScoresCut);
            ScoreList.Add(Plus1HandicapHoleScoresBuffer);
            ScoreList.Add(ScratchHandicapHoleScoresBuffer);
            ScoreList.Add(SixHandicapHoleScoresBuffer);
            ScoreList.Add(ThirteenHandicapHoleScoresBuffer);
            ScoreList.Add(TwentyOneHandicapHoleScoresBuffer);
            ScoreList.Add(TwentyNineHandicapHoleScoresBuffer);
            ScoreList.Add(Plus1HandicapHoleScoresIncrease);
            ScoreList.Add(ScratchHandicapHoleScoresIncrease);
            ScoreList.Add(SixHandicapHoleScoresIncrease);
            ScoreList.Add(ThirteenHandicapHoleScoresIncrease);
            ScoreList.Add(TwentyOneHandicapHoleScoresIncrease);
            ScoreList.Add(TwentyNineHandicapHoleScoresIncrease);
        }

        /*public static Dictionary<Int32, Int32> HoleScores = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
        };*/

        public static Dictionary<Int32, Int32> Plus1HandicapHoleScoresCut = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 3}, {18, 3}
        };

        public static Dictionary<Int32, Int32> Plus1HandicapHoleScoresBuffer = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
        };

        public static Dictionary<Int32, Int32> Plus1HandicapHoleScoresIncrease = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 5}, {18, 5}
        };

        public static Dictionary<Int32, Int32> ScratchHandicapHoleScoresCut = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 3}
        };

        public static Dictionary<Int32, Int32> ScratchHandicapHoleScoresBuffer = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 5}
        };

        public static Dictionary<Int32, Int32> ScratchHandicapHoleScoresIncrease = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 5}, {18, 5}
        };

        public static Dictionary<Int32, Int32> SixHandicapHoleScoresCut = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
            {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 4}, {15, 5}, {16, 5}, {17, 5}, {18, 5}
        };

        public static Dictionary<Int32, Int32> SixHandicapHoleScoresBuffer = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
            {10, 4}, {11, 5}, {12, 5}, {13, 6}, {14, 4}, {15, 5}, {16, 5}, {17, 5}, {18, 5}
        };

        public static Dictionary<Int32, Int32> SixHandicapHoleScoresIncrease = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
            {10, 5}, {11, 5}, {12, 5}, {13, 6}, {14, 4}, {15, 5}, {16, 5}, {17, 5}, {18, 5}
        };

        public static Dictionary<Int32, Int32> ThirteenHandicapHoleScoresCut = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 4}, {8, 5}, {9, 4},
            {10, 5}, {11, 5}, {12, 5}, {13, 6}, {14, 4}, {15, 5}, {16, 5}, {17, 5}, {18, 5}
        };

        public static Dictionary<Int32, Int32> ThirteenHandicapHoleScoresBuffer = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 3}, {4, 5}, {5, 5}, {6, 6}, {7, 4}, {8, 5}, {9, 4},
            {10, 5}, {11, 5}, {12, 5}, {13, 6}, {14, 4}, {15, 5}, {16, 5}, {17, 5}, {18, 5}
        };

        public static Dictionary<Int32, Int32> ThirteenHandicapHoleScoresIncrease = new Dictionary<Int32, Int32>()
        {
            {1, 4}, {2, 4}, {3, 4}, {4, 5}, {5, 5}, {6, 6}, {7, 4}, {8, 5}, {9, 4},
            {10, 5}, {11, 5}, {12, 5}, {13, 6}, {14, 4}, {15, 5}, {16, 5}, {17, 5}, {18, 5}
        };

        public static Dictionary<Int32, Int32> TwentyOneHandicapHoleScoresCut = new Dictionary<Int32, Int32>()
        {
            {1, 5}, {2, 5}, {3, 4}, {4, 5}, {5, 5}, {6, 6}, {7, 4}, {8, 5}, {9, 4},
            {10, 5}, {11, 5}, {12, 5}, {13, 6}, {14, 4}, {15, 5}, {16, 5}, {17, 6}, {18, 6}
        };

        public static Dictionary<Int32, Int32> TwentyOneHandicapHoleScoresBuffer = new Dictionary<Int32, Int32>()
        {
            {1, 5}, {2, 5}, {3, 4}, {4, 5}, {5, 5}, {6, 6}, {7, 4}, {8, 5}, {9, 4},
            {10, 5}, {11, 5}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        };

        public static Dictionary<Int32, Int32> TwentyOneHandicapHoleScoresIncrease = new Dictionary<Int32, Int32>()
        {
            {1, 5}, {2, 5}, {3, 4}, {4, 5}, {5, 5}, {6, 6}, {7, 4}, {8, 5}, {9, 4},
            {10, 5}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        };

        public static Dictionary<Int32, Int32> TwentyNineHandicapHoleScoresCut = new Dictionary<Int32, Int32>()
        {
            {1, 5}, {2, 5}, {3, 4}, {4, 5}, {5, 5}, {6, 6}, {7, 4}, {8, 5}, {9, 5},
            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        };

        public static Dictionary<Int32, Int32> TwentyNineHandicapHoleScoresBuffer = new Dictionary<Int32, Int32>()
        {
            {1, 5}, {2, 5}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 5}, {8, 6}, {9, 5},
            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        };

        public static Dictionary<Int32, Int32> TwentyNineHandicapHoleScoresIncrease = new Dictionary<Int32, Int32>()
        {
            {1, 5}, {2, 6}, {3, 5}, {4, 6}, {5, 6}, {6, 7}, {7, 5}, {8, 6}, {9, 5},
            {10, 6}, {11, 6}, {12, 6}, {13, 7}, {14, 5}, {15, 6}, {16, 6}, {17, 6}, {18, 6}
        };

        public enum ScoreListIndex
        {
            Plus1HandicapHoleScoresCut = 0,
            ScratchHandicapHoleScoresCut = 1,
            SixHandicapHoleScoresCut = 2,
            ThirteenHandicapHoleScoresCut = 3,
            TwentyOneHandicapHoleScoresCut = 4,
            TwentyNineHandicapHoleScoresCut = 5,
            Plus1HandicapHoleScoresBuffer = 6,
            ScratchHandicapHoleScoresBuffer = 7,
            SixHandicapHoleScoresBuffer = 8,
            ThirteenHandicapHoleScoresBuffer = 9,
            TwentyOneHandicapHoleScoresBuffer = 10,
            TwentyNineHandicapHoleScoresBuffer = 11,
            Plus1HandicapHoleScoresIncrease =12,
            ScratchHandicapHoleScoresIncrease = 13,
            SixHandicapHoleScoresIncrease = 14,
            ThirteenHandicapHoleScoresIncrease = 15,
            TwentyOneHandicapHoleScoresIncrease = 16,
            TwentyNineHandicapHoleScoresIncrease = 17,
        }

        private const Int32 CSSScore = 70;

        [Theory]
        [InlineData(-0.6,ScoreListIndex.Plus1HandicapHoleScoresCut, -0.1)]
        [InlineData(0.1, ScoreListIndex.ScratchHandicapHoleScoresCut, -0.1)]
        [InlineData(6.1, ScoreListIndex.SixHandicapHoleScoresCut, -0.2)]
        [InlineData(13.1, ScoreListIndex.ThirteenHandicapHoleScoresCut, -0.3)]
        [InlineData(21.1, ScoreListIndex.TwentyOneHandicapHoleScoresCut, -0.4)]
        [InlineData(29.1, ScoreListIndex.TwentyNineHandicapHoleScoresCut, -0.5)]
        public void HandicapAdjustmentCalculatorService_CalculateHandicapAdjustment_CuttingScores_CalculationSuccessful(Decimal exactHandicap, ScoreListIndex holeScoresIndex, Decimal adjustment)
        {
            IHandicapAdjustmentCalculatorService service = new HandicapAdjustmentCalculatorService();
            
            var adjustments = service.CalculateHandicapAdjustment(exactHandicap, CSSScore, ScoreList[(Int32)holeScoresIndex]);

            adjustments.Count.ShouldBe(1);
            adjustments[0].ShouldBe(adjustment);
        }

        [Theory]
        [InlineData(-0.6,ScoreListIndex.Plus1HandicapHoleScoresBuffer, 0.0)]
        [InlineData(0.1, ScoreListIndex.ScratchHandicapHoleScoresBuffer, 0.0)]
        [InlineData(6.1, ScoreListIndex.SixHandicapHoleScoresBuffer, 0.0)]
        [InlineData(13.1, ScoreListIndex.ThirteenHandicapHoleScoresBuffer, 0.0)]
        [InlineData(21.1, ScoreListIndex.TwentyOneHandicapHoleScoresBuffer, 0.0)]
        [InlineData(29.1, ScoreListIndex.TwentyNineHandicapHoleScoresBuffer, 0.0)]
        public void HandicapAdjustmentCalculatorService_CalculateHandicapAdjustment_BufferrScores_CalculationSuccessful(Decimal exactHandicap, ScoreListIndex holeScoresIndex, Decimal adjustment)
        {
            IHandicapAdjustmentCalculatorService service = new HandicapAdjustmentCalculatorService();
            
            var adjustments = service.CalculateHandicapAdjustment(exactHandicap, CSSScore, ScoreList[(Int32)holeScoresIndex]);

            adjustments.Count.ShouldBe(1);
            adjustments[0].ShouldBe(adjustment);
        }

        [Theory]
        [InlineData(-0.6,ScoreListIndex.Plus1HandicapHoleScoresIncrease, 0.1)]
        [InlineData(0.1, ScoreListIndex.ScratchHandicapHoleScoresIncrease, 0.1)]
        [InlineData(6.1, ScoreListIndex.SixHandicapHoleScoresIncrease, 0.1)]
        [InlineData(13.1, ScoreListIndex.ThirteenHandicapHoleScoresIncrease, 0.1)]
        [InlineData(21.1, ScoreListIndex.TwentyOneHandicapHoleScoresIncrease, 0.1)]
        [InlineData(29.1, ScoreListIndex.TwentyNineHandicapHoleScoresIncrease, 0.1)]
        public void HandicapAdjustmentCalculatorService_CalculateHandicapAdjustment_IncreasingScores_CalculationSuccessful(Decimal exactHandicap, ScoreListIndex holeScoresIndex, Decimal adjustment)
        {
            IHandicapAdjustmentCalculatorService service = new HandicapAdjustmentCalculatorService();
            
            var adjustments = service.CalculateHandicapAdjustment(exactHandicap, CSSScore, ScoreList[(Int32)holeScoresIndex]);

            adjustments.Count.ShouldBe(1);
            adjustments[0].ShouldBe(adjustment);
        }

        [Fact]
        public void HandicapAdjustmentCalculatorService_CalculateHandicapAdjustment_CuttingScores_CategoryChange_CalculationSuccessful()
        {
            IHandicapAdjustmentCalculatorService service = new HandicapAdjustmentCalculatorService();
            
            Dictionary<Int32, Int32> sixHandicapHoleScoresCut = new Dictionary<Int32, Int32>()
            {
                {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
                {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
            };
            Decimal exactHandicap = 6.1m;

            var adjustments = service.CalculateHandicapAdjustment(exactHandicap, CSSScore, sixHandicapHoleScoresCut);

            adjustments.Count.ShouldBe(6);
            adjustments[0].ShouldBe(-0.2m);
            adjustments[1].ShouldBe(-0.2m);
            adjustments[2].ShouldBe(-0.2m);
            adjustments[3].ShouldBe(-0.2m);
            adjustments[4].ShouldBe(-0.1m);
            adjustments[5].ShouldBe(-0.1m);
        }
    }
}
