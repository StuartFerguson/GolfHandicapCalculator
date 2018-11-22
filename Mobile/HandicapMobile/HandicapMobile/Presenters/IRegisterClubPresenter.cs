using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HandicapMobile.Common;
using HandicapMobile.Pages;
using HandicapMobile.ViewModels;

namespace HandicapMobile.Presenters
{
    public interface IRegisterClubPresenter : IPresenter
    {

    }

    public class RegisterClubPresenter : IRegisterClubPresenter
    {
        private readonly INavigationService NavigationService;
        private readonly IRegisterClubPage RegisterClubPage;
        private readonly RegisterClubViewModel RegisterClubViewModel;

        public RegisterClubPresenter(INavigationService navigationService, IRegisterClubPage registerClubPage, RegisterClubViewModel viewModel)
        {
            this.NavigationService = navigationService;
            this.RegisterClubPage = registerClubPage;
            this.RegisterClubViewModel = viewModel;
        }
        public async Task Start()
        {
            this.RegisterClubPage.RegisterClubButtonClick += RegisterClubPage_RegisterClubButtonClick;        
            this.RegisterClubPage.Init(this.RegisterClubViewModel);            
            await this.NavigationService.Push(this.RegisterClubPage);
        }

        private void RegisterClubPage_RegisterClubButtonClick(object sender, EventArgs e)
        {
            App.Current.MainPage.DisplayAlert("Test", this.RegisterClubViewModel.ClubName, "OK");
        }
    }
}
