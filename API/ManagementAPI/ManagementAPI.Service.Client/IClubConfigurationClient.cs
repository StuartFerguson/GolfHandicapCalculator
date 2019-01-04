using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;

namespace ManagementAPI.Service.Client
{
    public interface IClubConfigurationClient
    {
        /// <summary>
        /// Creates the club configuration.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<CreateClubConfigurationResponse> CreateClubConfiguration(CreateClubConfigurationRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the single club configuration.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="clubConfigurationId">The club configuration identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetClubConfigurationResponse> GetSingleClubConfiguration(String passwordToken, Guid clubConfigurationId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the club configuration list.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<GetClubConfigurationResponse>> GetClubConfigurationList(String passwordToken, CancellationToken cancellationToken);

        /// <summary>
        /// Adds the measured course to club configuration.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="clubConfigurationId">The club configuration identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddMeasuredCourseToClubConfiguration(String passwordToken, Guid clubConfigurationId, AddMeasuredCourseToClubRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the pending membership requests.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="clubConfigurationId">The club configuration identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<GetClubMembershipRequestResponse>> GetPendingMembershipRequests(String passwordToken, Guid clubConfigurationId, CancellationToken cancellationToken);
    }
}