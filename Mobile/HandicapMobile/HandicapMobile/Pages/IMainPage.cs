using System;
using System.Collections.Generic;
using System.Text;

namespace HandicapMobile.Pages
{
    public interface IMainPage : IPage
    {
        /// <summary>
        /// Occurs when [register club button click].
        /// </summary>
        event EventHandler RegisterClubButtonClick;
    }
}
