namespace ManagementAPI.GolfClub
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    internal class TournamentDivision
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentDivision" /> class.
        /// </summary>
        /// <param name="division">The division.</param>
        /// <param name="startHandicap">The start handicap.</param>
        /// <param name="endHandicap">The end handicap.</param>
        private TournamentDivision(Int32 division,
                                   Int32 startHandicap,
                                   Int32 endHandicap)
        {
            this.Division = division;
            this.StartHandicap = startHandicap;
            this.EndHandicap = endHandicap;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the division.
        /// </summary>
        /// <value>
        /// The division.
        /// </value>
        internal Int32 Division { get; }

        /// <summary>
        /// Gets the end handicap.
        /// </summary>
        /// <value>
        /// The end handicap.
        /// </value>
        internal Int32 EndHandicap { get; }

        /// <summary>
        /// Gets the start handicap.
        /// </summary>
        /// <value>
        /// The start handicap.
        /// </value>
        internal Int32 StartHandicap { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified tournament division.
        /// </summary>
        /// <param name="division">The division.</param>
        /// <param name="startHandicap">The start handicap.</param>
        /// <param name="endHandicap">The end handicap.</param>
        /// <returns></returns>
        internal static TournamentDivision Create(Int32 division,
                                                  Int32 startHandicap,
                                                  Int32 endHandicap)
        {
            return new TournamentDivision(division, startHandicap, endHandicap);
        }

        #endregion
    }
}