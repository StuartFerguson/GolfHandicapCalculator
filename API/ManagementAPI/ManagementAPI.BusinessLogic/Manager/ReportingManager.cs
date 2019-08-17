namespace ManagementAPI.BusinessLogic.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Threading.Tasks;
    using Database;
    using Database.Models;
    using Microsoft.EntityFrameworkCore;
    using Service.DataTransferObjects.Responses;
    using Shared.General;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ManagementAPI.BusinessLogic.Manager.IReportingManager" />
    public partial class ReportingManager : IReportingManager
    {
        #region Fields

        /// <summary>
        /// The read model resolver
        /// </summary>
        private readonly Func<ManagementAPIReadModel> ReadModelResolver;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingManager"/> class.
        /// </summary>
        /// <param name="readModelResolver">The read model resolver.</param>
        public ReportingManager(Func<ManagementAPIReadModel> readModelResolver)
        {
            this.ReadModelResolver = readModelResolver;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the number of members report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetNumberOfMembersReportResponse> GetNumberOfMembersReport(Guid golfClubId,
                                                                                     CancellationToken cancellationToken)
        {
            Guard.ThrowIfInvalidGuid(golfClubId, nameof(golfClubId));
            GetNumberOfMembersReportResponse response = new GetNumberOfMembersReportResponse();
            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                Int32 membersCount = await context.GolfClubMembershipReporting.CountAsync(g => g.GolfClubId == golfClubId, cancellationToken);
                response.NumberOfMembers = membersCount;
                response.GolfClubId = golfClubId;
            }

            return response;
        }

        /// <summary>
        /// Gets the number of members by handicap category report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetNumberOfMembersByHandicapCategoryReportResponse> GetNumberOfMembersByHandicapCategoryReport(Guid golfClubId,
                                                                     CancellationToken cancellationToken)
        {
            Guard.ThrowIfInvalidGuid(golfClubId, nameof(golfClubId));

            GetNumberOfMembersByHandicapCategoryReportResponse response = new GetNumberOfMembersByHandicapCategoryReportResponse();
            using (ManagementAPIReadModel context = this.ReadModelResolver())
            {
                response.GolfClubId = golfClubId;
                List<IGrouping<Int32, GolfClubMembershipReporting>> groupedData = await context.GolfClubMembershipReporting.Where(g => g.GolfClubId == golfClubId).GroupBy(g => g.HandicapCategory).ToListAsync(cancellationToken);

                foreach (IGrouping<Int32, GolfClubMembershipReporting> golfClubMembershipReportings in groupedData)
                {
                    response.MembersByHandicapCategoryResponse.Add(new MembersByHandicapCategoryResponse
                                                                   {
                                                                       HandicapCategory = golfClubMembershipReportings.Key,
                                                                       NumberOfMembers = golfClubMembershipReportings.Count()
                                                                   });
                }

            }

            return response;
        }

        /// <summary>
        /// Gets the number of members by time period report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="timePeriod">The time period.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetNumberOfMembersByTimePeriodReportResponse> GetNumberOfMembersByTimePeriodReport(Guid golfClubId,
                                                               String timePeriod,
                                                               CancellationToken cancellationToken)
        {
            TimePeriod parsedTimePeriod = Enum.Parse<TimePeriod>(timePeriod,true);

            GetNumberOfMembersByTimePeriodReportResponse response = new GetNumberOfMembersByTimePeriodReportResponse();
            response.GolfClubId = golfClubId;

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // get the base query data
                IQueryable<GolfClubMembershipReporting> data = context.GolfClubMembershipReporting.Where(r => r.GolfClubId == golfClubId).AsQueryable();
                List<MembersByTimePeriodResponse> membersByTimePeriodResponse = null;
                // Build the database query depending on the time period requested
                if (parsedTimePeriod == TimePeriod.Day)
                {
                    response.TimePeriod = Service.DataTransferObjects.Responses.TimePeriod.Day;
                    membersByTimePeriodResponse = data.GroupBy(x => new
                                                                    {
                                                                        x.DateJoined.Date
                                                                    },
                                                               (key,
                                                                group) => new MembersByTimePeriodResponse
                                                                          {
                                                                              Period = key.Date.ToString("yyyy-MM-dd"),
                                                                              NumberOfMembers = group.Count()
                                                                          }).ToList();
                }
                // TODO: May re-introduce this later on
                //else if (parsedTimePeriod == TimePeriod.Week)
                //{
                //    response.TimePeriod = Service.DataTransferObjects.Responses.TimePeriod.Week;
                //    membersByTimePeriodResponse = data.GroupBy(x => new
                //                                                    {
                //                                                        x.DateJoined.Date
                //                                                    },
                //                                               (key,
                //                                                group) => new MembersByTimePeriodResponse
                //                                                          {
                //                                                              Period = $"{key.Date.ToString("yyyy")}-{ISOWeek.GetWeekNumber(key.Date):D2}",
                //                                                              NumberOfMembers = group.Count()
                //                                                          }).ToList();
                //}
                else if (parsedTimePeriod == TimePeriod.Month)
                {
                    response.TimePeriod = Service.DataTransferObjects.Responses.TimePeriod.Month;
                    membersByTimePeriodResponse = data.GroupBy(x => new
                                                                    {
                                                                        x.DateJoined.Date
                                                                    },
                                                               (key,
                                                                group) => new MembersByTimePeriodResponse
                                                                          {
                                                                              Period = key.Date.ToString("yyyy-MM"),
                                                                              NumberOfMembers = group.Count()
                                                                          }).ToList();
                    membersByTimePeriodResponse = membersByTimePeriodResponse.GroupBy(g => g.Period).Select(x => new MembersByTimePeriodResponse
                                                                                                                 {
                                                                                                                     Period = x.Key,
                                                                                                                     NumberOfMembers = x.Sum(s => s.NumberOfMembers)
                                                                                                                 }).ToList();
                }
                else if (parsedTimePeriod == TimePeriod.Year)
                {
                    response.TimePeriod = Service.DataTransferObjects.Responses.TimePeriod.Year;
                    membersByTimePeriodResponse = data.GroupBy(x => new
                                                 {
                                                     Year = x.DateJoined.Date
                                                 },
                                            (key,
                                             group) => new MembersByTimePeriodResponse
                                                       {
                                                           Period = key.Year.ToString("yyyy"),
                                                           NumberOfMembers = group.Count()
                                                       }).ToList();
                    membersByTimePeriodResponse = membersByTimePeriodResponse.GroupBy(g => g.Period).Select(x => new MembersByTimePeriodResponse
                                                                                                                 {
                                                                                                                     Period = x.Key,
                                                                                                                     NumberOfMembers = x.Sum(s => s.NumberOfMembers)
                                                                                                                 }).ToList();
                }

                Int32 runningTotal = 0;
                response.MembersByTimePeriodResponse = membersByTimePeriodResponse?.OrderBy(x => x.Period).Select(x => new MembersByTimePeriodResponse
                                                                                                        {
                                                                                                            NumberOfMembers = runningTotal += x.NumberOfMembers,
                                                                                                            Period = x.Period
                                                                                                        }).ToList();
            }

            return response;
        }
        
        #endregion
    }
}