using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Shared
{
    public static class Constants
    {
        /// <summary>
        /// Used as a delimiter for data sent by the server/client as the character cannot be typed by a user.
        /// </summary>
        public const string Delimiter = "\0"; 

        /// <summary>
        ///  Indicates the end of a string sent over the server.
        /// </summary>
        public const string EndOfFile = "<EOF>"; 

    }
}
