namespace ManagementAPI.Service
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.StatusCodeResult" />
    public class NotImplementedResult : StatusCodeResult
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotImplementedResult"/> class.
        /// </summary>
        public NotImplementedResult() : base(NotImplementedResult.DefaultStatusCode)
        {
        }

        #endregion

        #region Others

        /// <summary>
        /// The default status code
        /// </summary>
        private const Int32 DefaultStatusCode = 501;

        #endregion
    }
}