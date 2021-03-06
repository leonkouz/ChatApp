﻿using ChatServer.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ChatApp.Core
{
    /// <summary>
    /// The view model for the messaging section of the MainWindow
    /// </summary>
    public class MessageViewModel : BaseViewModel
    {
        public MessageViewModel()
        {
            SendMessageCommand = new DelegateCommand(SendMessage);
            Message.Received += Message_Received;

        }

        #region Private Fields

        private readonly ObservableCollection<Message> _history = new ObservableCollection<Message>();

        private string _userMessage;

        #endregion

        #region Public Properties

        /// <summary>
        /// Fires each time a message is received from th server
        /// </summary>
        private void Message_Received(object sender, MessageReceivedEventArgs e)
        {
            Message message = new Message(e.Content, e.TimeStamp);
            AddToHistory(message);
        }

        /// <summary>
        /// A list of each message received from the server
        /// </summary>
        public IEnumerable<Message> History
        {
            get { return _history; }
        }

        /// <summary>
        /// Holds the user's message text from the UI
        /// </summary>
        public string UserMessage
        {
            get { return _userMessage; }
            set
            {
                _userMessage = value;
                RaisePropertyChangedEvent(nameof(UserMessage));
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Fires the SendMessage method
        /// </summary>
        public ICommand SendMessageCommand { get; set; }

        #endregion

        /// <summary>
        /// Sends the users message to the server
        /// </summary>
        private void SendMessage()
        {
            string messageContent = _userMessage;
            DateTime timeStamp = DateTime.Now;
            Message message = new Message(messageContent, timeStamp);

            ChatClient.SendMessage(message);

            ClearTextBox();
        }

        /// <summary>
        /// Adds the message to the history list
        /// </summary>
        /// <param name="message">Message received</param>
        private void AddToHistory(Message message)
        {

            Application.Current.Dispatcher.Invoke(delegate // <--- HERE
            {
                _history.Add(message);
            });
        }

        /// <summary>
        /// Clears the users message
        /// </summary>
        private void ClearTextBox()
        {
            Application.Current.Dispatcher.Invoke(delegate // <--- HERE
            {
                UserMessage = "";
            });
        }
    }
}
