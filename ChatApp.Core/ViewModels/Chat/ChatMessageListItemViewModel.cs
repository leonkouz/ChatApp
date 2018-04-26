using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core
{
    /// <summary>
    /// A view model for each chat message thread item in a chat thread
    /// </summary>
    public class ChatMessageListItemViewModel : BaseViewModel
    {
        /// <summary>
        /// The display name of the sender of the message
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// The intiails of the user
        /// </summary>
        public string Initials { get; set; }

        /// <summary>
        /// The content of the message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The colour (in hex) the user selected
        /// </summary>
        public string Colour { get; set; }

        /// <summary>
        /// True if this item is currently selected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// True if this message was sent by the signed in user
        /// </summary>
        public bool SentByMe { get; set; }

        /// <summary>
        /// True if the server returns a message sent confirmation
        /// </summary>
        public bool MessageSendSuccessfull { get; set; }

        /// <summary>
        /// The time the message was sent
        /// </summary>
        public DateTimeOffset MessageSentTime { get; set; }
    }
}
