using Messenger.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger
{
    public class MainViewModel: BindingHelper
    {
        private string ip;
        public string Ip
        {
            get => ip; set => ip = value;
        }

        private string nick;
        public string Nick
        {
            get => nick; set => nick = value;
        }

        public MainViewModel()
        {

        }

        public void NewChat(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Nick))
            {
                ServerWindow w = new ServerWindow(Nick);
                w.Show();
            } else
            {
                MessageBox.Show("Имя не заполнено");
            }
        }

        public void Connect(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Nick) && !string.IsNullOrEmpty(Ip))
            {
                PingReply reply;
                try
                {
                    if (!string.IsNullOrEmpty(Ip))
                    {
                        var ping = new Ping();
                        reply = ping.Send(Ip);
                        if (reply.Status == IPStatus.TimedOut)
                        {
                            throw new Exception();
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Неизвестный IP");
                    return;
                }
                ClientWindow w = new ClientWindow(Nick, Ip);
                w.Show();
            } else
            {
                if (string.IsNullOrEmpty(Nick)) MessageBox.Show("Имя должно быть заполнено");
                else if (string.IsNullOrEmpty(Ip)) MessageBox.Show("IP должно быть заполнено");
            }
        }
    }
}
