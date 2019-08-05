namespace DomainEventRouterAPI.Service.EventHandling
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using ManagementAPI.GolfClubMembership.DomainEvents;
    using ManagementAPI.Service.Client;
    using ManagementAPI.Service.DataTransferObjects.Responses;
    using Models;
    using Services;
    using Shared.EventSourcing;
    using Shared.General;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DomainEventRouterAPI.Service.EventHandling.IDomainEventHandler" />
    public class GolfClubMembershipDomainEventHandler : IDomainEventHandler
    {
        #region Fields

        /// <summary>
        /// The domain event types to silently handle
        /// </summary>
        private readonly IDomainEventTypesToSilentlyHandle DomainEventTypesToSilentlyHandle;

        /// <summary>
        /// The elastic service
        /// </summary>
        private readonly IElasticService ElasticService;

        /// <summary>
        /// The golf club client
        /// </summary>
        private readonly IGolfClubClient GolfClubClient;

        /// <summary>
        /// The model factory
        /// </summary>
        private readonly IModelFactory ModelFactory;

        /// <summary>
        /// The player client
        /// </summary>
        private readonly IPlayerClient PlayerClient;

        /// <summary>
        /// The security service client
        /// </summary>
        private readonly ISecurityServiceClient SecurityServiceClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubMembershipDomainEventHandler"/> class.
        /// </summary>
        /// <param name="securityServiceClient">The security service client.</param>
        /// <param name="golfClubClient">The golf club client.</param>
        /// <param name="playerClient">The player client.</param>
        /// <param name="modelFactory">The model factory.</param>
        /// <param name="elasticService">The elastic service.</param>
        /// <param name="domainEventTypesToSilentlyHandle">The domain event types to silently handle.</param>
        public GolfClubMembershipDomainEventHandler(ISecurityServiceClient securityServiceClient,
                                                    IGolfClubClient golfClubClient,
                                                    IPlayerClient playerClient,
                                                    IModelFactory modelFactory,
                                                    IElasticService elasticService,
                                                    IDomainEventTypesToSilentlyHandle domainEventTypesToSilentlyHandle)
        {
            this.SecurityServiceClient = securityServiceClient;
            this.GolfClubClient = golfClubClient;
            this.PlayerClient = playerClient;
            this.ModelFactory = modelFactory;
            this.ElasticService = elasticService;
            this.DomainEventTypesToSilentlyHandle = domainEventTypesToSilentlyHandle;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the specified domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task Handle(DomainEvent domainEvent,
                                 CancellationToken cancellationToken)
        {
            await this.HandleSpecificDomainEvent((dynamic)domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(ClubMembershipRequestAcceptedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            // Get the client token 
            String clientToken = await this.SecurityServiceClient.GetClientToken("golfhandicap.testdatagenerator", "golfhandicap.testdatagenerator", cancellationToken);

            // Get the golf club
            GetGolfClubResponse golfClub = await this.GolfClubClient.GetSingleGolfClub(clientToken, domainEvent.AggregateId, cancellationToken);

            // Get the player
            GetPlayerDetailsResponse player = await this.PlayerClient.GetPlayer(clientToken, domainEvent.PlayerId, cancellationToken);

            // Build the model to post to elastic search service
            GolfClubMembershipModel model = this.ModelFactory.Create(golfClub, player, domainEvent);

            // Post this model to Elastic Search
            String indexName = $"GolfClubMembership_{golfClub.Name.Replace(" ", string.Empty)}";
            String documentId = $"{golfClub.Id}-{domainEvent.PlayerId}";

            await this.ElasticService.PostModel(model, indexName, documentId, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="Exception">No event handler for {domainEvent.GetType()}</exception>
        private Task HandleSpecificDomainEvent(DomainEvent domainEvent,
                                               CancellationToken cancellationToken)
        {
            if (this.DomainEventTypesToSilentlyHandle.HandleSilently(this.GetType().Name, domainEvent))
            {
                //Silently handle this.
                return Task.CompletedTask;
            }

            Logger.LogWarning($"No event handler for {domainEvent.GetType()}");

            // Not sure yet if/how we want to handle these events. Handler added so nothing is written to log file to prevent them filling up.
            throw new Exception($"No event handler for {domainEvent.GetType()}");
        }

        #endregion
    }
}