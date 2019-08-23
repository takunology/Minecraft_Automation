using CoreRCON;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Now Starting...");
            await minecraft();
        }

        static async Task minecraft()
        {
            var serveraddress = IPAddress.Parse("127.0.0.1"); //IPアドレスとして扱うための変換
            var serverpass = "minecraft"; //RCONでログインするためのパスワード
            ushort port = 25575; //サーバのポート番号

            var command = "/weather clear"; // コマンド

            //CoreRCONを使ってMinecraftへ接続
            try
            {
                //var connection = new RCON(serveraddress, port, serverpass);
                var connection = new RCON(serveraddress, port, serverpass);
                //コマンドの送信
                var result = await connection.SendCommandAsync(command);
                //ログの受信
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }
        }
    }
}

