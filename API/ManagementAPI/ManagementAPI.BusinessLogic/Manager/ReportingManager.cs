namespace ManagementAPI.BusinessLogic.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
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
        /// Gets the members handicap list report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetMembersHandicapListReportResponse> GetMembersHandicapListReport(Guid golfClubId,
                                                       CancellationToken cancellationToken)
        {
            Guard.ThrowIfInvalidGuid(golfClubId, nameof(golfClubId));
            GetMembersHandicapListReportResponse response = new GetMembersHandicapListReportResponse();
            using (ManagementAPIReadModel context = this.ReadModelResolver())
            {
                List<PlayerHandicapListReporting> reportData = context.PlayerHandicapListReporting.Where(p => p.GolfClubId == golfClubId).ToList();
                response.GolfClubId = golfClubId;
                reportData.ForEach(f => response.MembersHandicapListReportResponse.Add(new MembersHandicapListReportResponse
                                                                                       {
                                                                                           GolfClubId = f.GolfClubId,
                                                                                           PlayerId = f.PlayerId,
                                                                                           HandicapCategory = f.HandicapCategory,
                                                                                           ExactHandicap = f.ExactHandicap,
                                                                                           PlayingHandicap = f.PlayingHandicap,
                                                                                           PlayerName = f.PlayerName
                                                                                       }));
            }

            return response;
        }

        /// <summary>
        /// Gets the player scores report.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="numberOfScores">The number of scores.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetPlayerScoresResponse> GetPlayerScoresReport(Guid playerId,
                                                Int32 numberOfScores,
                                                CancellationToken cancellationToken)
        {
            Guard.ThrowIfInvalidGuid(playerId, nameof(playerId));
            Guard.ThrowIfZero(numberOfScores, nameof(numberOfScores));
            Guard.ThrowIfNegative(numberOfScores, nameof(numberOfScores));

            GetPlayerScoresResponse response = new GetPlayerScoresResponse();

            using (ManagementAPIReadModel context = this.ReadModelResolver())
            {
                List<PublishedPlayerScore> reportData = await context.PublishedPlayerScores.Where(p => p.PlayerId == playerId).OrderByDescending(p => p.TournamentDate).Take(numberOfScores).ToListAsync(cancellationToken);

                reportData.ForEach(f => response.Scores.Add(new PlayerScoreResponse
                                                            {
                                                                TournamentDate = f.TournamentDate,
                                                                GolfClubId = f.GolfClubId,
                                                                MeasuredCourseId = f.MeasuredCourseId,
                                                                PlayerId = f.PlayerId,
                                                                TournamentId = f.TournamentId,
                                                                NetScore = f.NetScore,
                                                                MeasuredCourseName = f.MeasuredCourseName,
                                                                GolfClubName = f.GolfClubName,
                                                                MeasuredCourseTeeColour = f.MeasuredCourseTeeColour,
                                                                TournamentFormat = (TournamentFormat)f.TournamentFormat,
                                                                GrossScore = f.GrossScore,
                                                                CSS = f.CSS,
                                                                TournamentName = f.TournamentName
                                                            }));
            }

            return response;
        }

        /// <summary>
        /// Gets the number of members by age category report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetNumberOfMembersByAgeCategoryReportResponse> GetNumberOfMembersByAgeCategoryReport(Guid golfClubId,
                                                                CancellationToken cancellationToken)
        {
            // TODO: get the configured age categories from the golf club record
            Guard.ThrowIfInvalidGuid(golfClubId, nameof(golfClubId));

            GetNumberOfMembersByAgeCategoryReportResponse response = new GetNumberOfMembersByAgeCategoryReportResponse();

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Get the members grouped by age
                IQueryable<GolfClubMembershipReporting> members = context.GolfClubMembershipReporting.Where(g => g.GolfClubId == golfClubId).AsQueryable();
                DateTime zeroTime = new DateTime(1, 1, 1);

                // Group the members records by age
                var membersByAge = members.GroupBy(g => (zeroTime + DateTime.Now.Date.Subtract(g.DateOfBirth)).Year).Select(x => new
                                                                                                                                     {
                                                                                                                                         Age = x.Key,
                                                                                                                                         Count = x.Count()
                                                                                                                                     });
                // Now build the members by age category to go into the response
                var membersByAgeCategory = membersByAge.Select(x => new
                                                                    {
                                                                        AgeCategory = (x.Age < 17 ? "Junior" :
                                                                            x.Age >= 17 && x.Age < 19 ? "Juvenile" :
                                                                            x.Age >= 19 && x.Age < 21 ? "Youth" :
                                                                            x.Age >= 21 && x.Age < 25 ? "Young Adult" :
                                                                            x.Age >= 25 && x.Age < 65 ? "Adult" :
                                                                            x.Age >= 65 ? "Senior" : "Unknown"),
                                                                        Count = x.Count
                                                                    });

                response.GolfClubId = golfClubId;
                response.MembersByAgeCategoryResponse = membersByAgeCategory.GroupBy(g => g.AgeCategory).Select(x => new MembersByAgeCategoryResponse
                                                                                                                     {
                                                                                                                         AgeCategory = x.Key,
                                                                                                                         NumberOfMembers = x.Sum(s => s.Count)
                                                                                                                     }).ToList();
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