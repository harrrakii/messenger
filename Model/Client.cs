using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Model
{
    public class Client
    {
        public Socket socket {  get; set; }
        public string name { get; set; }
        public Client(Socket socket, string name)
        {
            this.socket = socket;
            this.name = name;
        }   
    }
}
