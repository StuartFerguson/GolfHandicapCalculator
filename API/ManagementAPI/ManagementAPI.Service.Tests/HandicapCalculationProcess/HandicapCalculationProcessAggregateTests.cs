namespace ManagementAPI.Service.Tests.HandicapCalculationProcess
{
    using System;
    using ManagementAPI.HandicapCalculationProcess;
    using Shouldly;
    using Tournament;
    using Xunit;

    public class HandicapCalculationProcessAggregateTests
    {
        [Fact]
        public void HandicapCalculationProcessAggregate_StartHandicapCalculationProcess_ProcessRecordedAsStarted()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetEmptyHandicapCalculationProcessAggregate();

            handicapCalculationProcessAggregate.StartHandicapCalculationProcess(TournamentTestData.GetCompletedTournamentAggregateWithCSSCalculatedAggregate(), HandicapCalculationProcessTestData.StartedDateTime);

            handicapCalculationProcessAggregate.AggregateId.ShouldBe(handicapCalculationProcessAggregate.AggregateId);
            handicapCalculationProcessAggregate.StartedDateTime.ShouldBe(handicapCalculationProcessAggregate.StartedDateTime);
            handicapCalculationProcessAggregate.Status.ShouldBe(HandicapProcessStatus.Started);
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_StartHandicapCalculationProcess_AlreadyStarted_ErrorThrown()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetStartedHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.StartHandicapCalculationProcess(TournamentTestData
                                                                                                                                .GetCompletedTournamentAggregateWithCSSCalculatedAggregate(),
                                                                                                                            HandicapCalculationProcessTestData
                                                                                                                                .StartedDateTime);
                                                    });
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_StartHandicapCalculationProcess_AlreadyRunning_ErrorThrown()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetRunningHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.StartHandicapCalculationProcess(TournamentTestData
                                                                                                                                .GetCompletedTournamentAggregateWithCSSCalculatedAggregate(),
                                                                                                                            HandicapCalculationProcessTestData
                                                                                                                                .StartedDateTime);
                                                    });
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_StartHandicapCalculationProcess_AlreadyCompleted_ErrorThrown()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetCompletedHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.StartHandicapCalculationProcess(TournamentTestData
                                                                                                                                .GetCompletedTournamentAggregateWithCSSCalculatedAggregate(),
                                                                                                                            HandicapCalculationProcessTestData
                                                                                                                                .StartedDateTime);
                                                    });
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_StartHandicapCalculationProcess_AlreadyErrored_ErrorThrown()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetErroredHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.StartHandicapCalculationProcess(TournamentTestData
                                                                                                                                .GetCompletedTournamentAggregateWithCSSCalculatedAggregate(),
                                                                                                                            HandicapCalculationProcessTestData
                                                                                                                                .StartedDateTime);
                                                    });
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_StartHandicapCalculationProcess_TournamentNotCompleted()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetEmptyHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.StartHandicapCalculationProcess(TournamentTestData
                                                                                                                                .GetCreatedTournamentWithScoresRecordedAggregate(),
                                                                                                                            HandicapCalculationProcessTestData
                                                                                                                                .StartedDateTime);
                                                    });
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_StartHandicapCalculationProcess_TournamentScoresNotPublished()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetEmptyHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.StartHandicapCalculationProcess(TournamentTestData
                                                                                                                                .GetCompletedTournamentAggregate(),
                                                                                                                            HandicapCalculationProcessTestData
                                                                                                                                .StartedDateTime);
                                                    });
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_UpdateProcessToRunning_ProcessSetToRunning()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetStartedHandicapCalculationProcessAggregate();

            handicapCalculationProcessAggregate.UpdateProcessToRunning(HandicapCalculationProcessTestData.RunningDateTime);

            handicapCalculationProcessAggregate.AggregateId.ShouldBe(handicapCalculationProcessAggregate.AggregateId);
            handicapCalculationProcessAggregate.StartedDateTime.ShouldBe(handicapCalculationProcessAggregate.StartedDateTime);
            handicapCalculationProcessAggregate.RunningDateTime.ShouldBe(handicapCalculationProcessAggregate.RunningDateTime);
            handicapCalculationProcessAggregate.Status.ShouldBe(HandicapProcessStatus.Running);
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_UpdateProcessToRunning_NotStarted_ErrorThrown()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetEmptyHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.UpdateProcessToRunning(HandicapCalculationProcessTestData
                                                                                                                       .RunningDateTime);
                                                    });
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_UpdateProcessToRunning_AlreadyRunning_ErrorThrown()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetRunningHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.UpdateProcessToRunning(HandicapCalculationProcessTestData
                                                                                                                       .RunningDateTime);
                                                    });
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_UpdateProcessToRunning_AlreadyCompleted_ErrorThrown()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetCompletedHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.UpdateProcessToRunning(HandicapCalculationProcessTestData
                                                                                                                       .RunningDateTime);
                                                    });
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_UpdateProcessToRunning_AlreadyErrored_ErrorThrown()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetErroredHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.UpdateProcessToRunning(HandicapCalculationProcessTestData
                                                                                                                       .RunningDateTime);
                                                    });
        }
        
        [Fact]
        public void HandicapCalculationProcessAggregate_UpdateProcessToComplete_ProcessSetToCompleted()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetRunningHandicapCalculationProcessAggregate();

            handicapCalculationProcessAggregate.UpdateProcessToComplete(HandicapCalculationProcessTestData.CompletedDateTime);

            handicapCalculationProcessAggregate.AggregateId.ShouldBe(handicapCalculationProcessAggregate.AggregateId);
            handicapCalculationProcessAggregate.StartedDateTime.ShouldBe(handicapCalculationProcessAggregate.StartedDateTime);
            handicapCalculationProcessAggregate.RunningDateTime.ShouldBe(handicapCalculationProcessAggregate.RunningDateTime);
            handicapCalculationProcessAggregate.CompletedDateTime.ShouldBe(handicapCalculationProcessAggregate.CompletedDateTime);
            handicapCalculationProcessAggregate.Status.ShouldBe(HandicapProcessStatus.Completed);
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_UpdateProcessToComplete_NotStarted_ErrorThrown()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetEmptyHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.UpdateProcessToComplete(HandicapCalculationProcessTestData
                                                                                                                        .CompletedDateTime);
                                                    });
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_UpdateProcessToComplete_NotRunning_ErrorThrown()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetStartedHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.UpdateProcessToComplete(HandicapCalculationProcessTestData
                                                                                                                        .CompletedDateTime);
                                                    });
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_UpdateProcessToComplete_AlreadyCompleted_ErrorThrown()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetCompletedHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.UpdateProcessToComplete(HandicapCalculationProcessTestData
                                                                                                                        .CompletedDateTime);
                                                    });
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_UpdateProcessToComplete_AlreadyErrored_ErrorThrown()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetErroredHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.UpdateProcessToComplete(HandicapCalculationProcessTestData
                                                                                                                        .CompletedDateTime);
                                                    });
        }
        
        [Fact]
        public void HandicapCalculationProcessAggregate_UpdateProcessToErrored_ProcessSetToErrored()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetRunningHandicapCalculationProcessAggregate();

            handicapCalculationProcessAggregate.UpdateProcessToErrored(HandicapCalculationProcessTestData.ErroredDateTime, HandicapCalculationProcessTestData.ErrorMessage);

            handicapCalculationProcessAggregate.AggregateId.ShouldBe(handicapCalculationProcessAggregate.AggregateId);
            handicapCalculationProcessAggregate.StartedDateTime.ShouldBe(handicapCalculationProcessAggregate.StartedDateTime);
            handicapCalculationProcessAggregate.RunningDateTime.ShouldBe(handicapCalculationProcessAggregate.RunningDateTime);
            handicapCalculationProcessAggregate.ErroredDateTime.ShouldBe(handicapCalculationProcessAggregate.ErroredDateTime);
            handicapCalculationProcessAggregate.ErrorMessage.ShouldBe(handicapCalculationProcessAggregate.ErrorMessage);
            handicapCalculationProcessAggregate.Status.ShouldBe(HandicapProcessStatus.Errored);
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_UpdateProcessToErrored_NotStarted_ErrorThrown()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetEmptyHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.UpdateProcessToErrored(HandicapCalculationProcessTestData
                                                                                                                       .ErroredDateTime, HandicapCalculationProcessTestData.ErrorMessage);
                                                    });
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_UpdateProcessToErrored_NotRunning_ErrorThrown()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessTestData.GetStartedHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.UpdateProcessToErrored(HandicapCalculationProcessTestData.ErroredDateTime,
                                                                                                                   HandicapCalculationProcessTestData.ErrorMessage);
                                                    });
        }

        [Fact]
        public void HandicapCalculationProcessAggregate_UpdateProcessToErrored_AlreadyCompleted_ErrorThrown()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate =
                HandicapCalculationProcessTestData.GetCompletedHandicapCalculationProcessAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        handicapCalculationProcessAggregate.UpdateProcessToErrored(HandicapCalculationProcessTestData.ErroredDateTime,
                                                                                                                   HandicapCalculationProcessTestData.ErrorMessage);
                                                    });
        }
    }
}