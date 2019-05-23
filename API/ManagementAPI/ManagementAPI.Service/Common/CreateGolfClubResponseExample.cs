namespace ManagementAPI.Service.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class CreateGolfClubResponseExample : IExamplesProvider
    {
        #region Methods

        public Object GetExamples()
        {
            return new CreateGolfClubResponse
                   {
                       GolfClubId = Guid.Parse("F303BB78-40FF-495B-A21B-8AF136934CEB")
                   };
        }

        #endregion
    }
}