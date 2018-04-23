using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        #region Private Fields

        /// <summary>
        /// The current page displayed in the window
        /// </summary>
        private ApplicationPage _currentPage = ApplicationPage.GlobalChat;


        /// <summary>
        /// Indicites whether the side menu should be shown
        /// </summary>
        private bool _sideMenuVisible = true;

        #endregion

        #region Public Properties

        /// <summary>
        /// The current page displayed in the window
        /// </summary>
        public ApplicationPage CurrentPage
        {
            get => _currentPage;
            private set
            {
                _currentPage = value;
                RaisePropertyChangedEvent("CurrentPage");
            }
        }

        /// <summary>
        /// True if the side menu should be shown
        /// </summary>
        public bool SideMenuVisible
        {
            get => _sideMenuVisible;
            set
            {
                _sideMenuVisible = value;
                RaisePropertyChangedEvent("SideMenuVisible");
            }
        }

        #endregion

        /// <summary>
        /// Navigates to the specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        public void GoToPage(ApplicationPage page)
        {
            // Set the current page
            CurrentPage = page;

            // Show side menu or not
            if (page == ApplicationPage.GlobalChat)
                SideMenuVisible = true;
        }
    }
}
