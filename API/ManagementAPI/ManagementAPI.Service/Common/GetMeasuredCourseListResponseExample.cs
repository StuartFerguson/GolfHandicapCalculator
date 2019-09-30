namespace ManagementAPI.Service.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class GetMeasuredCourseListResponseExample : IExamplesProvider<GetMeasuredCourseListResponse>
    {
        #region Methods

        public GetMeasuredCourseListResponse GetExamples()
        {
            return new GetMeasuredCourseListResponse
                   {
                       GolfClubId = Guid.Parse("E80BE203-7FC4-43C2-A1A4-02306B94FA57"),
                       MeasuredCourses = new List<MeasuredCourseListResponse>
                                         {
                                             new MeasuredCourseListResponse
                                             {
                                                 MeasuredCourseId = Guid.Parse("5FE95B01-1657-41E2-8C21-E6C825253E6C"),
                                                 StandardScratchScore = 70,
                                                 TeeColour = "White",
                                                 Name = "Test Course"
                                             },
                                             new MeasuredCourseListResponse
                                             {
                                                 MeasuredCourseId = Guid.Parse("A7F8D3FD-6539-4C0C-A748-CDE603FD1D14"),
                                                 StandardScratchScore = 70,
                                                 TeeColour = "Yello",
                                                 Name = "Test Course"
                                             }
                                         }
                   };
        }

        #endregion
    }
}