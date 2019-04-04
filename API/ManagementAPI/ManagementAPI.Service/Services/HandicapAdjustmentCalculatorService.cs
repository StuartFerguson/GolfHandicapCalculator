using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagementAPI.Service.Services
{
    using Shared.General;

    public class HandicapAdjustmentCalculatorService : IHandicapAdjustmentCalculatorService
    {
        #region Public Methods

        #region public Decimal CalculateHandicapAdjustment(Decimal exactHandicap, Int32 CSS, Dictionary<Int32, Int32> grossHoleScores)        

        /// <summary>
        /// Calculates the handicap adjustment.
        /// </summary>
        /// <param name="exactHandicap">The exact handicap.</param>
        /// <param name="CSS">The CSS.</param>
        /// <param name="grossHoleScores">The gross hole scores.</param>
        /// <returns></returns>
        public List<HandicapAdjustment> CalculateHandicapAdjustment(Decimal exactHandicap,
                                                                    Int32 CSS,
                                                                    Dictionary<Int32, Int32> grossHoleScores)
        {
            List<HandicapAdjustment> result = new List<HandicapAdjustment>();

            // Get the gross score
            Int32 grossScore = grossHoleScores.Values.Sum();

            Logger.LogInformation($"Gross Score {grossScore}");
            Logger.LogInformation($"CSS {CSS}");

            // Now determine the playing handicap
            Decimal playingHandicap = 0;
            if (exactHandicap < 0)
            {
                playingHandicap = Math.Round(exactHandicap, 0);
            }
            else
            {
                playingHandicap = Math.Round(exactHandicap, 0, MidpointRounding.AwayFromZero);    
            }

            // Now the net score
            Int32 netScore = grossScore - (Int32)playingHandicap;

            Logger.LogInformation($"Net Score {netScore}");

            // Determine the buffer zone
            Int32 bufferZone = DetermineBufferZone(playingHandicap);

            // Now get the difference from CSS
            Int32 netDifference = netScore - CSS;

            if (netDifference >= 0)
            {
                // Net score is above the CSS then check if within buffer or not
                if (netDifference > bufferZone)
                {
                    // need to adjust handicap up (0.1)                    
                    result.Add(new HandicapAdjustment
                               {
                                   AdjustmentValuePerStroke = 0,
                                   NumberOfStrokesBelowCss = 0,
                                   TotalAdjustment = 0.1m
                               });
                }

                if (netDifference <= bufferZone)
                {
                    result.Add(new HandicapAdjustment
                               {
                                   AdjustmentValuePerStroke = 0,
                                   NumberOfStrokesBelowCss = 0,
                                   TotalAdjustment = 0.0m
                               });
                }
            }
            else
            {
                Decimal workingExactHandicap = exactHandicap;
                // We need to calculate a reduction adjustment
                for (Int32 i = 0; i < netDifference * -1; i++)
                {
                    Decimal adjustmentValue = DetermineAdjustmentValue(workingExactHandicap);                    

                    workingExactHandicap = workingExactHandicap - adjustmentValue;                    

                    // Keep a running adjustment total
                    result.Add(new HandicapAdjustment
                               {
                                   AdjustmentValuePerStroke = adjustmentValue,
                                   NumberOfStrokesBelowCss = 1,
                                   TotalAdjustment = adjustmentValue * -1
                               });
                }

                result = (from r in result
                          group r by r.AdjustmentValuePerStroke
                          into g
                          orderby g.Key
                          select new HandicapAdjustment
                                 {
                                     AdjustmentValuePerStroke = g.Key,
                                     TotalAdjustment = g.Sum(x => x.TotalAdjustment),
                                     NumberOfStrokesBelowCss = g.Sum(x => x.NumberOfStrokesBelowCss)
                                 }).OrderByDescending(r => r.AdjustmentValuePerStroke).ToList();
            }
            
            return result;
        }
        #endregion

        #endregion

        #region Private Methods

        #region private Int32 DetermineBufferZone(Decimal playingHandicap)        
        /// <summary>
        /// Determines the buffer zone.
        /// </summary>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <returns></returns>
        private Int32 DetermineBufferZone(Decimal playingHandicap)
        {
            Int32 result = 0;

            if (playingHandicap < 5)
            {
                result = 1;
            }
            else if (playingHandicap >= 6 && playingHandicap <= 12)
            {
                result = 2;
            }
            else if (playingHandicap >= 13 && playingHandicap <= 20)
            {
                result = 2;
            }
            else if (playingHandicap >= 21 && playingHandicap <= 28)
            {
                result = 4;
            }
            else
            {
                result = 5;
            }

            return result;
        }
        #endregion

        #region private Decimal DetermineAdjustmentValue(Decimal exactHandicap)        
        /// <summary>
        /// Determines the adjustment value.
        /// </summary>
        /// <param name="exactHandicap">The exact handicap.</param>
        /// <returns></returns>
        private Decimal DetermineAdjustmentValue(Decimal exactHandicap)
        {
            Decimal result = 0;

            if (exactHandicap <= 5.4m)
            {
                result = 0.1m;
            }
            else if (exactHandicap >= 5.5m && exactHandicap <= 12.4m)
            {
                result = 0.2m;
            }
            else if (exactHandicap >= 12.5m && exactHandicap <= 20.4m)
            {
                result = 0.3m;
            }
            else if (exactHandicap >= 20.5m && exactHandicap <= 28.4m)
            {
                result = 0.4m;
            }
            else
            {
                result = 0.5m;
            }

            return result;
        }
        #endregion

        #endregion
    }
}