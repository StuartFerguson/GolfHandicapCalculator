namespace ManagementAPI.Service.Common
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        #region Methods

        /// <summary>
        /// Converts to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// Source type is not enum
        /// or
        /// Destination type is not enum
        /// </exception>
        public static T ConvertTo<T>(this Object value) where T : struct, IConvertible
        {
            Type sourceType = value.GetType();
            if (!sourceType.IsEnum)
                throw new ArgumentException("Source type is not enum");
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Destination type is not enum");
            return (T)Enum.Parse(typeof(T), value.ToString());
        }

        #endregion
    }
}