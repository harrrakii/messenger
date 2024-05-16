using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Messenger.Model;
using System.Configuration;

namespace Messenger
{
    public class TcpServer
    {
        public readonly Socket socket;
        public Dictionary<Client, CancellationTokenSource> Clients = new Dictionary<Client, CancellationTokenSource>();
        public ObservableCollection<string> Logs = new ObservableCollection<string>();
        public CancellationTokenSource MainToken;
        public ObservableCollection<string> Users { get; set; } = new ObservableCollection<string>();


        public TcpServer()
        {
            var ipAddress = new IPEndPoint(IPAddress.Any, 1111);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipAddress);
            socket.Listen(100);
            MainToken = new CancellationTokenSource();
            ListenToClient(MainToken.Token);
        }

        private async Task ListenToClient(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var clientSocket = await socket.AcceptAsync();

                var bytes = new byte[1024];
                await clientSocket.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                var sortByte = bytes?.Where(x => x != 0).ToArray();
                var name = Encoding.UTF8.GetString(sortByte);
                if (name != "/disconnect")
                {
                    var client = new Client(clientSocket, name);
                    Clients.Add(client, new CancellationTokenSource());
                    Users.Add(client.name); 
                    string users = "user:";
                    foreach (var item in Clients.Keys) users += $"{item.name.ToString()}/";
                    SendMessage(clientSocket, users);
                    Logs.Add($"{client.name} - подключился\n{DateTime.Now.ToString()}");
                    ReceiveMessage(client, Clients[client].Token);
                }
            }
        }


        private async Task ReceiveMessage(Client client, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var bytes = new byte[1024];
                await client.socket.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                var sortByte = bytes?.Where(x => x != 0).ToArray();
                var message = Encoding.UTF8.GetString(sortByte);
                if (message == "/disconnect")
                {
                    Clients[client].Cancel();
                }
                SendMessage(client.socket, $"{client.name}: {DateTime.Now.ToString()} - {message}");
            }

            Clients.Remove(client);
            Logs.Add($"{client.name} - отключился\n{DateTime.Now.ToString()}");
        }



        private async Task SendMessage(Socket client, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            foreach (var item in Clients.Keys) await item.socket.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
        }

    }
}
