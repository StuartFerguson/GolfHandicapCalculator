namespace ManagementAPI.Service.DataTransferObjects.Requests
{
    using System;

    public class RegisterClubAdministratorRequest
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public String EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public String Password { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>
        /// The confirm password.
        /// </value>
        public String ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        public String TelephoneNumber { get; set; }
    }
}
