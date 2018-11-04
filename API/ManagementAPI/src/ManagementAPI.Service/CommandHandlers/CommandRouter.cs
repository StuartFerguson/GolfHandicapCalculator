using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.Commands;
using Shared.CommandHandling;
using Shared.EventStore;
using ClubConfigAggregate = ManagementAPI.ClubConfigurationAggregate.ClubConfigurationAggregate;
namespace ManagementAPI.Service.CommandHandlers
{
    public class CommandRouter : ICommandRouter
    {
        #region Fields

        /// <summary>
        /// The club aggregate repository
        /// </summary>
        private readonly IAggregateRepository<ClubConfigAggregate> ClubAggregateRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRouter"/> class.
        /// </summary>
        /// <param name="clubAggregateRepository">The club aggregate repository.</param>
        public CommandRouter(IAggregateRepository<ClubConfigAggregate> clubAggregateRepository)
        {
            this.ClubAggregateRepository = clubAggregateRepository;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Routes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task Route(ICommand command, CancellationToken cancellationToken)
        {
            ICommandHandler commandHandler = CreateHandler((dynamic)command);

            await commandHandler.Handle(command, cancellationToken);
        }

        #endregion

        #region Private Methods

        #region private ICommandHandler CreateHandler(CreateClubConfigurationCommand command)        
        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(CreateClubConfigurationCommand command)
        {
            return new ClubConfigurationCommandHandler(this.ClubAggregateRepository);
        }
        #endregion

        #region private ICommandHandler CreateHandler(AddMeasuredCourseToClubCommand command)        
        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(AddMeasuredCourseToClubCommand command)
        {
            return new ClubConfigurationCommandHandler(this.ClubAggregateRepository);
        }
        #endregion

        #endregion
    }
}
