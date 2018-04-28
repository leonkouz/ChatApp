using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Shared
{
    public class LoginToken
    {
        #region Public Properties

        /// <summary>
        /// The user's email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's password
        /// </summary>
        public SecureString Password { get; set; }

        #endregion

        /// <summary>
        /// Concatenates user information to send to the server in order to register user
        /// </summary>
        /// <returns></returns>
        public string BuildRegisterUserTcpString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Email); // test@test.com
            sb.Append(Constants.Delimiter);
            sb.Append(Password.Unsecure()); //*****
            sb.Append(Constants.Delimiter);

            // String of data looks like this:
            // test@test.com\0*****

            return sb.ToString();
        }

    }
}
