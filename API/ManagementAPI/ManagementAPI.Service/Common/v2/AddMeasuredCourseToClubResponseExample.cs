namespace ManagementAPI.Service.Common.v2
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class AddMeasuredCourseToClubResponseExample : IExamplesProvider<AddMeasuredCourseToClubResponse>
    {
        public AddMeasuredCourseToClubResponse GetExamples()
        {
            return new AddMeasuredCourseToClubResponse
                   {
                       GolfClubId = Guid.Parse("F303BB78-40FF-495B-A21B-8AF136934CEB"),
                       MeasuredCourseId = Guid.NewGuid()
            };
        }
    }
}