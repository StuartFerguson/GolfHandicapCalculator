namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    using System;

    public class MembersByHandicapCategoryResponse
    {
        /// <summary>
        /// Gets or sets the handicap category.
        /// </summary>
        /// <value>
        /// The handicap category.
        /// </value>
        public Int32 HandicapCategory { get; set; }

        /// <summary>
        /// Gets or sets the number of members.
        /// </summary>
        /// <value>
        /// The number of members.
        /// </value>
        public Int32 NumberOfMembers { get; set; }
    }
}