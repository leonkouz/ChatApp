﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core
{
    /// <summary>
    /// Prefixes to add to the beginning of TCP strings to indicate what the data is for.
    /// </summary>
    public enum DataPrefix
    {
        [Description("-msg")]
        Message = 0,


    }

}
