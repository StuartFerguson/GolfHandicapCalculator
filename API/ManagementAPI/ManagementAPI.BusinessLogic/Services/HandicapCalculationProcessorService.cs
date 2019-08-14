namespace ManagementAPI.BusinessLogic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using HandicapCalculationProcess;
    using Player;
    using Shared.EventStore;
    using Shared.General;
    using Tournament;
    using Tournament.DataTransferObjects;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IHandicapCalculationProcessorService" />
    public class HandicapCalculationProcessorService : IHandicapCalculationProcessorService
    {
        /// <summary>
        /// Gets the handicap calculation process repository.
        /// </summary>
        /// <value>
        /// The handicap calculation process repository.
        /// </value>
        public IAggregateRepository<HandicapCalculationProcessAggregate> HandicapCalculationProcessRepository { get; private set; }

        /// <summary>
        /// Gets the tournament repository.
        /// </summary>
        /// <value>
        /// The tournament repository.
        /// </value>
        public IAggregateRepository<TournamentAggregate> TournamentRepository { get; private set; }

        /// <summary>
        /// Gets the handicap adjustment calculator service.
        /// </summary>
        /// <value>
        /// The handicap adjustment calculator service.
        /// </value>
        public IHandicapAdjustmentCalculatorService HandicapAdjustmentCalculatorService { get; private set; }

        /// <summary>
        /// Gets the player repository.
        /// </summary>
        /// <value>
        /// The player repository.
        /// </value>
        public IAggregateRepository<PlayerAggregate> PlayerRepository { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapCalculationProcessorService" /> class.
        /// </summary>
        /// <param name="handicapCalculationProcessRepository">The handicap calculation process repository.</param>
        /// <param name="tournamentRepository">The tournament repository.</param>
        /// <param name="handicapAdjustmentCalculatorService">The handicap adjustment calculator service.</param>
        /// <param name="playerRepository">The player repository.</param>
        public HandicapCalculationProcessorService(IAggregateRepository<HandicapCalculationProcessAggregate> handicapCalculationProcessRepository,
                                                   IAggregateRepository<TournamentAggregate> tournamentRepository,
                                                   IHandicapAdjustmentCalculatorService handicapAdjustmentCalculatorService,
                                                   IAggregateRepository<PlayerAggregate> playerRepository)
        {
            this.HandicapCalculationProcessRepository = handicapCalculationProcessRepository;
            this.TournamentRepository = tournamentRepository;
            this.HandicapAdjustmentCalculatorService = handicapAdjustmentCalculatorService;
            this.PlayerRepository = playerRepository;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <param name="processId">The process identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task Start(Guid processId,
                                Guid tournamentId,
                                DateTime startDateTime,
                                CancellationToken cancellationToken)
        {
            this.CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // We are passing the CancellationTokenSource into the Run method as we want to know when the CancellationToken has been cancelled
            Thread thread = new Thread(async () => await this.Run(processId, tournamentId, startDateTime, this.CancellationTokenSource.Token));
            thread.Name = $"Tournament {tournamentId} Worker Thread";
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// The cancellation token source
        /// </summary>
        private CancellationTokenSource CancellationTokenSource;

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            this.CancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Runs the specified process identifier.
        /// </summary>
        /// <param name="processId">The process identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="processRunDateTime">The process run date time.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task Run(Guid processId, Guid tournamentId, DateTime processRunDateTime, CancellationToken cancellationToken)
        {
            Boolean processedWithoutError = true;
            String processedErrorMessage = String.Empty;

            // Mark the aggregate as 'Running'
            await this.UpdateHandicapCalculationStatus(processId, tournamentId, HandicapCalculationStatus.Running, cancellationToken);

            List<PlayerScoreRecordDataTransferObject> publishedScores = new List<PlayerScoreRecordDataTransferObject>();

            try
            {
                // Rehydrate the tournament
                TournamentAggregate tournamentAggregate = await this.TournamentRepository.GetLatestVersion(tournamentId, cancellationToken);

                // Get the published scores
                publishedScores = tournamentAggregate.GetScores().Where(s => s.IsPublished).ToList();

                Int32 maxDegreeOfParallelism = 15;

                ActionBlock<PlayerScoreRecordDataTransferObject> workerBlock =
                    new ActionBlock<PlayerScoreRecordDataTransferObject>(async score =>
                                                                             await this.ApplyHandicapAdjustment(score, cancellationToken),
                                                                         new ExecutionDataflowBlockOptions
                                                                         {
                                                                             MaxDegreeOfParallelism = maxDegreeOfParallelism,
                                                                             CancellationToken = cancellationToken,
                                                                             BoundedCapacity = 20,                                              
                                                                         });

                foreach (PlayerScoreRecordDataTransferObject publishedScore in publishedScores)
                {
                    while (workerBlock.InputCount > 20)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                    }

                    workerBlock.Post(publishedScore);
                }

                workerBlock.Complete();
                await workerBlock.Completion;
            }
            catch(Exception ex)
            {
                Logger.LogCritical(ex);
                processedWithoutError = false;
                processedErrorMessage = ex.Message;
            }

            if (processedWithoutError)
            {
                Int32 scoreCount = publishedScores.Any() ? publishedScores.Count : 0;

                // update aggregate with 'Completed' event.
                await this.UpdateHandicapCalculationStatus(processId, tournamentId, HandicapCalculationStatus.Complete, cancellationToken);
                Logger.LogInformation($"Process finished, {scoreCount} scores processed.");
            }
            else
            {
                // update aggregate with 'Errored' event.
                await this.UpdateHandicapCalculationStatus(processId, tournamentId, HandicapCalculationStatus.Error, cancellationToken, processedErrorMessage);
                Logger.LogError(new Exception("Process finished with error."));
            }
        }

        /// <summary>
        /// Applies the handicap adjustment.
        /// </summary>
        /// <param name="score">The score.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task ApplyHandicapAdjustment(PlayerScoreRecordDataTransferObject score,
                                                   CancellationToken cancellationToken)
        {
            PlayerAggregate playerAggregate = await this.PlayerRepository.GetLatestVersion(score.PlayerId, cancellationToken);

            List<HandicapAdjustment> adjustments =
                this.HandicapAdjustmentCalculatorService.CalculateHandicapAdjustment(playerAggregate.ExactHandicap, score.CSS, score.HoleScores);

            Logger.LogInformation($"Player Id {playerAggregate.AggregateId} Total Adjustment [{adjustments.Sum(x => x.TotalAdjustment)}]");

            foreach (HandicapAdjustment adjustment in adjustments)
            {
                Player.HandicapAdjustmentDataTransferObject handicapAdjustment = new Player.HandicapAdjustmentDataTransferObject
                {
                                                                   TotalAdjustment = adjustment.TotalAdjustment,
                                                                   AdjustmentValuePerStroke = adjustment.AdjustmentValuePerStroke,
                                                                   NumberOfStrokesBelowCss = adjustment.NumberOfStrokesBelowCss
                                                               };

                playerAggregate.AdjustHandicap(handicapAdjustment, score.TournamentId, score.GolfClubId, score.MeasuredCourseId, score.ScoreDate);
            }

            await this.PlayerRepository.SaveChanges(playerAggregate, cancellationToken);
        }

        /// <summary>
        /// Updates the handicap calculation status.
        /// </summary>
        /// <param name="processId">The process identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        private async Task UpdateHandicapCalculationStatus(Guid processId,
                                                           Guid tournamentId,                                                     
                                                           HandicapCalculationStatus status,
                                                           CancellationToken cancellationToken,
                                                           String errorMessage = null)
        {
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate = await this.HandicapCalculationProcessRepository.GetLatestVersion(tournamentId, cancellationToken);

            switch(status)
            {
                case HandicapCalculationStatus.Running:
                    handicapCalculationProcessAggregate.UpdateProcessToRunning(DateTime.Now);
                    break;
                case HandicapCalculationStatus.Complete:
                    handicapCalculationProcessAggregate.UpdateProcessToComplete(DateTime.Now);
                    break;
                case HandicapCalculationStatus.Error:
                    handicapCalculationProcessAggregate.UpdateProcessToErrored(DateTime.Now, errorMessage);
                    break;
            }

            await this.HandicapCalculationProcessRepository.SaveChanges(handicapCalculationProcessAggregate, cancellationToken);
        }
    }
}