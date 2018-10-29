using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;
//using ManagementAPI.Service.DataTransferObjects;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Commands
{
    public class CreateClubConfigurationCommand : Command<CreateClubConfigurationResponse>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the create club configuration request.
        /// </summary>
        /// <value>
        /// The create club configuration request.
        /// </value>
        public CreateClubConfigurationRequest CreateClubConfigurationRequest { private set; get; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateClubConfigurationCommand" /> class.
        /// </summary>
        /// <param name="createClubConfigurationRequest">The create club configuration request.</param>
        /// <param name="commandId">The command identifier.</param>
        private CreateClubConfigurationCommand(CreateClubConfigurationRequest createClubConfigurationRequest, Guid commandId) : base(commandId)
        {
            this.CreateClubConfigurationRequest = createClubConfigurationRequest;
        }
        #endregion

        #region public static CreateClubCommand Create()        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="createClubConfigurationRequest">The create club configuration request.</param>
        /// <returns></returns>
        public static CreateClubConfigurationCommand Create(CreateClubConfigurationRequest createClubConfigurationRequest)
        {
            return new CreateClubConfigurationCommand(createClubConfigurationRequest, Guid.NewGuid());
        }
        #endregion
    }
}
