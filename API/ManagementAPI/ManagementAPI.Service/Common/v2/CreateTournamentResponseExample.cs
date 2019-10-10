using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Common.v2
{
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class CreateTournamentResponseExample : IExamplesProvider<CreateTournamentResponse>
    {
        #region Methods

        public CreateTournamentResponse GetExamples()
        {
            return new CreateTournamentResponse
                   {
                       TournamentId = Guid.Parse("665220DE-6020-4E7A-A86A-1CB6E621176B")
                   };
        }

        #endregion
    }
}
