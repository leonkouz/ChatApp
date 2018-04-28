using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Core
{
    public class Response
    {
        public StatusCode Status { get; set; }
        public string Error { get; set; }

    }
}
