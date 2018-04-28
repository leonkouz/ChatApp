using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core
{
    /// <summary>
    /// A view model for each chat list item in the overview chat list
    /// </summary>
    public class UserListItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The user id
        /// </summary>
        private string ID { get; set; }

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The password of the user
        /// </summary>
        public SecureString Password { get; set; }

        /// <summary>
        /// The full name of the user
        /// </summary>
        public string FullName => FirstName + " " + LastName;

        /// <summary>
        /// The first name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The status message of the user
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// The intiails of the user
        /// </summary>
        public string Initials => GetInitials(FirstName, LastName);

        /// <summary>
        /// True if the user is online
        /// </summary>
        public bool Online { get; set; }

        /// <summary>
        /// The colour (in hex) the user selected
        /// </summary>
        public string Colour { get; set; }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the initials of the name
        /// </summary>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The second name</param>
        /// <returns></returns>
        public string GetInitials(string firstName, string lastName)
        {
            return (firstName.First() + lastName.First()).ToString();
        }

        #endregion

    }
}
