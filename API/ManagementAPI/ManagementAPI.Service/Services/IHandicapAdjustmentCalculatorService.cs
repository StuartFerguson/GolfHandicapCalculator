using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Services
{
    public interface IHandicapAdjustmentCalculatorService
    {
        /// <summary>
        /// Calculates the handicap adjustment.
        /// </summary>
        /// <param name="exactHandicap">The exact handicap.</param>
        /// <param name="CSS">The CSS.</param>
        /// <param name="grossHoleScores">The gross hole scores.</param>
        /// <returns></returns>
        List<Decimal> CalculateHandicapAdjustment(Decimal exactHandicap, Int32 CSS, Dictionary<Int32,Int32> grossHoleScores);
    }
}
