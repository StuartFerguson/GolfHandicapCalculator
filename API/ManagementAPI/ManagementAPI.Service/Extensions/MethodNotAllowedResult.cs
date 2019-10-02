namespace ManagementAPI.Service
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="StatusCodeResult" />
    public class MethodNotAllowedResult : StatusCodeResult
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodNotAllowedResult"/> class.
        /// </summary>
        public MethodNotAllowedResult() : base(MethodNotAllowedResult.DefaultStatusCode)
        {
        }

        #endregion

        #region Others

        /// <summary>
        /// The default status code
        /// </summary>
        private const Int32 DefaultStatusCode = 405;

        #endregion
    }
}