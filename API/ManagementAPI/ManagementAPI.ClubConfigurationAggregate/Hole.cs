using System;

namespace ManagementAPI.GolfClub
{
    /// <summary>
    /// 
    /// </summary>
    internal class Hole
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Hole" /> class.
        /// </summary>
        /// <param name="holeNumber">The hole number.</param>
        /// <param name="lengthInYards">The length in yards.</param>
        /// <param name="lengthInMeters">The length in meters.</param>
        /// <param name="par">The par.</param>
        /// <param name="strokeIndex">Index of the stroke.</param>
        private Hole(Int32 holeNumber, Int32 lengthInYards, Int32 lengthInMeters, Int32 par, Int32 strokeIndex)
        {
            this.HoleNumber = holeNumber;
            this.LengthInYards = lengthInYards;
            this.LengthInMeters = lengthInMeters;
            this.Par = par;
            this.StrokeIndex = strokeIndex;
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the hole number.
        /// </summary>
        /// <value>
        /// The hole number.
        /// </value>
        internal Int32 HoleNumber{ get; private set; }

        /// <summary>
        /// Gets the length in yards.
        /// </summary>
        /// <value>
        /// The length in yards.
        /// </value>
        internal Int32 LengthInYards{ get; private set; }

        /// <summary>
        /// Gets the length in meters.
        /// </summary>
        /// <value>
        /// The length in meters.
        /// </value>
        internal Int32 LengthInMeters{ get; private set; }

        /// <summary>
        /// Gets the par.
        /// </summary>
        /// <value>
        /// The par.
        /// </value>
        internal Int32 Par { get; private set; }

        /// <summary>
        /// Gets the index of the stroke.
        /// </summary>
        /// <value>
        /// The index of the stroke.
        /// </value>
        internal Int32 StrokeIndex{ get; private set; }

        #endregion

        #region Internal Methods

        #region internal static Hole Create(Int32 holeNumber, Int32 lengthInYards, Int32 lengthInMeters, Int32 par, Int32 strokeIndex)
        /// <summary>
        /// Creates the specified hole number.
        /// </summary>
        /// <param name="holeNumber">The hole number.</param>
        /// <param name="lengthInYards">The length in yards.</param>
        /// <param name="lengthInMeters">The length in meters.</param>
        /// <param name="par">The par.</param>
        /// <param name="strokeIndex">Index of the stroke.</param>
        /// <returns></returns>
        internal static Hole Create(Int32 holeNumber, Int32 lengthInYards, Int32 lengthInMeters, Int32 par, Int32 strokeIndex)
        {
            return new Hole(holeNumber, lengthInYards, lengthInMeters, par, strokeIndex);
        }
        #endregion

        #endregion
    }
}