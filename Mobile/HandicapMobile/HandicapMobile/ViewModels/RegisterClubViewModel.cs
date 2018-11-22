using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HandicapMobile.ViewModels
{ 
    public class RegisterClubViewModel : BindableObject
    {
        private String clubName;

        public RegisterClubViewModel()
        {
            ClubName = String.Empty;
        }

        public String ClubName
        {
            get { return clubName; }
            set
            {
                clubName = value;
                OnPropertyChanged(clubName);
            }
        }
    }
}
