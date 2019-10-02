namespace ManagementAPI.Service
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        #region Methods

        /// <summary>
        /// Methods the not allowed.
        /// </summary>
        /// <param name="controllerBase">The controller base.</param>
        /// <returns></returns>
        public static MethodNotAllowedResult MethodNotAllowed(this ControllerBase controllerBase)
        {
            return new MethodNotAllowedResult();
        }

        /// <summary>
        /// Nots the implemented.
        /// </summary>
        /// <param name="controllerBase">The controller base.</param>
        /// <returns></returns>
        public static NotImplementedResult NotImplemented(this ControllerBase controllerBase)
        {
            return new NotImplementedResult();
        }

        #endregion
    }
}