namespace ManagementAPI.Service.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using Common;
    using HandicapCalculationProcess;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shared.CommandHandling;
    using Shared.EventStore;
    using Shared.Exceptions;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HandicapCalculationController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The command router
        /// </summary>
        private readonly ICommandRouter CommandRouter;

        private readonly IAggregateRepository<HandicapCalculationProcessAggregate> HandicapCalculationProcessRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapCalculationController" /> class.
        /// </summary>
        /// <param name="commandRouter">The command router.</param>
        /// <param name="handicapCalculationProcessRepository">The handicap calculation process repository.</param>
        public HandicapCalculationController(ICommandRouter commandRouter, IAggregateRepository<HandicapCalculationProcessAggregate> handicapCalculationProcessRepository)
        {
            this.CommandRouter = commandRouter;
            this.HandicapCalculationProcessRepository = handicapCalculationProcessRepository;
        }

        #endregion

        #region Methods

        [HttpPost]
        [Authorize(Policy = PolicyNames.ProcessHandicapCalculationsPolicy)]
        public async Task<IActionResult> ProcessHandicapCalculations([FromQuery] Guid tournamentId,
                                                                     CancellationToken cancellationToken)
        {
            StartHandicapCalculationProcessForTournamentCommand command = StartHandicapCalculationProcessForTournamentCommand.Create(tournamentId);

            await this.CommandRouter.Route(command, cancellationToken);

            return this.NoContent();
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.GetHandicapCalculationProcessStatusPolicy)]
        public async Task<IActionResult> GetHandicapCalculationProcessStatus([FromQuery] Guid tournamentId,
                                                                             CancellationToken cancellationToken)
        {
            HandicapCalculationProcessAggregate handicapCalculationProcess = await this.HandicapCalculationProcessRepository.GetLatestVersion(tournamentId, cancellationToken);

            if (handicapCalculationProcess.Status == HandicapProcessStatus.Default)
            {
                throw new NotFoundException($"Handicap Calculation Process not found with Id {tournamentId}");
            }

            return this.Ok(handicapCalculationProcess.Status.ToString());
        }

        #endregion
    }
}