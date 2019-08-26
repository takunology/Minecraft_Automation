using CoreRCON;
using System;
using System.Net;
using System.Text.RegularExpressions;
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

            string Player_Name = "Player_Name";
            string TPCommand = "/tp " + Player_Name + " ~ ~ ~"; //プレイヤーの位置情報を得るためのコマンド

            //CoreRCONを使ってMinecraftへ接続
            try
            {
                // Minecraft 接続環境(固定)
                var connection = new RCON(serveraddress, port, serverpass);
                var result = await connection.SendCommandAsync(TPCommand);

                //相対座標取得
                string GetNum = Regex.Replace(result, @"[^0-9,.]", ""); //座標数値のみ検索
                string[] StrArray = GetNum.Split(','); //カンマで区切られた文字を部分的に取り出して、配列に保持

                Console.WriteLine(result);//生のデータ

                double Position_x = double.Parse(StrArray[0]); //南北方向 北へ進むほど数値が小さくなる
                double Position_y = double.Parse(StrArray[1]); //高さ
                double Position_z = double.Parse(StrArray[2]); //東西方向 東へ進むほど数値が大きくなる

                Console.Write("Move x:");
                double Input_x = double.Parse(Console.ReadLine()); //x座標入力
                Console.Write("Move y:");
                double Input_y = double.Parse(Console.ReadLine()); //y座標入力
                Console.Write("Move z:");
                double Input_z = double.Parse(Console.ReadLine()); //z座標入力

                double Teleport_x = Position_x + Input_x; //移動後の座標x
                double Teleport_y = Position_y + Input_y; //移動後の座標y
                double Teleport_z = Position_z + Input_z; //移動後の座標z

                string Telepote_Command = "/tp " + Player_Name + " " + Teleport_x + " " + Teleport_y + " " + Teleport_z;

                result = await connection.SendCommandAsync(Telepote_Command);

                Console.WriteLine(Telepote_Command); //テレポート後のログ

                //完了コマンド(固定)
                Console.ForegroundColor = ConsoleColor.Green; //コンソール文字列に色付け
                Console.WriteLine("\nDone."); //ちょっとカッコつけて完了の表示
                Console.ForegroundColor = ConsoleColor.White; //色を戻す

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}


