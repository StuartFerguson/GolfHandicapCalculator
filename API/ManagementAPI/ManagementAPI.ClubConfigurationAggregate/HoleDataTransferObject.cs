using System;

namespace ManagementAPI.ClubConfigurationAggregate
{
    public class HoleDataTransferObject
    {
        /// <summary>
        /// Gets the hole number.
        /// </summary>
        /// <value>
        /// The hole number.
        /// </value>
        public Int32 HoleNumber{ get; set; }

        /// <summary>
        /// Gets or sets the length in yards.
        /// </summary>
        /// <value>
        /// The length in yards.
        /// </value>
        public Int32 LengthInYards{ get; set; }

        /// <summary>
        /// Gets or sets the length in meters.
        /// </summary>
        /// <value>
        /// The length in meters.
        /// </value>
        public Int32 LengthInMeters{ get; set; }

        /// <summary>
        /// Gets the par.
        /// </summary>
        /// <value>
        /// The par.
        /// </value>
        public Int32 Par { get; set; }

        /// <summary>
        /// Gets the index of the stroke.
        /// </summary>
        /// <value>
        /// The index of the stroke.
        /// </value>
        public Int32 StrokeIndex{ get; set; }
    }
}