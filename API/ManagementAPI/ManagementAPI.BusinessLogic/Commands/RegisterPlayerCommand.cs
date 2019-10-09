namespace ManagementAPI.BusinessLogic.Commands
{
    using System;
    using Service.DataTransferObjects.Requests;
    using Service.DataTransferObjects.Responses;
    using Shared.CommandHandling;

    public class RegisterPlayerCommand : Command<RegisterPlayerResponse>
    {
        #region Properties

        /// <summary>
        /// Gets the register player request.
        /// </summary>
        /// <value>
        /// The register player request.
        /// </value>
        public RegisterPlayerRequest RegisterPlayerRequest { get; private set; }

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public Guid PlayerId { get; private set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Gets the add measured course to club request.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="registerPlayerRequest">The register player request.</param>
        /// <param name="commandId">The command identifier.</param>
        /// <value>
        /// The add measured course to club request.
        /// </value>
        private RegisterPlayerCommand(Guid playerId, RegisterPlayerRequest registerPlayerRequest, Guid commandId) : base(commandId)
        {
            this.PlayerId = playerId;
            this.RegisterPlayerRequest = registerPlayerRequest;
        }
        #endregion

        #region public static RegisterPlayerCommand Create()        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="registerPlayerRequest">The register player request.</param>
        /// <returns></returns>
        public static RegisterPlayerCommand Create(Guid playerId, RegisterPlayerRequest registerPlayerRequest)
        {
            return new RegisterPlayerCommand(playerId, registerPlayerRequest, Guid.NewGuid());
        }
        #endregion
    }
}
