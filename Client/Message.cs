using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Message
    {
        private string _content;
        private DateTime _timeStamp;
        private string _user;

        private static string delimiter = "|/][-"; //random string of characters to prevent which are unlikely to be used in chat.

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }

        public string User
        {
            get { return _user; }
            set { _user = value; }
        }

        public static event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public Message(string content, DateTime timeStamp, string user)
        {
            _content = content;
            _timeStamp = timeStamp;
            _user = user;
        }

        public static MessageReceivedEventArgs BuildMessageReceivedEvent(Message message)
        {
            MessageReceivedEventArgs args = new MessageReceivedEventArgs();
            args.Content = message.Content;
            args.TimeStamp = message.TimeStamp;
            args.User = message.User;

            return args;
        }
        
        public string BuildTcpString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Content);
            sb.Append(delimiter);
            sb.Append(TimeStamp);
            sb.Append(delimiter);
            sb.Append(User);

            return sb.ToString();
        }

        public static Message BuildMessageFromTcpString(string tcpString)
        {
            string[] arr = tcpString.Split(new string[] { delimiter }, StringSplitOptions.None);

            string content = arr[0];

            DateTime timeStamp;
            try
            {
                timeStamp = DateTime.Parse(arr[1]);
            }
            catch
            {
                throw new ArgumentException("Unable to parse string as DateTime");
            }

            string user = arr[2];

            Message message = new Message(content, timeStamp, user);
            return message;
        }

        public void OnMessageReceived(MessageReceivedEventArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }
        public string User { get; set; }
    }
}
