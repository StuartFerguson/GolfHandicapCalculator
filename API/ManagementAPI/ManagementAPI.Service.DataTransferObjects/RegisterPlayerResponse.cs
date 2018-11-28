using System;

namespace ManagementAPI.Service.DataTransferObjects
{
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