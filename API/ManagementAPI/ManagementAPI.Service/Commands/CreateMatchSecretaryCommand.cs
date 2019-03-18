using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Commands
{
    public class CreateMatchSecretaryCommand : Command<String>
    {
        #region Properties

        /// <summary>
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { get; private set; }

        /// <summary>
        /// Gets or sets the create match secretary request.
        /// </summary>
        /// <value>
        /// The create match secretary request.
        /// </value>
        public CreateMatchSecretaryRequest CreateMatchSecretaryRequest { private set; get; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMatchSecretaryCommand" /> class.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="createMatchSecretaryRequest">The create match secretary request.</param>
        /// <param name="commandId">The command identifier.</param>
        private CreateMatchSecretaryCommand(Guid golfClubId, CreateMatchSecretaryRequest createMatchSecretaryRequest, Guid commandId) : base(commandId)
        {
            this.GolfClubId = golfClubId;
            this.CreateMatchSecretaryRequest = createMatchSecretaryRequest;
        }
        #endregion

        #region public static CreateMatchSecretaryCommand Create()        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="createMatchSecretaryRequest">The create match secretary request.</param>
        /// <returns></returns>
        public static CreateMatchSecretaryCommand Create(Guid golfClubId, CreateMatchSecretaryRequest createMatchSecretaryRequest)
        {
            return new CreateMatchSecretaryCommand(golfClubId,  createMatchSecretaryRequest, Guid.NewGuid());
        }
        #endregion
    }
}
