﻿using System;
using System.Collections.Generic;

namespace ManagementAPI.GolfClub
{
    public class MeasuredCourseDataTransferObject
    {
        /// <summary>
        /// Gets or sets the measured course identifier.
        /// </summary>
        /// <value>
        /// The measured course identifier.
        /// </value>
        public Guid MeasuredCourseId { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }
        
        /// <summary>
        /// Gets the tee colour.
        /// </summary>
        /// <value>
        /// The tee colour.
        /// </value>
        public String TeeColour { get; set; }

        /// <summary>
        /// Gets the standard scratch score.
        /// </summary>
        /// <value>
        /// The standard scratch score.
        /// </value>
        public Int32 StandardScratchScore { get; set; }

        /// <summary>
        /// Gets the holes.
        /// </summary>
        /// <value>
        /// The holes.
        /// </value>
        public List<HoleDataTransferObject> Holes{ get; set; }
    }
}