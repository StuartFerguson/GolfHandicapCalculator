namespace ManagementAPI.Service.DataTransferObjects.Responses.v2
{
    using System;
    using System.Collections.Generic;

    public class GetGolfClubResponse
    {
        /// <summary>
        /// Gets or sets the club configuration identifier.
        /// </summary>
        /// <value>
        /// The club configuration identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets the address line1.
        /// </summary>
        /// <value>
        /// The address line1.
        /// </value>
        public String AddressLine1 { get; set; }

        /// <summary>
        /// Gets the address line2.
        /// </summary>
        /// <value>
        /// The address line2.
        /// </value>
        public String AddressLine2 { get; set; }

        /// <summary>
        /// Gets the town.
        /// </summary>
        /// <value>
        /// The town.
        /// </value>
        public String Town { get; set; }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public String Region  { get; set; }

        /// <summary>
        /// Gets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public String PostalCode { get; set; }

        /// <summary>
        /// Gets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        public String TelephoneNumber { get; set; }

        /// <summary>
        /// Gets the website.
        /// </summary>
        /// <value>
        /// The website.
        /// </value>
        public String Website { get; set; }

        /// <summary>
        /// Gets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public String EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the golf club membership details response list.
        /// </summary>
        /// <value>
        /// The golf club membership details response list.
        /// </value>
        public List<GolfClubMembershipDetailsResponse> GolfClubMembershipDetailsResponseList { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public List<GolfClubUserResponse> Users { get; set; }

        /// <summary>
        /// Gets or sets the measured courses.
        /// </summary>
        /// <value>
        /// The measured courses.
        /// </value>
        public List<MeasuredCourseListResponse> MeasuredCourses { get; set; }
    }
}