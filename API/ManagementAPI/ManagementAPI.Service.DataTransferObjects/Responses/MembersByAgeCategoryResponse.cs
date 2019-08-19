namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    using System;

    public class MembersByAgeCategoryResponse
    {
        /// <summary>
        /// Gets or sets the age category.
        /// </summary>
        /// <value>
        /// The age category.
        /// </value>
        public String AgeCategory { get; set; }

        /// <summary>
        /// Gets or sets the number of members.
        /// </summary>
        /// <value>
        /// The number of members.
        /// </value>
        public Int32 NumberOfMembers { get; set; }
    }
}