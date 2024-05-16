using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Messenger.ViewModel
{
    internal class ServerViewModel : BindingHelper
    {
        private ObservableCollection<string> usersOrLogs;
        public ObservableCollection<string> UsersOrLogs
        {
            get => usersOrLogs;
            set
            {
                usersOrLogs = value;
                OnPropertyChanged(nameof(UsersOrLogs));
            }
        }

        private ObservableCollection<string> messages;
        public ObservableCollection<string> Messages
        {
            get => messages;
            set
            {
                messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }

        private string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private bool showLogs;
        public bool ShowLogs
        {
            get => showLogs;
            set
            {
                showLogs = value;
                OnPropertyChanged(nameof(ShowLogs));
                UpdateUsersOrLogs();
            }
        }

        private TcpClient client;
        private TcpServer server;

        public ServerViewModel(string name)
        {
            server = new TcpServer();
            client = new TcpClient(name, "127.0.0.1");
            Messages = client.Messages;
            UsersOrLogs = client.Users;
            ShowLogs = false; 
        }

        public void Switch(object sender, RoutedEventArgs e)
        {
            ShowLogs = !ShowLogs;
        }

        private void UpdateUsersOrLogs()
        {
            if (ShowLogs)
            {
                UsersOrLogs = server.Logs;
            }
            else
            {
                UsersOrLogs = client.Users;
            }
        }

        public void Exit(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }


        public async void Send(object sender, RoutedEventArgs e)
        {
            await client.SendMessage(Message);
            Message = "";
        }
    }
}
