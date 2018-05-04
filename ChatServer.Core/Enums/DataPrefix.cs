using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Shared
{
    /// <summary>
    /// Prefixes to add to the beginning of TCP strings to indicate what the data is for.
    /// </summary>
    public enum DataPrefix
    {
        [Description("none")]
        None = 0,

        [Description("-msg")]
        Message = 1,

        [Description("-reguser")]
        RegisterUser = 2,

        [Description("-login")]
        LoginUser = 3

    }
}
