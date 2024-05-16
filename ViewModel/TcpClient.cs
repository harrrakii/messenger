using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Messenger
{
    public class TcpClient
    {
        private Socket server;
        public ObservableCollection<string> Messages { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Users { get; set; } = new ObservableCollection<string>();
        public CancellationTokenSource TokenClient;

        public TcpClient(string name, string ip)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Connect(ip, 1111);
            SendMessage(name);
            TokenClient = new CancellationTokenSource();
            RecieveMessage(TokenClient.Token);
        }

        public async Task SendMessage(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            await server.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
        }

        private async Task RecieveMessage(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var bytes = new byte[1024];
                await server.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                var sortByte = bytes.Where(item => item != 0).ToArray();
                var message = Encoding.UTF8.GetString(sortByte);
                if (!message.Contains("user"))
                {
                    Messages.Add(message);
                }
                else
                {
                    var usersList = message.Split(':')[1].Split('/');
                    Users.Clear();
                    foreach (var user in usersList)
                    {
                        if (!string.IsNullOrEmpty(user))
                        {
                            Users.Add(user);
                        }
                    }
                }
            }

            server.Close();
        }
    }
}
