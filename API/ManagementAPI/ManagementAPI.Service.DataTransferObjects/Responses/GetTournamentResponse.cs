using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    public class GetTournamentResponse
    {
        public Guid TournamentId { get; set; }

        public TournamentFormat TournamentFormat { get; set; }

        public Boolean HasResultBeenProduced { get; set; }

        public Guid MeasuredCourseId { get; set; }

        public String MeasuredCourseName { get; set; }

        public Int32 MeasuredCourseSSS { get; set; }

        public String MeasuredCourseTeeColour { get; set; }

        public String TournamentName { get; set; }

        public PlayerCategory PlayerCategory { get; set; }

        public DateTime TournamentDate { get; set; }

        public Int32 PlayersSignedUpCount { get; set; }

        public Int32 PlayersScoresRecordedCount { get; set; }
    }

    public enum PlayerCategory
    {
        /// <summary>
        /// The gents
        /// </summary>
        Gents = 1,

        /// <summary>
        /// The gents senior
        /// </summary>
        GentsSenior,

        /// <summary>
        /// The female
        /// </summary>
        Female,

        /// <summary>
        /// The female senior
        /// </summary>
        FemaleSenior,

        /// <summary>
        /// The junior
        /// </summary>
        Junior
    }

    public enum TournamentFormat
    {
        StrokePlay = 1,
        Stableford = 2
    }

    public class GetTournamentListResponse
    {
        public List<GetTournamentResponse> Tournaments { get; set; }
    }
}
