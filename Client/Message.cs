﻿using System;
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
        private string _user = ChatClient.UserName;

        private static string delimiter = "\0"; //this character is used as it cannot be typed by a user

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

        public static event EventHandler<MessageReceivedEventArgs> Received;

        public Message(string content, DateTime timeStamp)
        {
            _content = content;
            _timeStamp = timeStamp;
        }

        public static MessageReceivedEventArgs BuildMessageReceivedEvent(Message message)
        {
            MessageReceivedEventArgs args = new MessageReceivedEventArgs();
            args.Content = message.Content;
            args.TimeStamp = message.TimeStamp;

            return args;
        }
        
        public string BuildTcpString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_content);
            sb.Append(delimiter); 
            sb.Append(_timeStamp);
            sb.Append(delimiter);
            sb.Append(_user);

            return sb.ToString();
        }

        public static Message BuildMessageFromTcpString(string tcpString)
        {
            string[] result = tcpString.Split(new string[] { delimiter }, StringSplitOptions.None);

            string content = result[0];

            DateTime timeStamp;
            try
            {
                timeStamp = DateTime.Parse(result[1]);
            }
            catch
            {
                throw new ArgumentException("Unable to parse string as DateTime");
            }

            string user = result[2];

            Message message = new Message(content, timeStamp);
            return message;
        }

        public void OnMessageReceived(MessageReceivedEventArgs e)
        {
            Received?.Invoke(this, e);
        }
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}