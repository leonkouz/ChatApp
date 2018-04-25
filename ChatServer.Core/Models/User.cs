﻿using System.Linq;
using System.Security;
using System.Text;

namespace ChatServer.Core
{
    /// <summary>
    /// The User model for the TCP client
    /// </summary>
    public class User
    {
        /// <summary>
        /// The user id
        /// </summary>
        private string ID { get; set; }

        /// <summary>
        /// The display name of the user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The password of the user
        /// </summary>
        public SecureString Password { get; }

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

        #region Helper Functions

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

        public string BuildTcpString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(UserName);
            sb.Append(Constants.Delimiter);
            sb.Append(Email);
            sb.Append(Constants.Delimiter);
            sb.Append(Password.Unsecure());

            return sb.ToString();

        }

        #endregion




    }
}
