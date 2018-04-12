﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// A view model for the overview chat list
    /// </summary>
    public class UserListViewModel : BaseViewModel
    {
        /// <summary>
        /// The user list items for the list
        /// </summary>
        public List<UserListItemViewModel> Items { get; set; }

    }
}