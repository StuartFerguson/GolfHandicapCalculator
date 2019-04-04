namespace ManagementAPI.Service.Services
{
    using System;

    public class HandicapAdjustment
    {
        /// <summary>
        /// Gets or sets the number of strokes below CSS.
        /// </summary>
        /// <value>
        /// The number of strokes below CSS.
        /// </value>
        public Int32 NumberOfStrokesBelowCss { get; set; }

        /// <summary>
        /// Gets or sets the adjustment value per stroke.
        /// </summary>
        /// <value>
        /// The adjustment value per stroke.
        /// </value>
        public Decimal AdjustmentValuePerStroke { get; set; }

        /// <summary>
        /// Gets or sets the total adjustment.
        /// </summary>
        /// <value>
        /// The total adjustment.
        /// </value>
        public Decimal TotalAdjustment { get; set; }
    }
}