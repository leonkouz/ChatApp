using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Shared
{
    public class ServerResponseEventArgs : EventArgs
    {
        /// <summary>
        /// The type of response.
        /// </summary>
        public DataPrefix Type { get; set; }

        /// <summary>
        /// The status of the response.
        /// </summary>
        public StatusCode Status { get; set; }

        /// <summary>
        /// Error message returned from the server.
        /// </summary>
        public string Error { get; set; }
    }
}
