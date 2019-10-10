using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Service.DataTransferObjects.Requests
{
    public class TournamentPatchRequest
    {
        public TournamentStatusUpdate Status { get; set; }

        public String CancellationReason{ get; set; }
    }

    public enum TournamentStatusUpdate
    {
        NotSet,
        Cancel,
        Complete,
        ProduceResult
    }
}
