using Client.ViewModels;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //https://www.youtube.com/watch?v=vaeg0Gwzybw
        private string _user;

        public MainWindow(string userName)
        {
            InitializeComponent();

            this.DataContext = new WindowViewModel(this);

            _user = userName;

            this.Loaded += MainWindow_Loaded;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ChatClient.Connect(_user);
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
