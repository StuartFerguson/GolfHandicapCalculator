using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Service.Tests.General
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Manager;
    using Database;
    using Database.Models;
    using DataTransferObjects.Responses;
    using GolfClub;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Player;
    using Shouldly;
    using Tournament;
    using Xunit;
    using TimePeriod = DataTransferObjects.Responses.TimePeriod;

    public class ReportingManagerTests
    {
        [Fact]
        public async Task ReportingManager_GetPlayerScores_ReportDataReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            ManagementAPIReadModel context = this.GetContext(databaseName);

            List<PublishedPlayerScore> reportingData = new List<PublishedPlayerScore>();

            reportingData.Add(new PublishedPlayerScore
            {
                TournamentDate = TournamentTestData.TournamentDate,
                PlayerId = PlayerTestData.AggregateId,
                TournamentFormat = TournamentTestData.TournamentFormat,
                GolfClubId = TournamentTestData.GolfClubId,
                MeasuredCourseId = TournamentTestData.MeasuredCourseId,
                TournamentId = TournamentTestData.AggregateId,
                CSS = TournamentTestData.CSS,
                NetScore = TournamentTestData.NetScore,
                MeasuredCourseName = GolfClubTestData.MeasuredCourseName,
                GolfClubName = GolfClubTestData.Name,
                MeasuredCourseTeeColour = GolfClubTestData.TeeColour,
                GrossScore = TournamentTestData.GrossScore,
                TournamentName = TournamentTestData.Name
            });

            await context.PublishedPlayerScores.AddRangeAsync(reportingData, CancellationToken.None);
            await context.SaveChangesAsync(CancellationToken.None);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            ReportingManager reportingManager = new ReportingManager(contextResolver);

            GetPlayerScoresResponse reportData = await reportingManager.GetPlayerScoresReport(PlayerTestData.AggregateId, 10, CancellationToken.None);

            reportData.Scores.Count.ShouldBe(reportingData.Count);
        }

        [Fact]
        public async Task ReportingManager_GetPlayerScores_NoMembers_ReportDataReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            ManagementAPIReadModel context = this.GetContext(databaseName);

            List<PublishedPlayerScore> reportingData = new List<PublishedPlayerScore>();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            ReportingManager reportingManager = new ReportingManager(contextResolver);

            GetPlayerScoresResponse reportData = await reportingManager.GetPlayerScoresReport(PlayerTestData.AggregateId, 10, CancellationToken.None);

            reportData.Scores.Count.ShouldBe(reportingData.Count);
        }

        [Fact]
        public async Task ReportingManager_GetNumberOfMembersReport_ReportDataReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            ManagementAPIReadModel context = this.GetContext(databaseName);

            List<GolfClubMembershipReporting> reportingData = new List<GolfClubMembershipReporting>();

            reportingData.Add(new GolfClubMembershipReporting
                              {
                                  DateJoined = DateTime.Now,
                                  DateOfBirth = DateTime.Now.AddYears(-20),
                                  PlayerId = Guid.NewGuid(),
                                  GolfClubId = GolfClubTestData.AggregateId,
                                  GolfClubName = GolfClubTestData.Name,
                                  HandicapCategory = 1,
                                  PlayerGender = "M",
                                  PlayerName = "Test Player 1"
            });

            reportingData.Add(new GolfClubMembershipReporting
                              {
                                  DateJoined = DateTime.Now,
                                  DateOfBirth = DateTime.Now.AddYears(-25),
                                  PlayerId = Guid.NewGuid(),
                                  GolfClubId = GolfClubTestData.AggregateId,
                                  GolfClubName = GolfClubTestData.Name,
                                  HandicapCategory = 2,
                                  PlayerGender = "M",
                                  PlayerName = "Test Player 2"
                              });
            await context.GolfClubMembershipReporting.AddRangeAsync(reportingData, CancellationToken.None);
            await context.SaveChangesAsync(CancellationToken.None);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            ReportingManager reportingManager = new ReportingManager(contextResolver);

            GetNumberOfMembersReportResponse reportData = await reportingManager.GetNumberOfMembersReport(GolfClubTestData.AggregateId, CancellationToken.None);

            reportData.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            reportData.NumberOfMembers.ShouldBe(reportingData.Count);
        }

        [Fact]
        public async Task ReportingManager_GetNumberOfMembersReport_NoMembers_ReportDataReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            ManagementAPIReadModel context = this.GetContext(databaseName);

            List<GolfClubMembershipReporting> reportingData = new List<GolfClubMembershipReporting>();
            
            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            ReportingManager reportingManager = new ReportingManager(contextResolver);

            GetNumberOfMembersReportResponse reportData = await reportingManager.GetNumberOfMembersReport(GolfClubTestData.AggregateId, CancellationToken.None);

            reportData.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            reportData.NumberOfMembers.ShouldBe(reportingData.Count);
        }

        [Fact]
        public async Task ReportingManager_GetMembersHandicapListReport_ReportDataReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            ManagementAPIReadModel context = this.GetContext(databaseName);

            List<PlayerHandicapListReporting> reportingData = new List<PlayerHandicapListReporting>();

            reportingData.Add(new PlayerHandicapListReporting
            {
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                HandicapCategory = 1,
                PlayerName = "Test Player 1",
                ExactHandicap = 5.4m,
                PlayingHandicap = 5
            });

            reportingData.Add(new PlayerHandicapListReporting
            {
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                HandicapCategory = 2,
                PlayerName = "Test Player 2",
                ExactHandicap = 12.8m,
                PlayingHandicap = 13
            });
            await context.PlayerHandicapListReporting.AddRangeAsync(reportingData, CancellationToken.None);
            await context.SaveChangesAsync(CancellationToken.None);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            ReportingManager reportingManager = new ReportingManager(contextResolver);

            GetMembersHandicapListReportResponse reportData = await reportingManager.GetMembersHandicapListReport(GolfClubTestData.AggregateId, CancellationToken.None);

            reportData.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            reportData.MembersHandicapListReportResponse.ShouldNotBeEmpty();
            reportData.MembersHandicapListReportResponse.Count.ShouldBe(reportingData.Count);
        }

        [Fact]
        public async Task ReportingManager_GetMembersHandicapListReport_NoMembers_ReportDataReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            ManagementAPIReadModel context = this.GetContext(databaseName);

            List<PlayerHandicapListReporting> reportingData = new List<PlayerHandicapListReporting>();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            ReportingManager reportingManager = new ReportingManager(contextResolver);

            GetMembersHandicapListReportResponse reportData = await reportingManager.GetMembersHandicapListReport(GolfClubTestData.AggregateId, CancellationToken.None);

            reportData.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            reportData.MembersHandicapListReportResponse.Count.ShouldBe(reportingData.Count);
        }

        [Fact]
        public async Task ReportingManager_GetNumberOfMembersByHandicapCategoryReport_ReportDataReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            ManagementAPIReadModel context = this.GetContext(databaseName);

            List<GolfClubMembershipReporting> reportingData = new List<GolfClubMembershipReporting>();

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = DateTime.Now,
                DateOfBirth = DateTime.Now.AddYears(-20),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 1,
                PlayerGender = "M",
                PlayerName = "Test Player 1"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = DateTime.Now,
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 2,
                PlayerGender = "M",
                PlayerName = "Test Player 2"
            });

            reportingData.Add(new GolfClubMembershipReporting
                              {
                                  DateJoined = DateTime.Now,
                                  DateOfBirth = DateTime.Now.AddYears(-25),
                                  PlayerId = Guid.NewGuid(),
                                  GolfClubId = GolfClubTestData.AggregateId,
                                  GolfClubName = GolfClubTestData.Name,
                                  HandicapCategory = 2,
                                  PlayerGender = "M",
                                  PlayerName = "Test Player 3"
                              });

            reportingData.Add(new GolfClubMembershipReporting
                              {
                                  DateJoined = DateTime.Now,
                                  DateOfBirth = DateTime.Now.AddYears(-25),
                                  PlayerId = Guid.NewGuid(),
                                  GolfClubId = GolfClubTestData.AggregateId,
                                  GolfClubName = GolfClubTestData.Name,
                                  HandicapCategory = 3,
                                  PlayerGender = "M",
                                  PlayerName = "Test Player 4"
                              });

            reportingData.Add(new GolfClubMembershipReporting
                              {
                                  DateJoined = DateTime.Now,
                                  DateOfBirth = DateTime.Now.AddYears(-25),
                                  PlayerId = Guid.NewGuid(),
                                  GolfClubId = GolfClubTestData.AggregateId,
                                  GolfClubName = GolfClubTestData.Name,
                                  HandicapCategory = 3,
                                  PlayerGender = "M",
                                  PlayerName = "Test Player 5"
                              });

            reportingData.Add(new GolfClubMembershipReporting
                              {
                                  DateJoined = DateTime.Now,
                                  DateOfBirth = DateTime.Now.AddYears(-25),
                                  PlayerId = Guid.NewGuid(),
                                  GolfClubId = GolfClubTestData.AggregateId,
                                  GolfClubName = GolfClubTestData.Name,
                                  HandicapCategory = 3,
                                  PlayerGender = "M",
                                  PlayerName = "Test Player 6"
                              });

            await context.GolfClubMembershipReporting.AddRangeAsync(reportingData, CancellationToken.None);
            await context.SaveChangesAsync(CancellationToken.None);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            ReportingManager reportingManager = new ReportingManager(contextResolver);

            GetNumberOfMembersByHandicapCategoryReportResponse reportData = await reportingManager.GetNumberOfMembersByHandicapCategoryReport(GolfClubTestData.AggregateId, CancellationToken.None);

            reportData.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            reportData.MembersByHandicapCategoryResponse.ShouldNotBeEmpty();
            reportData.MembersByHandicapCategoryResponse.Where(q => q.HandicapCategory == 1).Single().NumberOfMembers.ShouldBe(1);
            reportData.MembersByHandicapCategoryResponse.Where(q => q.HandicapCategory == 2).Single().NumberOfMembers.ShouldBe(2);
            reportData.MembersByHandicapCategoryResponse.Where(q => q.HandicapCategory == 3).Single().NumberOfMembers.ShouldBe(3);
        }

        [Fact]
        public async Task ReportingManager_GetNumberOfMembersByHandicapCategoryReport_NoMembers_ReportDataReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            ManagementAPIReadModel context = this.GetContext(databaseName);
            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            ReportingManager reportingManager = new ReportingManager(contextResolver);

            GetNumberOfMembersByHandicapCategoryReportResponse reportData = await reportingManager.GetNumberOfMembersByHandicapCategoryReport(GolfClubTestData.AggregateId, CancellationToken.None);

            reportData.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            reportData.MembersByHandicapCategoryResponse.ShouldBeEmpty();
        }


        [Fact]
        public async Task ReportingManager_GetNumberOfMembersByTimePeriodReport_ByDay_ReportDataReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            ManagementAPIReadModel context = this.GetContext(databaseName);
            List<GolfClubMembershipReporting> reportingData = new List<GolfClubMembershipReporting>();

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2019,08,16),
                DateOfBirth = DateTime.Now.AddYears(-20),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 1,
                PlayerGender = "M",
                PlayerName = "Test Player 1"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2019, 08, 16),
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 2,
                PlayerGender = "M",
                PlayerName = "Test Player 2"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2019, 08, 16),
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 2,
                PlayerGender = "M",
                PlayerName = "Test Player 3"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2019, 08, 15),
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 3,
                PlayerGender = "M",
                PlayerName = "Test Player 4"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2019, 08, 14),
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 3,
                PlayerGender = "M",
                PlayerName = "Test Player 5"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2019, 08, 13),
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 3,
                PlayerGender = "M",
                PlayerName = "Test Player 6"
            });
            context.GolfClubMembershipReporting.AddRange(reportingData);
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            ReportingManager reportingManager = new ReportingManager(contextResolver);

            GetNumberOfMembersByTimePeriodReportResponse reportData = 
                await reportingManager.GetNumberOfMembersByTimePeriodReport(GolfClubTestData.AggregateId, TimePeriod.Day.ToString(), 
                                                                            CancellationToken.None);

            reportData.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            reportData.TimePeriod.ShouldBe(TimePeriod.Day);
            reportData.MembersByTimePeriodResponse.ShouldNotBeNull();
            reportData.MembersByTimePeriodResponse.ShouldNotBeEmpty();
            reportData.MembersByTimePeriodResponse.Count.ShouldBe(4);
            reportData.MembersByTimePeriodResponse.SingleOrDefault(x => x.Period == "2019-08-13").ShouldNotBeNull();
            reportData.MembersByTimePeriodResponse.Single(x => x.Period == "2019-08-13").NumberOfMembers.ShouldBe(1);
            reportData.MembersByTimePeriodResponse.SingleOrDefault(x => x.Period == "2019-08-14").ShouldNotBeNull();
            reportData.MembersByTimePeriodResponse.Single(x => x.Period == "2019-08-14").NumberOfMembers.ShouldBe(2);
            reportData.MembersByTimePeriodResponse.SingleOrDefault(x => x.Period == "2019-08-15").ShouldNotBeNull();
            reportData.MembersByTimePeriodResponse.Single(x => x.Period == "2019-08-15").NumberOfMembers.ShouldBe(3);
            reportData.MembersByTimePeriodResponse.SingleOrDefault(x => x.Period == "2019-08-16").ShouldNotBeNull();
            reportData.MembersByTimePeriodResponse.Single(x => x.Period == "2019-08-16").NumberOfMembers.ShouldBe(6);
        }

        [Fact]
        public async Task ReportingManager_GetNumberOfMembersByTimePeriodReport_ByMonth_ReportDataReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            ManagementAPIReadModel context = this.GetContext(databaseName);
            List<GolfClubMembershipReporting> reportingData = new List<GolfClubMembershipReporting>();

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2019, 08, 16),
                DateOfBirth = DateTime.Now.AddYears(-20),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 1,
                PlayerGender = "M",
                PlayerName = "Test Player 1"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2019, 08, 16),
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 2,
                PlayerGender = "M",
                PlayerName = "Test Player 2"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2019, 08, 16),
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 2,
                PlayerGender = "M",
                PlayerName = "Test Player 3"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2019, 07, 15),
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 3,
                PlayerGender = "M",
                PlayerName = "Test Player 4"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2019, 07, 14),
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 3,
                PlayerGender = "M",
                PlayerName = "Test Player 5"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2019, 06, 13),
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 3,
                PlayerGender = "M",
                PlayerName = "Test Player 6"
            });
            context.GolfClubMembershipReporting.AddRange(reportingData);
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            ReportingManager reportingManager = new ReportingManager(contextResolver);

            GetNumberOfMembersByTimePeriodReportResponse reportData = await reportingManager.GetNumberOfMembersByTimePeriodReport(GolfClubTestData.AggregateId, TimePeriod.Month.ToString(), CancellationToken.None);

            reportData.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            reportData.TimePeriod.ShouldBe(TimePeriod.Month);
            reportData.MembersByTimePeriodResponse.ShouldNotBeNull();
            reportData.MembersByTimePeriodResponse.ShouldNotBeEmpty();
            reportData.MembersByTimePeriodResponse.Count.ShouldBe(3);
            reportData.MembersByTimePeriodResponse.SingleOrDefault(x => x.Period == "2019-06").ShouldNotBeNull();
            reportData.MembersByTimePeriodResponse.Single(x => x.Period == "2019-06").NumberOfMembers.ShouldBe(1);
            reportData.MembersByTimePeriodResponse.SingleOrDefault(x => x.Period == "2019-07").ShouldNotBeNull();
            reportData.MembersByTimePeriodResponse.Single(x => x.Period == "2019-07").NumberOfMembers.ShouldBe(3);
            reportData.MembersByTimePeriodResponse.SingleOrDefault(x => x.Period == "2019-08").ShouldNotBeNull();
            reportData.MembersByTimePeriodResponse.Single(x => x.Period == "2019-08").NumberOfMembers.ShouldBe(6);
        }

        [Fact]
        public async Task ReportingManager_GetNumberOfMembersByTimePeriodReport_ByYear_ReportDataReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            ManagementAPIReadModel context = this.GetContext(databaseName);
            List<GolfClubMembershipReporting> reportingData = new List<GolfClubMembershipReporting>();

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2019, 08, 16),
                DateOfBirth = DateTime.Now.AddYears(-20),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 1,
                PlayerGender = "M",
                PlayerName = "Test Player 1"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2019, 08, 16),
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 2,
                PlayerGender = "M",
                PlayerName = "Test Player 2"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2018, 08, 16),
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 2,
                PlayerGender = "M",
                PlayerName = "Test Player 3"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2018, 07, 15),
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 3,
                PlayerGender = "M",
                PlayerName = "Test Player 4"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2018, 07, 14),
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 3,
                PlayerGender = "M",
                PlayerName = "Test Player 5"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = new DateTime(2017, 06, 13),
                DateOfBirth = DateTime.Now.AddYears(-25),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 3,
                PlayerGender = "M",
                PlayerName = "Test Player 6"
            });
            context.GolfClubMembershipReporting.AddRange(reportingData);
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            ReportingManager reportingManager = new ReportingManager(contextResolver);

            GetNumberOfMembersByTimePeriodReportResponse reportData = await reportingManager.GetNumberOfMembersByTimePeriodReport(GolfClubTestData.AggregateId, TimePeriod.Year.ToString(), CancellationToken.None);

            reportData.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            reportData.TimePeriod.ShouldBe(TimePeriod.Year);
            reportData.MembersByTimePeriodResponse.ShouldNotBeNull();
            reportData.MembersByTimePeriodResponse.ShouldNotBeEmpty();
            reportData.MembersByTimePeriodResponse.Count.ShouldBe(3);
            reportData.MembersByTimePeriodResponse[0].Period.ShouldBe("2017");
            reportData.MembersByTimePeriodResponse[0].NumberOfMembers.ShouldBe(1);
            reportData.MembersByTimePeriodResponse[1].Period.ShouldBe("2018");
            reportData.MembersByTimePeriodResponse[1].NumberOfMembers.ShouldBe(4);
            reportData.MembersByTimePeriodResponse[2].Period.ShouldBe("2019");
            reportData.MembersByTimePeriodResponse[2].NumberOfMembers.ShouldBe(6);
        }

        [Theory]
        [InlineData("day", TimePeriod.Day)]
        //[InlineData("week", TimePeriod.Week)]
        [InlineData("month", TimePeriod.Month)]
        [InlineData("year", TimePeriod.Year)]
        public async Task ReportingManager_GetNumberOfMembersByTimePeriodReport_NoMembers_ReportDataReturned(String timePeriod, TimePeriod expectedTimePeriod)
        {
            String databaseName = Guid.NewGuid().ToString("N");
            ManagementAPIReadModel context = this.GetContext(databaseName);

            List<GolfClubMembershipReporting> reportingData = new List<GolfClubMembershipReporting>();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            ReportingManager reportingManager = new ReportingManager(contextResolver);

            GetNumberOfMembersByTimePeriodReportResponse reportData = await reportingManager.GetNumberOfMembersByTimePeriodReport(GolfClubTestData.AggregateId, timePeriod, CancellationToken.None);

            reportData.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            reportData.TimePeriod.ShouldBe(expectedTimePeriod);
            reportData.MembersByTimePeriodResponse.ShouldBeEmpty();
        }
        
        [Fact]
        public async Task ReportingManager_GetNumberOfMembersByAgeCategoryReport_ReportDataReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            ManagementAPIReadModel context = this.GetContext(databaseName);

            List<GolfClubMembershipReporting> reportingData = new List<GolfClubMembershipReporting>();

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = DateTime.Now,
                DateOfBirth = DateTime.Now.AddYears(-16),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 1,
                PlayerGender = "M",
                PlayerName = "Test Player 1"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = DateTime.Now,
                DateOfBirth = DateTime.Now.AddYears(-18),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 2,
                PlayerGender = "M",
                PlayerName = "Test Player 2"
            });

            reportingData.Add(new GolfClubMembershipReporting
                              {
                                  DateJoined = DateTime.Now,
                                  DateOfBirth = DateTime.Now.AddYears(-18),
                                  PlayerId = Guid.NewGuid(),
                                  GolfClubId = GolfClubTestData.AggregateId,
                                  GolfClubName = GolfClubTestData.Name,
                                  HandicapCategory = 2,
                                  PlayerGender = "M",
                                  PlayerName = "Test Player 3"
                              });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = DateTime.Now,
                DateOfBirth = DateTime.Now.AddYears(-19),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 2,
                PlayerGender = "M",
                PlayerName = "Test Player 4"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = DateTime.Now,
                DateOfBirth = DateTime.Now.AddYears(-20),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 3,
                PlayerGender = "M",
                PlayerName = "Test Player 5"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = DateTime.Now,
                DateOfBirth = DateTime.Now.AddYears(-22),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 3,
                PlayerGender = "M",
                PlayerName = "Test Player 6"
            });

            reportingData.Add(new GolfClubMembershipReporting
            {
                DateJoined = DateTime.Now,
                DateOfBirth = DateTime.Now.AddYears(-24),
                PlayerId = Guid.NewGuid(),
                GolfClubId = GolfClubTestData.AggregateId,
                GolfClubName = GolfClubTestData.Name,
                HandicapCategory = 3,
                PlayerGender = "M",
                PlayerName = "Test Player 7"
            });

            reportingData.Add(new GolfClubMembershipReporting
                              {
                                  DateJoined = DateTime.Now,
                                  DateOfBirth = DateTime.Now.AddYears(-26),
                                  PlayerId = Guid.NewGuid(),
                                  GolfClubId = GolfClubTestData.AggregateId,
                                  GolfClubName = GolfClubTestData.Name,
                                  HandicapCategory = 3,
                                  PlayerGender = "M",
                                  PlayerName = "Test Player 8"
                              });

            reportingData.Add(new GolfClubMembershipReporting
                              {
                                  DateJoined = DateTime.Now,
                                  DateOfBirth = DateTime.Now.AddYears(-35),
                                  PlayerId = Guid.NewGuid(),
                                  GolfClubId = GolfClubTestData.AggregateId,
                                  GolfClubName = GolfClubTestData.Name,
                                  HandicapCategory = 3,
                                  PlayerGender = "M",
                                  PlayerName = "Test Player 9"
                              });

            reportingData.Add(new GolfClubMembershipReporting
                              {
                                  DateJoined = DateTime.Now,
                                  DateOfBirth = DateTime.Now.AddYears(-64),
                                  PlayerId = Guid.NewGuid(),
                                  GolfClubId = GolfClubTestData.AggregateId,
                                  GolfClubName = GolfClubTestData.Name,
                                  HandicapCategory = 3,
                                  PlayerGender = "M",
                                  PlayerName = "Test Player 10"
                              });

            reportingData.Add(new GolfClubMembershipReporting
                              {
                                  DateJoined = DateTime.Now,
                                  DateOfBirth = DateTime.Now.AddYears(-65),
                                  PlayerId = Guid.NewGuid(),
                                  GolfClubId = GolfClubTestData.AggregateId,
                                  GolfClubName = GolfClubTestData.Name,
                                  HandicapCategory = 3,
                                  PlayerGender = "M",
                                  PlayerName = "Test Player 11"
                              });

            reportingData.Add(new GolfClubMembershipReporting
                              {
                                  DateJoined = DateTime.Now,
                                  DateOfBirth = DateTime.Now.AddYears(-70),
                                  PlayerId = Guid.NewGuid(),
                                  GolfClubId = GolfClubTestData.AggregateId,
                                  GolfClubName = GolfClubTestData.Name,
                                  HandicapCategory = 3,
                                  PlayerGender = "M",
                                  PlayerName = "Test Player 12"
                              });

            await context.GolfClubMembershipReporting.AddRangeAsync(reportingData, CancellationToken.None);
            await context.SaveChangesAsync(CancellationToken.None);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            ReportingManager reportingManager = new ReportingManager(contextResolver);

            GetNumberOfMembersByAgeCategoryReportResponse reportData = await reportingManager.GetNumberOfMembersByAgeCategoryReport(GolfClubTestData.AggregateId, CancellationToken.None);

            reportData.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            reportData.MembersByAgeCategoryResponse.ShouldNotBeEmpty();
            reportData.MembersByAgeCategoryResponse.Single(q => q.AgeCategory == "Junior").NumberOfMembers.ShouldBe(1);
            reportData.MembersByAgeCategoryResponse.Single(q => q.AgeCategory == "Juvenile").NumberOfMembers.ShouldBe(2);
            reportData.MembersByAgeCategoryResponse.Single(q => q.AgeCategory == "Youth").NumberOfMembers.ShouldBe(2);
            reportData.MembersByAgeCategoryResponse.Single(q => q.AgeCategory == "Young Adult").NumberOfMembers.ShouldBe(2);
            reportData.MembersByAgeCategoryResponse.Single(q => q.AgeCategory == "Adult").NumberOfMembers.ShouldBe(3);
            reportData.MembersByAgeCategoryResponse.Single(q => q.AgeCategory == "Senior").NumberOfMembers.ShouldBe(2);
        }

        [Fact]
        public async Task ReportingManager_GetNumberOfMembersByAgeCategoryReport_NoMembers_ReportDataReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            ManagementAPIReadModel context = this.GetContext(databaseName);
            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            ReportingManager reportingManager = new ReportingManager(contextResolver);

            GetNumberOfMembersByAgeCategoryReportResponse reportData = await reportingManager.GetNumberOfMembersByAgeCategoryReport(GolfClubTestData.AggregateId, CancellationToken.None);

            reportData.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            reportData.MembersByAgeCategoryResponse.ShouldBeEmpty();
        }


        private ManagementAPIReadModel GetContext(String databaseName)
        {
            DbContextOptionsBuilder<ManagementAPIReadModel> builder = new DbContextOptionsBuilder<ManagementAPIReadModel>()
                                                                      .UseInMemoryDatabase(databaseName)
                                                                      .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            ManagementAPIReadModel context = new ManagementAPIReadModel(builder.Options);

            return context;
        }
    }
}
