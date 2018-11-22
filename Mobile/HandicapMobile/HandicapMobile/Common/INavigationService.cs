using HandicapMobile.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HandicapMobile.Common
{
    public interface INavigationService
    {
        /// <summary>
        /// Pushes the specified page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        Task Push(IPage page);

        /// <summary>
        /// Pops this instance.
        /// </summary>
        /// <returns></returns>
        Task Pop();
    }
}
