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

        /// <summary>
        /// The current page displayed in the window
        /// </summary>
        private ApplicationPage _currentPage = ApplicationPage.Login;

        /// <summary>
        /// The current page displayed in the window
        /// </summary>
        public ApplicationPage CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                RaisePropertyChangedEvent("CurrentPage");
            }
        }

    }
}
