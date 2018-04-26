using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core
{

    /// <summary>
    /// The design-time data for a <see cref="ChatMessageListViewModel"/>
    /// </summary>
    public class ChatMessageListDesignModel : ChatMessageListViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static ChatMessageListDesignModel Instance => new ChatMessageListDesignModel();

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatMessageListDesignModel()
        {
            Items = new List<ChatMessageListItemViewModel>
            {
                new ChatMessageListItemViewModel
                {
                    SenderName = "Luke",
                    Initials = "LM",
                    Colour = "3099c5",
                    Message = "Hi, how are you?",
                    MessageSentTime = DateTimeOffset.UtcNow,
                    SentByMe = false,
                },

                new ChatMessageListItemViewModel
                {
                    SenderName = "Leon",
                    Initials = "LK",
                    Colour = "77f442",
                    Message = "I'm good how about you?",
                    MessageSentTime = DateTimeOffset.UtcNow,
                    SentByMe = true,
                },

                new ChatMessageListItemViewModel
                {
                    SenderName = "Luke",
                    Initials = "LM",
                    Colour = "3099c5",
                    Message = "I'm not too bad.\r\n How was your weekend?",
                    MessageSentTime = DateTimeOffset.UtcNow,
                    SentByMe = false,
                }
            };
        }
    }
}
