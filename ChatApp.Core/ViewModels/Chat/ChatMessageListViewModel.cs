﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core
{
    /// <summary>
    /// A view model for a chat message thread
    /// </summary>
    public class ChatMessageListViewModel : BaseViewModel
    {
        /// <summary>
        /// The chat thread items for the list
        /// </summary>
        public List<ChatMessageListItemViewModel> Items { get; set; }

    }
}
