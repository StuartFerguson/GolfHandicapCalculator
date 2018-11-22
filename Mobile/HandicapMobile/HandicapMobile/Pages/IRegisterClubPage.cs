using System;
using HandicapMobile.ViewModels;

namespace HandicapMobile.Pages
{
    public interface IRegisterClubPage : IPage
    {
        /// <summary>
        /// Occurs when [register club button click].
        /// </summary>
        event EventHandler RegisterClubButtonClick;

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        void Init(RegisterClubViewModel viewModel);
    }
}