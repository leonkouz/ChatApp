using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core
{
    /// <summary>
    /// A view model for each chat list item in the overview chat list
    /// </summary>
    public class UserListItemViewModel : BaseViewModel
    {
        /// <summary>
        /// The display name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The status message of the user
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// The intiails of the user
        /// </summary>
        public string Initials { get; set; }

        /// <summary>
        /// The colour (in hex) the user selected
        /// </summary>
        public string Colour { get; set; }


    }
}
