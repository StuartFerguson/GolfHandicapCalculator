using System;
using HandicapMobile.Common;
using HandicapMobile.Pages;
using HandicapMobile.Presenters;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HandicapMobile
{
    public partial class App : Application
    {
        /// <summary>
        /// Unity container
        /// </summary>
        public static IUnityContainer Container;

        public App()
        {
            InitializeComponent();

            Bootstrapper.Run();

            Application.Current.MainPage = new NavigationPage((Page)Container.Resolve<IMainPage>());
        }

        /// <summary>
        /// Application developers override this method to perform actions when the application starts.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected async override void OnStart()
        {
            // Handle when your app starts
            var mainPagePresenter = Container.Resolve<IMainPagePresenter>();
            
            // show the login page
            await mainPagePresenter.Start();
        }

        /// <summary>
        /// Application developers override this method to perform actions when the application enters the sleeping state.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        /// <summary>
        /// Application developers override this method to perform actions when the application resumes from a sleeping state.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
