using CoreRCON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFforMinecraft
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += (o, e) => DragMove();

        }


        async public void Command_KeyDown(object sender, KeyEventArgs e)
        {
            SendCommand sendCommand = new SendCommand();
            if (e.Key == Key.Enter)
            {
                //Send(Command.Text);
                Send();
                
            }
        }

        static async Task Send()
        {
            var ServerAddress = IPAddress.Parse("127.0.0.1");
            var ServerPass = "minecraft";
            ushort ServerPort = 25575;
            var mainWindow = (MainWindow)App.Current.MainWindow;

            try
            {
                var Connection = new RCON(ServerAddress, ServerPort, ServerPass);
                var Result = await Connection.SendCommandAsync("/time set 0");
                mainWindow.ConsoleLog.Text = Result.ToString();
                //await Task.Delay(100);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                mainWindow.ConsoleLog.Text = e.ToString();
            }
        }

    }
}
