using System;

namespace ManagementAPI.Service.DataTransferObjects
{
    public class CreateClubConfigurationResponse
    {
        /// <summary>
        /// Gets or sets the club configuration identifier.
        /// </summary>
        /// <value>
        /// The club configuration identifier.
        /// </value>
        public Guid ClubConfigurationId { get; set; }
    }
}