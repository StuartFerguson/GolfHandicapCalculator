using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    public class GetGolfClubUserListResponse
    {
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public List<GolfClubUserResponse> Users { get; set; }
    }
}
