namespace ManagementAPI.Service.Common.v2
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class RegisterPlayerResponseExample : IExamplesProvider<RegisterPlayerResponse>
    {
        #region Methods

        public RegisterPlayerResponse GetExamples()
        {
            return new RegisterPlayerResponse
                   {
                       PlayerId = Guid.Parse("BFC2FC37-86B3-4EAD-9CFA-6AF60803440F")
                   };
        }

        #endregion
    }
}