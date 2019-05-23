using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Commands
{
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;

    public class CreateGolfClubCommand : Command<CreateGolfClubResponse>
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
        /// Gets the security user identifier.
        /// </summary>
        /// <value>
        /// The security user identifier.
        /// </value>
        public Guid SecurityUserId { get; private set; }

        /// <summary>
        /// Gets or sets the create club configuration request.
        /// </summary>
        /// <value>
        /// The create club configuration request.
        /// </value>
        public CreateGolfClubRequest CreateGolfClubRequest { private set; get; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateGolfClubCommand" /> class.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="securityUserId">The security user identifier.</param>
        /// <param name="createGolfClubRequest">The create golf club request.</param>
        /// <param name="commandId">The command identifier.</param>
        private CreateGolfClubCommand(Guid golfClubId, Guid securityUserId, CreateGolfClubRequest createGolfClubRequest, Guid commandId) : base(commandId)
        {
            this.GolfClubId = golfClubId;
            this.CreateGolfClubRequest = createGolfClubRequest;
            this.SecurityUserId = securityUserId;
        }
        #endregion

        #region public static CreateGolfClubCommand Create()        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="securityUserId">The security user identifier.</param>
        /// <param name="createGolfClubRequest">The create golf club request.</param>
        /// <returns></returns>
        public static CreateGolfClubCommand Create(Guid golfClubId, Guid securityUserId, CreateGolfClubRequest createGolfClubRequest)
        {
            return new CreateGolfClubCommand(golfClubId, securityUserId, createGolfClubRequest, Guid.NewGuid());
        }
        #endregion
    }
}
