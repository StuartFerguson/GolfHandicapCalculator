namespace ManagementAPI.Service.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class CreateTournamentResponseExample : IExamplesProvider
    {
        #region Methods

        public Object GetExamples()
        {
            return new CreateTournamentResponse
                   {
                       TournamentId = Guid.Parse("665220DE-6020-4E7A-A86A-1CB6E621176B")
                   };
        }

        #endregion
    }
}