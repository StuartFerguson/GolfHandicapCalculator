using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Common.v2
{
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;

    public class CreateMatchSecretaryResponseExample : IExamplesProvider<CreateMatchSecretaryResponse>
    {
        public CreateMatchSecretaryResponse GetExamples()
        {
            return new CreateMatchSecretaryResponse
                   {
                       GolfClubId = Guid.Parse("F3524ACA-2ADB-42C7-A93E-29B6766CDDE6"),
                       UserName = "testuser@golfclub.co.uk"
                   };
        }
    }
}
