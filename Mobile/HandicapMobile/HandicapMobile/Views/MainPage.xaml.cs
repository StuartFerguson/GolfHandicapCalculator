using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandicapMobile.Pages;
using HandicapMobile.Presenters;
using Xamarin.Forms;

namespace HandicapMobile
{
    public partial class MainPage : ContentPage, IMainPage, IPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            this.btnRegisterClub.Clicked += BtnRegisterClub_Clicked;
        }

        /// <summary>
        /// Occurs when [register club button click].
        /// </summary>
        public event EventHandler RegisterClubButtonClick;

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
