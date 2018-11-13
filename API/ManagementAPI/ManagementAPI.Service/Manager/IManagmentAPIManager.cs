using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;

namespace ManagementAPI.Service.Manager
{
    public interface IManagmentAPIManager
    {
        /// <summary>
        /// Gets the club configuration.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <returns></returns>
        Task<GetClubConfigurationResponse> GetClubConfiguration(Guid clubId, CancellationToken cancellationToken);
    }
}
