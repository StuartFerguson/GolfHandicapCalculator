namespace ManagementAPI.GolfClub
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    internal class MeasuredCourse
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasuredCourse" /> class.
        /// </summary>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="teeColour">The tee colour.</param>
        /// <param name="standardScratchScore">The standard scratch score.</param>
        private MeasuredCourse(Guid measuredCourseId,
                               String name,
                               String teeColour,
                               Int32 standardScratchScore)
        {
            this.MeasuredCourseId = measuredCourseId;
            this.Name = name;
            this.TeeColour = teeColour;
            this.StandardScratchScore = standardScratchScore;
            this.Holes = new List<Hole>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the holes.
        /// </summary>
        /// <value>
        /// The holes.
        /// </value>
        internal List<Hole> Holes { get; }

        /// <summary>
        /// Gets the course identifier.
        /// </summary>
        /// <value>
        /// The course identifier.
        /// </value>
        internal Guid MeasuredCourseId { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        internal String Name { get; }

        /// <summary>
        /// Gets the standard scratch score.
        /// </summary>
        /// <value>
        /// The standard scratch score.
        /// </value>
        internal Int32 StandardScratchScore { get; }

        /// <summary>
        /// Gets the tee colour.
        /// </summary>
        /// <value>
        /// The tee colour.
        /// </value>
        internal String TeeColour { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the hole.
        /// </summary>
        /// <param name="hole">The hole.</param>
        internal void AddHole(Hole hole)
        {
            this.Holes.Add(hole);
        }

        /// <summary>
        /// Creates the specified tee colour.
        /// </summary>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="teeColour">The tee colour.</param>
        /// <param name="standardScratchScore">The standard scratch score.</param>
        /// <returns></returns>
        internal static MeasuredCourse Create(Guid measuredCourseId,
                                              String name,
                                              String teeColour,
                                              Int32 standardScratchScore)
        {
            return new MeasuredCourse(measuredCourseId, name, teeColour, standardScratchScore);
        }

        #endregion
    }
}