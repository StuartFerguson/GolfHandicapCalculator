using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ManagementAPI.Database.Models
{
    public class ClubInformation
    {
        /// <summary>
        /// Gets or sets the club configuration identifier.
        /// </summary>
        /// <value>
        /// The club configuration identifier.
        /// </value>
        [Key]
        public Guid ClubConfigurationId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>
        /// The address line1.
        /// </value>
        public String AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>
        /// The address line2.
        /// </value>
        public String AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the town.
        /// </summary>
        /// <value>
        /// The town.
        /// </value>
        public String Town { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public String Region { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public String PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        public String TelephoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the web site.
        /// </summary>
        /// <value>
        /// The web site.
        /// </value>
        public String WebSite { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public String EmailAddress { get; set; }
    }
}
