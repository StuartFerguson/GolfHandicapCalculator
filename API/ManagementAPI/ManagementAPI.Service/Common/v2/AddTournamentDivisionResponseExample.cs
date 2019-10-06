using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Common.v2
{
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;

    public class AddTournamentDivisionToGolfClubResponseExample : IExamplesProvider<AddTournamentDivisionToGolfClubResponse>
    {
        public AddTournamentDivisionToGolfClubResponse GetExamples()
        {
            return new AddTournamentDivisionToGolfClubResponse
            {
                       GolfClubId = Guid.Parse("F303BB78-40FF-495B-A21B-8AF136934CEB"),
                       TournamentDivision = 1
                   };
        }
    }
}
