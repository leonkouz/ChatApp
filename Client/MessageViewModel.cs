using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client
{
    public class MessageViewModel : ObservableObject
    {
        private readonly ObservableCollection<Message> _history = new ObservableCollection<Message>();

        public string _userMessage;

        public MessageViewModel()
        {
            Message.Received += Message_Received;
        }

        private void Message_Received(object sender, MessageReceivedEventArgs e)
        {
            Message message = new Message(e.Content, e.TimeStamp);
            AddToHistory(message);
        }

        public IEnumerable<Message> History
        {
            get { return _history; }
        }

        public string UserMessage
        {
            get { return _userMessage; }
            set
            {
                _userMessage = value;
            }
        }
        
        public ICommand SendMessageCommand
        {
            get { return new DelegateCommand(SendMessage);}
        }

        private void SendMessage()
        {    
            string messageContent = _userMessage;
            DateTime timeStamp = DateTime.Now;
            Message message = new Message(messageContent, timeStamp);

            ChatClient.SendMessage(message);

            ClearTextBox();
        }

        private void AddToHistory(Message message)
        {
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                _history.Add(message);
                RaisePropertyChangedEvent("History");
            });
        }

        private void ClearTextBox()
        {
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                _userMessage = "";
                RaisePropertyChangedEvent("UserMessage");
            });
        }
    }
}
