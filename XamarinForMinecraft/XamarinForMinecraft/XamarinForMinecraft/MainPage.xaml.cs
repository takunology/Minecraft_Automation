using CoreRCON;
using System;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinForMinecraft
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked (object sender, EventArgs e)
        {
            MinecraftCommand mc = new MinecraftCommand();
           
            await mc.Execute(your_ip.Text, command.Text);

            Loglabel.Text = $"{System.DateTime.Now} \n {mc.logMessage}";
        }

    }

    public partial class MinecraftCommand
    {
        public string logMessage;

        public async Task Execute(string ipAddress, string mcCommand)
        {
            await CommandSend(ipAddress, mcCommand);
        }

        private async Task CommandSend(string ipAddress, string mcCommand)
        {
            try
            {
                var serveraddress = IPAddress.Parse(ipAddress); //IPアドレスとして扱うための変換
                var serverpass = "minecraft"; //RCONでログインするためのパスワード
                ushort port = 25575; //RCONのポート番号

                var rcon = new RCON(serveraddress, port, serverpass);
                await rcon.ConnectAsync(); //接続
                var result = await rcon.SendCommandAsync(mcCommand);

                logMessage = result;
            }
            catch (Exception ex)
            {
                logMessage = ex.Message;
            }
        }
    }
}
