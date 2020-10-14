using CoreRCON;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Uno_Minecraft.Shared
{
    class AutomationApps
    {
        public string log;

        public async Task Run()
        {
            await Automation();
        }

        private async Task Automation()
        {
            var serveraddress = IPAddress.Parse("127.0.0.1"); //IPアドレスとして扱うための変換
            var serverpass = "minecraft"; //RCONでログインするためのパスワード
            ushort port = 25575; //サーバのポート番号

            var command = "/time set 0"; // コマンド

            //CoreRCONを使ってMinecraftへ接続
            try
            {
                //var connection = new RCON(serveraddress, port, serverpass);
                var connection = new RCON(serveraddress, port, serverpass);
                //コマンドの送信
                var result = await connection.SendCommandAsync(command);
                //コマンドの受信
                log = result;
            }
            catch (Exception e)
            {
                log = e.ToString();
            }
        }

    }
}
