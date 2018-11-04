using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.DataTransferObjects
{
    public class AddMeasuredCourseToClubRequest
    {
        /// <summary>
        /// Gets or sets the club identifier.
        /// </summary>
        /// <value>
        /// The club identifier.
        /// </value>
        public Guid ClubAggregateId { get; set; }
        
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
        public List<HoleDataTransferObject> Holes { get; set; }
    }
}
