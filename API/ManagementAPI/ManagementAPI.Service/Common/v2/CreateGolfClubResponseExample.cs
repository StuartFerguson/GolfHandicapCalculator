namespace ManagementAPI.Service.Common.v2
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects;
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class CreateGolfClubResponseExample : IExamplesProvider<CreateGolfClubResponse>
    {
        #region Methods

        public CreateGolfClubResponse GetExamples()
        {
            return new CreateGolfClubResponse
                   {
                       GolfClubId = Guid.Parse("F303BB78-40FF-495B-A21B-8AF136934CEB")
                   };
        }

        #endregion
    }
}