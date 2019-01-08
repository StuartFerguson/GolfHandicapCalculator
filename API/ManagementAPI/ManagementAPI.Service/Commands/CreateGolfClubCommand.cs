using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Commands
{
    public class CreateGolfClubCommand : Command<CreateGolfClubResponse>
    {
        #region Properties

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
        /// <param name="createGolfClubRequest">The create golf club request.</param>
        /// <param name="commandId">The command identifier.</param>
        private CreateGolfClubCommand(CreateGolfClubRequest createGolfClubRequest, Guid commandId) : base(commandId)
        {
            this.CreateGolfClubRequest = createGolfClubRequest;
        }
        #endregion

        #region public static CreateGolfClubCommand Create()        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="createGolfClubRequest">The create golf club request.</param>
        /// <returns></returns>
        public static CreateGolfClubCommand Create(CreateGolfClubRequest createGolfClubRequest)
        {
            return new CreateGolfClubCommand(createGolfClubRequest, Guid.NewGuid());
        }
        #endregion
    }
}
