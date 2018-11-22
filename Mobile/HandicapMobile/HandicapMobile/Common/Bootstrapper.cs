using System;
using System.Collections.Generic;
using System.Text;
using HandicapMobile.Pages;
using HandicapMobile.Presenters;
using HandicapMobile.ViewModels;
using HandicapMobile.Views;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace HandicapMobile.Common
{
    public class Bootstrapper
    {
        /// <summary>
        /// Runs this instance.
        /// </summary>
        public static void Run()
        {
            var unityContainer = new UnityContainer();

            // Presentation registrations
            unityContainer.RegisterType<IMainPagePresenter, MainPagePresenter>(new SingletonLifetimeManager());
            unityContainer.RegisterType<IRegisterClubPresenter, RegisterClubPresenter>(new SingletonLifetimeManager());

            // View registrations
            unityContainer.RegisterType<IMainPage, MainPage>(new SingletonLifetimeManager());
            unityContainer.RegisterType<IRegisterClubPage, RegisterClubPage>(new SingletonLifetimeManager());

            // View model registrations
            unityContainer.RegisterType<RegisterClubViewModel>(new TransientLifetimeManager());

            // Other registrations
            unityContainer.RegisterType<INavigationService, NavigationService>(
                new InjectionConstructor(unityContainer.Resolve<IMainPage>()));

            App.Container = unityContainer;
        }
    }
}
