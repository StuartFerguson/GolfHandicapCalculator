namespace ManagementAPI.Service.Services.ExternalServices.DataTransferObjects
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class CreateRoleResponse
    {
        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>
        /// The role identifier.
        /// </value>
        public Guid RoleId { get; set; }
    }
}