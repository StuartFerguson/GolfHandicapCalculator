using System;
using System.Collections.Generic;

namespace ManagementAPI.Tournament
{
    internal class CSSScoreTableEntry
    {
        /// <summary>
        /// Gets or sets the category1 percentage.
        /// </summary>
        /// <value>
        /// The category1 percentage.
        /// </value>
        public Decimal Category1Percentage { get; set; }

        /// <summary>
        /// Gets or sets the category2 percentage.
        /// </summary>
        /// <value>
        /// The category2 percentage.
        /// </value>
        public Decimal Category2Percentage { get; set; }

        /// <summary>
        /// Gets or sets the category3 and4 percentage.
        /// </summary>
        /// <value>
        /// The category3 and4 percentage.
        /// </value>
        public Decimal Category3And4Percentage { get; set; }

        /// <summary>
        /// Gets or sets the adjustment ranges.
        /// </summary>
        /// <value>
        /// The adjustment ranges.
        /// </value>
        public List<AdjustmentRange> AdjustmentRanges { get; set; }        
    }
}