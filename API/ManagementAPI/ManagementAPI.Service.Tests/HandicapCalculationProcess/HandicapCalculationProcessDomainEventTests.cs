using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Service.Tests.HandicapCalculationProcess
{
    using ManagementAPI.HandicapCalculationProcess.DomainEvents;
    using Shouldly;
    using Xunit;

    public class HandicapCalculationProcessDomainEventTests
    {
        [Fact]
        public void HandicapCalculationProcessStartedEvent_CanBeCreated_IsCreated()
        {
            HandicapCalculationProcessStartedEvent handicapCalculationProcessStartedEvent =
                HandicapCalculationProcessStartedEvent.Create(HandicapCalculationProcessTestData.AggregateId, HandicapCalculationProcessTestData.StartedDateTime);

            handicapCalculationProcessStartedEvent.ShouldNotBeNull();
            handicapCalculationProcessStartedEvent.EventId.ShouldNotBe(Guid.Empty);
            handicapCalculationProcessStartedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            handicapCalculationProcessStartedEvent.AggregateId.ShouldBe(HandicapCalculationProcessTestData.AggregateId);
            handicapCalculationProcessStartedEvent.StartedDateTime.ShouldBe(HandicapCalculationProcessTestData.StartedDateTime);
        }

        [Fact]
        public void HandicapCalculationProcessChangedToRunningEvent_CanBeCreated_IsCreated()
        {
            HandicapCalculationProcessChangedToRunningEvent handicapCalculationProcessChangedToRunningEvent =
                HandicapCalculationProcessChangedToRunningEvent.Create(HandicapCalculationProcessTestData.AggregateId, HandicapCalculationProcessTestData.RunningDateTime);

            handicapCalculationProcessChangedToRunningEvent.ShouldNotBeNull();
            handicapCalculationProcessChangedToRunningEvent.EventId.ShouldNotBe(Guid.Empty);
            handicapCalculationProcessChangedToRunningEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            handicapCalculationProcessChangedToRunningEvent.AggregateId.ShouldBe(HandicapCalculationProcessTestData.AggregateId);
            handicapCalculationProcessChangedToRunningEvent.RunningDateTime.ShouldBe(HandicapCalculationProcessTestData.RunningDateTime);
        }

        [Fact]
        public void HandicapCalculationProcessChangedToCompletedEvent_CanBeCreated_IsCreated()
        {
            HandicapCalculationProcessChangedToCompletedEvent handicapCalculationProcessChangedToCompletedEvent =
                HandicapCalculationProcessChangedToCompletedEvent.Create(HandicapCalculationProcessTestData.AggregateId, HandicapCalculationProcessTestData.CompletedDateTime);

            handicapCalculationProcessChangedToCompletedEvent.ShouldNotBeNull();
            handicapCalculationProcessChangedToCompletedEvent.EventId.ShouldNotBe(Guid.Empty);
            handicapCalculationProcessChangedToCompletedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            handicapCalculationProcessChangedToCompletedEvent.AggregateId.ShouldBe(HandicapCalculationProcessTestData.AggregateId);
            handicapCalculationProcessChangedToCompletedEvent.CompletedDateTime.ShouldBe(HandicapCalculationProcessTestData.CompletedDateTime);
        }

        [Fact]
        public void HandicapCalculationProcessChangedToErroredEvent_CanBeCreated_IsCreated()
        {
            HandicapCalculationProcessChangedToErroredEvent handicapCalculationProcessChangedToErroredEvent =
                HandicapCalculationProcessChangedToErroredEvent.Create(HandicapCalculationProcessTestData.AggregateId,
                                                                       HandicapCalculationProcessTestData.ErroredDateTime,
                                                                       HandicapCalculationProcessTestData.ErrorMessage);

            handicapCalculationProcessChangedToErroredEvent.ShouldNotBeNull();
            handicapCalculationProcessChangedToErroredEvent.EventId.ShouldNotBe(Guid.Empty);
            handicapCalculationProcessChangedToErroredEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            handicapCalculationProcessChangedToErroredEvent.AggregateId.ShouldBe(HandicapCalculationProcessTestData.AggregateId);
            handicapCalculationProcessChangedToErroredEvent.ErroredDateTime.ShouldBe(HandicapCalculationProcessTestData.ErroredDateTime);
            handicapCalculationProcessChangedToErroredEvent.ErrorMessage.ShouldBe(HandicapCalculationProcessTestData.ErrorMessage);
        }
    }
}
