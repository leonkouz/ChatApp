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

        public MainWindow()
        {
            InitializeComponent();

            //_user = userName;

            this.Loaded += MainWindow_Loaded;

            this.DataContext = new WindowViewModel(this);

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                ChatClient.Connect(_user);
            }
            catch
            {
                MessageBox.Show("Was unable to connect to server");
            }
            
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
