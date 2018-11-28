using System;

namespace ManagementAPI.Tournament
{
    internal class AdjustmentRange
    {
        /// <summary>
        /// Gets or sets the adjustment.
        /// </summary>
        /// <value>
        /// The adjustment.
        /// </value>
        public Int32 Adjustment { get; set; }

        /// <summary>
        /// Gets or sets the range start.
        /// </summary>
        /// <value>
        /// The range start.
        /// </value>
        public Int32 RangeStart { get; set; }

        /// <summary>
        /// Gets or sets the range end.
        /// </summary>
        /// <value>
        /// The range end.
        /// </value>
        public Int32 RangeEnd { get; set; }
    }
}