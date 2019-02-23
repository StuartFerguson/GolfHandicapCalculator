namespace ManagementAPI.Service.Common
{
    using System;
    using DataTransferObjects;
    using Swashbuckle.AspNetCore.Filters;

    public class GetPlayerDetailsResponseExample : IExamplesProvider
    {
        #region Methods

        public Object GetExamples()
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