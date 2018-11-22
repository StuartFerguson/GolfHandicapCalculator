using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandicapMobile.Pages;
using HandicapMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HandicapMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterClubPage : ContentPage, IRegisterClubPage
	{
        /// <summary>
        /// The view model
        /// </summary>
        private RegisterClubViewModel ViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterClubPage"/> class.
        /// </summary>
        public RegisterClubPage()
		{
			InitializeComponent();

		    txtClubName.Focus();
		    btnRegisterClub.Clicked += BtnRegisterClub_Clicked;
		}

        /// <summary>
        /// Occurs when [register club button click].
        /// </summary>
        public event EventHandler RegisterClubButtonClick;

        /// <summary>
        /// Initializes the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void Init(RegisterClubViewModel viewModel)
	    {
	        this.ViewModel = viewModel;

            txtClubName.TextChanged += TxtClubName_TextChanged;
	    }

        /// <summary>
        /// Handles the TextChanged event of the TxtClubName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void TxtClubName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.ViewModel.ClubName = e.NewTextValue;
        }

        /// <summary>
        /// Handles the Clicked event of the BtnRegisterClub control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BtnRegisterClub_Clicked(object sender, EventArgs e)
        {
            RegisterClubButtonClick(sender, e);
        }
    }
}