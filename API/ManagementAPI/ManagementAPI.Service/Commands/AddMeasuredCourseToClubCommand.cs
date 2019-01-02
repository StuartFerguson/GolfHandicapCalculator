using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Commands
{
    public class AddMeasuredCourseToClubCommand : Command<String>
    {
        #region Properties

        /// <summary>
        /// Gets the add measured course to club request.
        /// </summary>
        /// <value>
        /// The add measured course to club request.
        /// </value>
        public AddMeasuredCourseToClubRequest AddMeasuredCourseToClubRequest { get; private set; }
        
        /// <summary>
        /// Gets the club configuration identifier.
        /// </summary>
        /// <value>
        /// The club configuration identifier.
        /// </value>
        public Guid ClubConfigurationId { get; private set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateClubConfigurationCommand" /> class.
        /// </summary>
        /// <param name="addMeasuredCourseToClubRequest">The add measured course to club request.</param>
        /// <param name="commandId">The command identifier.</param>
        private AddMeasuredCourseToClubCommand(Guid clubConfigurationId, AddMeasuredCourseToClubRequest addMeasuredCourseToClubRequest, Guid commandId) : base(commandId)
        {
            this.ClubConfigurationId = clubConfigurationId;
            this.AddMeasuredCourseToClubRequest = addMeasuredCourseToClubRequest;
        }
        #endregion

        #region public static AddMeasuredCourseToClubCommand Create()        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="clubConfigurationId">The club configuration identifier.</param>
        /// <param name="addMeasuredCourseToClubRequest">The add measured course to club request.</param>
        /// <returns></returns>
        public static AddMeasuredCourseToClubCommand Create(Guid clubConfigurationId, AddMeasuredCourseToClubRequest addMeasuredCourseToClubRequest)
        {
            return new AddMeasuredCourseToClubCommand(clubConfigurationId, addMeasuredCourseToClubRequest, Guid.NewGuid());
        }
        #endregion
    }
}
