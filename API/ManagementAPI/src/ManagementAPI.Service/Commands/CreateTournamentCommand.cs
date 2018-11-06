using Shared.CommandHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;

namespace ManagementAPI.Service.Commands
{
    public class CreateTournamentCommand : Command<CreateTournamentResponse>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the create tournament request.
        /// </summary>
        /// <value>
        /// The create tournament request.
        /// </value>
        public CreateTournamentRequest CreateTournamentRequest { private set; get; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTournamentCommand" /> class.
        /// </summary>
        /// <param name="createTournamentRequest">The create tournament request.</param>
        /// <param name="commandId">The command identifier.</param>
        private CreateTournamentCommand(CreateTournamentRequest createTournamentRequest, Guid commandId) : base(commandId)
        {
            this.CreateTournamentRequest = createTournamentRequest;
        }
        #endregion

        #region public static CreateTournamentCommand Create()        
        /// <summary>
        /// Creates the specified create tournament request.
        /// </summary>
        /// <param name="createTournamentRequest">The create tournament request.</param>
        /// <returns></returns>
        public static CreateTournamentCommand Create(CreateTournamentRequest createTournamentRequest)
        {
            return new CreateTournamentCommand(createTournamentRequest, Guid.NewGuid());
        }
        #endregion
    }
}
