namespace MobileAppConfigurationAPI.Database.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    public class ApplicationConfiguration
    {
        [Key]
        public String IMEINumber { get; set; }

        public String SecurityServiceUri { get; set; }

        public String ManagementApiUri { get; set; }
    }
}