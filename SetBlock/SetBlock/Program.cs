using CoreRCON;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
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
                var connection = new RCON(serveraddress, port, serverpass);
                var result = await connection.SendCommandAsync(TPCommand);

                string GetNum = Regex.Replace(result, @"[^0-9-,.]", ""); //座標数値のみ検索
                string[] StrArray = GetNum.Split(','); //カンマで区切られた文字を部分的に取り出して、配列に保持

                Console.WriteLine(result);//生のデータ
                Console.WriteLine(GetNum + "\n");//正規表現にてフィルタされた値の確認

                foreach(string str in StrArray)
                {
                    Console.WriteLine(str);//配列で保持された座標データの確認（上からx, y, z座標）
                }

                double Position_x = double.Parse(StrArray[0]); //南北方向 北へ進むほど数値が小さくなる
                double Position_y = double.Parse(StrArray[1]); //高さ
                double Position_z = double.Parse(StrArray[2]); //東西方向 東へ進むほど数値が大きくなる

                double x = Position_x, y = Position_y, z = Position_z; //配置するために使用する座標

                //ブロック名(grass:草ブロック, glass:ガラスブロック, air:空気(削除) etc...)
                string Block_Name = "grass";
                //ブロックを置くコマンド(1個だけ配置)
                //string SetBlock = "/setblock " + Position_x.ToString() + " " + Position_y.ToString() + " " + Position_z.ToString() + " " + Block_Name;
                
                // 線形(x方向)にブロックを配置する
                /*for(int i = 0; i < 10; i++)
                {
                    x++;
                    string SetBlock = "/setblock " + x.ToString() + " " + y.ToString() + " " + z.ToString() + " " + Block_Name;

                    //コマンド送信
                    result = await connection.SendCommandAsync(SetBlock);
                    Console.WriteLine(result);
                }*/

                //2次元方向(x, z方向)にブロックを配置する
                for (int i = 0; i < 50; i++)
                {
                    x++;
                    for (int j = 0; j < 50; j++)
                    {
                        z++;
                        string SetBlock = "/setblock " + x.ToString() + " " + y.ToString() + " " + z.ToString() + " " + Block_Name;
                        result = await connection.SendCommandAsync(SetBlock);
                        Console.WriteLine(result);
                        Thread.Sleep(5); //5ミリ秒間隔で、鯖落ち防止
                    }
                    z = Position_z; //位置情報を修正(初期化)
                }
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

