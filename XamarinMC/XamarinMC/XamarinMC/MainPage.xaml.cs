using CoreRCON;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinMC
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            MinecraftAutomation mcAuto = new MinecraftAutomation();
            //Func<Task> function = async() => await mcAuto.Run();
            await mcAuto.Run(your_ip.Text, command.Text);

            Loglabel.Text = mcAuto.log; //ログ
        }
    }

    public class MinecraftAutomation
    {
        public string log;

        public async Task Run(string your_ip, string command)
        {
            await Execute(your_ip, command);
        }

        private async Task Execute(string your_ip, string command)
        {

            var serveraddress = IPAddress.Parse(your_ip); //IPアドレスとして扱うための変換
            var serverpass = "minecraft"; //RCONでログインするためのパスワード
            ushort port = 25575; //サーバのポート番号

            try
            {
                var rcon = new RCON(serveraddress, port, serverpass);
                await rcon.ConnectAsync(); //接続
                var result = await rcon.SendCommandAsync(command);

                log = result;
            }
            catch (Exception e)
            {
                log = e.ToString();
            }
        }
    }

}
