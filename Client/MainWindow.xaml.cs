using System;
using System.Collections.Generic;
using System.Linq;
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

        TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            readData = "Conected to Chat Server ...";
            msg();
            clientSocket.Connect("127.0.0.1", 8888);
            serverStream = clientSocket.GetStream();

            byte[] outStream = Encoding.ASCII.GetBytes(textBox.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            Thread ctThread = new Thread(getMessage);
            ctThread.Start();

        }

        private void getMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                int buffSize = 0;
                byte[] inStream = new byte[65536];
                buffSize = clientSocket.ReceiveBufferSize;
                serverStream.Read(inStream, 0, buffSize);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                readData = "" + returndata;
                msg();
            }
        }

        private void msg()
        {
            this.Dispatcher.Invoke(() =>
            {
                textBox.Text = textBox.Text + Environment.NewLine + " >> " + readData;
            });
        }

    }
}
