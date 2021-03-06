﻿namespace ManagementAPI.Service.Common.v2
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses;
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class RegisterClubAdministratorResponseExample : IExamplesProvider<RegisterClubAdministratorResponse>
    {
        #region Methods

        public RegisterClubAdministratorResponse GetExamples()
        {
            return new RegisterClubAdministratorResponse
                   {
                       GolfClubAdministratorId = Guid.Parse("48F7A16D-CCAE-4875-B3AE-B0E93C724F4A")
                   };
        }

        #endregion
    }
}