using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ChatClient _client;

        List<string> messageHistory = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            Message.Received += Message_Received;
        }

        private void Message_Received(object sender, MessageReceivedEventArgs e)
        {
            messageHistory.Add(e.Content + "\n");

            this.Dispatcher.Invoke(() =>
            {
                string messageLog = null;

                foreach(string str in messageHistory)
                {
                    messageLog = messageLog + str + "\n";
                
                };

                messageHistoryTextBlock.Text = messageLog;

            });
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _client = new ChatClient();
            _client.Connect();
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (usernameTextbox.Text != "")
            {
                string messageContent = messageEntryTextBox.Text;
                DateTime timeStamp = DateTime.Now;
                string user = usernameTextbox.Text;
                Message message = new Message(messageContent, timeStamp, user);

                _client.SendMessage(message);
            }
        }
    }
}
