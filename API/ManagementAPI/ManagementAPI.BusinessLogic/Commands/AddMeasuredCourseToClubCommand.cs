namespace ManagementAPI.BusinessLogic.Commands
{
    using System;
    using Service.DataTransferObjects.Requests;
    using Shared.CommandHandling;

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
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { get; private set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateGolfClubCommand" /> class.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="addMeasuredCourseToClubRequest">The add measured course to club request.</param>
        /// <param name="commandId">The command identifier.</param>
        private AddMeasuredCourseToClubCommand(Guid golfClubId, AddMeasuredCourseToClubRequest addMeasuredCourseToClubRequest, Guid commandId) : base(commandId)
        {
            this.GolfClubId = golfClubId;
            this.AddMeasuredCourseToClubRequest = addMeasuredCourseToClubRequest;
        }
        #endregion

        #region public static AddMeasuredCourseToClubCommand Create()        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="addMeasuredCourseToClubRequest">The add measured course to club request.</param>
        /// <returns></returns>
        public static AddMeasuredCourseToClubCommand Create(Guid golfClubId, AddMeasuredCourseToClubRequest addMeasuredCourseToClubRequest)
        {
            return new AddMeasuredCourseToClubCommand(golfClubId, addMeasuredCourseToClubRequest, Guid.NewGuid());
        }
        #endregion
    }
}
