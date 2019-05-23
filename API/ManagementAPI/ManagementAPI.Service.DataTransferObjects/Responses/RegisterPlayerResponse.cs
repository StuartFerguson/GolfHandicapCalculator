namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    using System;

    public class RegisterPlayerResponse
    {
        /// <summary>
        /// Gets or sets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public Guid PlayerId { get; set; }
    }
}