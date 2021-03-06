﻿namespace ManagementAPI.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;

    [ExcludeFromCodeCoverage]
    public static class Retry
    {
        #region Fields

        /// <summary>
        /// The default retry for
        /// </summary>
        private static readonly TimeSpan DefaultRetryFor = TimeSpan.FromSeconds(60);

        /// <summary>
        /// The default retry interval
        /// </summary>
        private static readonly TimeSpan DefaultRetryInterval = TimeSpan.FromMilliseconds(200);

        #endregion

        #region Methods

        /// <summary>
        /// Fors the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="retryFor">The retry for.</param>
        /// <param name="retryInterval">The retry interval.</param>
        /// <returns></returns>
        public static async Task For(Func<Task> action,
                                     TimeSpan? retryFor = null,
                                     TimeSpan? retryInterval = null)
        {
            DateTime startTime = DateTime.Now;
            Exception lastException = null;

            if (retryFor == null)
            {
                retryFor = Retry.DefaultRetryFor;
            }

            while (DateTime.Now.Subtract(startTime).TotalMilliseconds < retryFor.Value.TotalMilliseconds)
            {
                try
                {
                    await action();
                    lastException = null;
                    break;
                }
                catch(Exception e)
                {
                    lastException = e;

                    // wait before retrying
                    Thread.Sleep(retryInterval ?? Retry.DefaultRetryInterval);
                }
            }

            if (lastException != null)
            {
                throw lastException;
            }
        }

        #endregion
    }
}