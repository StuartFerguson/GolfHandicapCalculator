namespace ManagementAPI.Service.Services.ExternalServices.DataTransferObjects
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class CreateRoleRequest
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>
        /// The name of the role.
        /// </value>
        public String RoleName { get; set; }
    }
}