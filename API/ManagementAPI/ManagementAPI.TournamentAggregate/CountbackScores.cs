namespace ManagementAPI.Tournament
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    internal class CountbackScores
    {
        /// <summary>
        /// Gets or sets the last9 holes.
        /// </summary>
        /// <value>
        /// The last9 holes.
        /// </value>
        internal Decimal Last9Holes { get; set; }
        /// <summary>
        /// Gets or sets the last6 holes.
        /// </summary>
        /// <value>
        /// The last6 holes.
        /// </value>
        internal Decimal Last6Holes { get; set; }
        /// <summary>
        /// Gets or sets the last3 holes.
        /// </summary>
        /// <value>
        /// The last3 holes.
        /// </value>
        internal Decimal Last3Holes { get; set; }
    }
}