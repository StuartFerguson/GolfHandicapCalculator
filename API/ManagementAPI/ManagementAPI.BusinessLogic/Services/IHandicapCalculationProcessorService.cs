namespace ManagementAPI.BusinessLogic.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IHandicapCalculationProcessorService
    {
        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <param name="processId">The process identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="startDateTime">The start date time.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task Start(Guid processId,
                   Guid tournamentId,
                   DateTime startDateTime,
                   CancellationToken cancellationToken);

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        void Stop();
    }

    /// <summary>
    /// 
    /// </summary>
    public enum HandicapCalculationStatus
    {
        /// <summary>
        /// The running
        /// </summary>
        Running,
        /// <summary>
        /// The complete
        /// </summary>
        Complete,
        /// <summary>
        /// The error
        /// </summary>
        Error
    }
}
