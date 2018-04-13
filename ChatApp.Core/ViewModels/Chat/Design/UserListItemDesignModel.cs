using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core
{

    /// <summary>
    /// The design-time data for a <see cref="UserListItemViewModel"/>
    /// </summary>
    public class UserListItemDesignModel : UserListItemViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static UserListItemDesignModel Instance => new UserListItemDesignModel();

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserListItemDesignModel()
        {
            Initials = "LM";
            Name = "Luke";
            StatusMessage = "This is my status message";
            Colour = "3099c5";
        }
    }
}
