namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// 
    /// </summary>
    public class GetNumberOfMembersByTimePeriodReportResponse
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNumberOfMembersByTimePeriodReportResponse"/> class.
        /// </summary>
        public GetNumberOfMembersByTimePeriodReportResponse()
        {
            this.MembersByTimePeriodResponse = new List<MembersByTimePeriodResponse>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { get; set; }

        /// <summary>
        /// Gets or sets the members by time period response.
        /// </summary>
        /// <value>
        /// The members by time period response.
        /// </value>
        public List<MembersByTimePeriodResponse> MembersByTimePeriodResponse { get; set; }

        /// <summary>
        /// Gets or sets the time period.
        /// </summary>
        /// <value>
        /// The time period.
        /// </value>
        [JsonConverter(typeof(StringEnumConverter))]
        public TimePeriod TimePeriod { get; set; }

        #endregion
    }
}