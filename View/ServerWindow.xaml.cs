using Messenger.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Messenger
{
    /// <summary>
    /// Логика взаимодействия для ServerWindow.xaml
    /// </summary>
    public partial class ServerWindow : Window
    {

        private bool logs;
        public ServerWindow(string name)
        {
            InitializeComponent();
            DataContext = new ServerViewModel(name);
        }


    }
}
