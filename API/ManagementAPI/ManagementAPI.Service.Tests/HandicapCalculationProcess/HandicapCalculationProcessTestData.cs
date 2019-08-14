namespace ManagementAPI.Service.Tests.HandicapCalculationProcess
{
    using System;
    using ManagementAPI.BusinessLogic.Commands;
    using ManagementAPI.HandicapCalculationProcess;
    using Tournament;

    public class HandicapCalculationProcessTestData
    {
        public static Guid AggregateId = Guid.Parse("F8181814-32E5-4318-A7DE-8ECFC130980B");

        public static DateTime StartedDateTime = new DateTime(2019,1,3);
        public static DateTime RunningDateTime = new DateTime(2019, 1, 3, 10,59,0);
        public static DateTime CompletedDateTime = new DateTime(2019, 1, 3, 11, 59, 0);
        public static DateTime ErroredDateTime = new DateTime(2019, 1, 3, 11, 15, 0);
        public static String ErrorMessage = "Error message";

        public static Guid TournamentId  = Guid.Parse("287422D4-0388-4455-B2B5-FFF549050784");

        public static HandicapCalculationProcessAggregate GetEmptyHandicapCalculationProcessAggregate()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessAggregate.Create(HandicapCalculationProcessTestData.AggregateId);

            return handicapCalculationProcessAggregate;
        }

        public static HandicapCalculationProcessAggregate GetStartedHandicapCalculationProcessAggregate()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessAggregate.Create(HandicapCalculationProcessTestData.AggregateId);

            handicapCalculationProcessAggregate.StartHandicapCalculationProcess(TournamentTestData.GetCompletedTournamentAggregateWithCSSCalculatedAggregate(), HandicapCalculationProcessTestData.StartedDateTime);

            return handicapCalculationProcessAggregate;
        }

        public static HandicapCalculationProcessAggregate GetRunningHandicapCalculationProcessAggregate()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessAggregate.Create(HandicapCalculationProcessTestData.AggregateId);

            handicapCalculationProcessAggregate.StartHandicapCalculationProcess(TournamentTestData.GetCompletedTournamentAggregateWithCSSCalculatedAggregate(), HandicapCalculationProcessTestData.StartedDateTime);
            handicapCalculationProcessAggregate.UpdateProcessToRunning(HandicapCalculationProcessTestData.RunningDateTime);
            return handicapCalculationProcessAggregate;
        }

        public static HandicapCalculationProcessAggregate GetCompletedHandicapCalculationProcessAggregate()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessAggregate.Create(HandicapCalculationProcessTestData.AggregateId);

            handicapCalculationProcessAggregate.StartHandicapCalculationProcess(TournamentTestData.GetCompletedTournamentAggregateWithCSSCalculatedAggregate(), HandicapCalculationProcessTestData.StartedDateTime);
            handicapCalculationProcessAggregate.UpdateProcessToRunning(HandicapCalculationProcessTestData.RunningDateTime);
            handicapCalculationProcessAggregate.UpdateProcessToComplete(HandicapCalculationProcessTestData.CompletedDateTime);
            return handicapCalculationProcessAggregate;
        }

        public static HandicapCalculationProcessAggregate GetErroredHandicapCalculationProcessAggregate()
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = HandicapCalculationProcessAggregate.Create(HandicapCalculationProcessTestData.AggregateId);

            handicapCalculationProcessAggregate.StartHandicapCalculationProcess(TournamentTestData.GetCompletedTournamentAggregateWithCSSCalculatedAggregate(), HandicapCalculationProcessTestData.StartedDateTime);
            handicapCalculationProcessAggregate.UpdateProcessToRunning(HandicapCalculationProcessTestData.RunningDateTime);
            handicapCalculationProcessAggregate.UpdateProcessToErrored(HandicapCalculationProcessTestData.ErroredDateTime, HandicapCalculationProcessTestData.ErrorMessage);
            return handicapCalculationProcessAggregate;
        }

        public static StartHandicapCalculationProcessForTournamentCommand GetStartHandicapCalculationProcessForTournamentCommand()
        {
            return StartHandicapCalculationProcessForTournamentCommand.Create(HandicapCalculationProcessTestData.TournamentId);
        }
    }
}