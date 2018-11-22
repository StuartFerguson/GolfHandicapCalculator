using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HandicapMobile.Common
{
    public interface IPresenter
    {
        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <returns></returns>
        Task Start();
    }
}
