﻿using ChatApp.Core;
using System.Windows;

namespace ChatApp
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

            Loaded += MainWindow_Loaded;

            DataContext = new WindowViewModel(this);
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
