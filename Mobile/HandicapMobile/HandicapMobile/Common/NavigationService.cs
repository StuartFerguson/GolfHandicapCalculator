using System.Threading.Tasks;
using HandicapMobile.Pages;
using Xamarin.Forms;

namespace HandicapMobile.Common
{
    public class NavigationService : INavigationService
    {
        #region Fields     
        
        /// <summary>
        /// The parent page
        /// </summary>
        private readonly Page ParentPage;
        
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <param name="parentPage">The parent page.</param>
        public NavigationService(Page parentPage)
        {
            this.ParentPage = parentPage;
        }
        #endregion

        #region Public Methods

        #region public async Task Push(IPage page)        
        /// <summary>
        /// Pushes the specified page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public async Task Push(IPage page)
        {
            await this.ParentPage.Navigation.PushAsync((Page)page);
        }
        #endregion

        #region public async Task Pop()        
        /// <summary>
        /// Pops this instance.
        /// </summary>
        /// <returns></returns>
        public async Task Pop()
        {
            await this.ParentPage.Navigation.PopAsync();
        }
        #endregion

        #endregion
    }
}