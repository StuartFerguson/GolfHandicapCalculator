using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HandicapMobile.Common;
using HandicapMobile.Pages;

namespace HandicapMobile.Presenters
{
    public interface IMainPagePresenter : IPresenter
    {
        
    }

    public class MainPagePresenter : IMainPagePresenter
    {
        private readonly IMainPage MainPage;
        private readonly IRegisterClubPresenter RegisterClubPresenter;
        
        public MainPagePresenter(IMainPage mainPage, IRegisterClubPresenter registerClubPresenter)
        {
            this.MainPage = mainPage;            
            this.RegisterClubPresenter = registerClubPresenter;

            this.MainPage.RegisterClubButtonClick += MainPage_RegisterClubButtonClick;
        }

        private async void MainPage_RegisterClubButtonClick(object sender, EventArgs e)
        {
            await this.RegisterClubPresenter.Start();
        }

        public async Task Start()
        {
            //App.Current.MainPage.DisplayAlert("Test", "Test", "OK");
        }
    }
}
