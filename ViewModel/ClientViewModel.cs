using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger.ViewModel
{
    internal class ClientViewModel
    {
        private ObservableCollection<string> messages;
        public ObservableCollection<string> Messages
        {
            get => messages; set => messages = value;
        }

        private ObservableCollection<string> users;
        public ObservableCollection<string> Users
        {
            get => users; set => users = value;
        }

        private string message;
        public string Message
        {
            get => message; set => message = value;
        }

        private TcpClient client;

        public ClientViewModel(string name, string ip)
        {
            client = new TcpClient(name, ip);
            Messages = client.Messages;
            Users = client.Users;
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