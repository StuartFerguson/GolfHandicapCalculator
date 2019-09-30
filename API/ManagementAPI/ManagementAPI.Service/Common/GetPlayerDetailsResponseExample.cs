namespace ManagementAPI.Service.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class GetPlayerDetailsResponseExample : IExamplesProvider<GetPlayerDetailsResponse>
    {
        #region Methods

        public GetPlayerDetailsResponse GetExamples()
        {
            return new GetPlayerDetailsResponse
                   {
                       DateOfBirth = DateTime.Now.Date,
                       EmailAddress = "testplayer@playeremail.com",
                       ExactHandicap = 5.9m,
                       FirstName = "Test",
                       LastName = "Player",
                       MiddleName = string.Empty,
                       Gender = "M",
                       FullName = "Test Player",
                       HasBeenRegistered = true,
                       HandicapCategory = 2,
                       PlayingHandicap = 6
                   };
        }

        #endregion
    }
}