namespace ManagementAPI.Service.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class GetGolfClubResponseExample : IExamplesProvider
    {
        #region Methods

        public Object GetExamples()
        {
            return new GetGolfClubResponse
                   {
                       Name = "Test Golf Club",
                       EmailAddress = "testemail@golfclub.com",
                       TelephoneNumber = "1234567890",
                       AddressLine1 = "Address Line 1",
                       AddressLine2 = string.Empty,
                       Id = Guid.Parse("3E65A026-6331-40C7-B42C-DFF27D133E60"),
                       PostalCode = "TE57 1NG",
                       Region = "TestRegion",
                       Town = "TestTown",
                       Website = "www.golfclub.com"
                   };
        }

        #endregion
    }
}