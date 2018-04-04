using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client
{
    public class MessageViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Message> _history = new ObservableCollection<Message>();

        public string _userMessage;

        public MessageViewModel()
        {
            Message.Received += Message_Received;
        }

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
        
        /// <summary>
        /// Fires the SendMessage method
        /// </summary>

        public ICommand SendMessageCommand
        {
            get { return new DelegateCommand(SendMessage);}
        }

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
            App.Current.Dispatcher.Invoke(delegate // <--- HERE
            {
                _history.Add(message);
            });
        }

        /// <summary>
        /// Clears the users message
        /// </summary>

        private void ClearTextBox()
        {
            App.Current.Dispatcher.Invoke(delegate // <--- HERE
            {
                UserMessage = "";
            });
        }
    }
}
