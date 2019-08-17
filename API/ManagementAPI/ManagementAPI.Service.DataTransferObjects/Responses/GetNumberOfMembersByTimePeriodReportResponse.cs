using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class GetNumberOfMembersByTimePeriodReportResponse
    {
        public GetNumberOfMembersByTimePeriodReportResponse()
        {
            this.MembersByTimePeriodResponse = new List<MembersByTimePeriodResponse>();
        }
        public Guid GolfClubId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TimePeriod TimePeriod { get; set; }

        public List<MembersByTimePeriodResponse> MembersByTimePeriodResponse { get; set; }
    }

    public enum TimePeriod
    {
        Day,
        Week,
        Month,
        Year
    }

    public class MembersByTimePeriodResponse
    {
        public String Period { get; set; }

        public Int32 NumberOfMembers { get; set; }
    }
}
