﻿using System.Linq;
using System.Security;
using System.Text;

namespace ChatServer.Shared
{
    /// <summary>
    /// The User model for the TCP client
    /// </summary>
    public class RegisterUserToken
    {
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

        /// <summary>
        /// Concatenates user information to send to the server in order to register user
        /// </summary>
        /// <returns></returns>
        public string BuildRegisterUserTcpString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Email); // test@test.com
            sb.Append(Constants.Delimiter);
            sb.Append(FirstName); // Bob
            sb.Append(Constants.Delimiter);
            sb.Append(LastName); // Johns
            sb.Append(Constants.Delimiter);
            sb.Append(Password.Unsecure()); //*****

            // Add the data prefix so the server is aware of the type of request
            string str = DataPrefix.RegisterUser.GetDescription() + sb.ToString();

            // String of data looks like this:
            // -regusertest@test.com\0Bob\0Johns\0*****

            return str;
        }

        #endregion




    }
}
