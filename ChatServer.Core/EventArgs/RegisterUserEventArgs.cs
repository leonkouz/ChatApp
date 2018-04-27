using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Core
{
    public class RegisterUserEventArgs : EventArgs
    {
        public StatusCode Status { get; set; }
    }
}
