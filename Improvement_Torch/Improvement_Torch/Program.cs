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
            Console.WriteLine("RCON is Starting...\n");
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
                string GetNum = Regex.Replace(result, @"[^0-9-,.]", ""); //座標数値のみ検索
                string[] StrArray = GetNum.Split(','); //カンマで区切られた文字を部分的に取り出して、配列に保持

                Console.WriteLine(result);//生のデータ

                double Position_x = double.Parse(StrArray[0]); //南北方向 北へ進むほど数値が小さくなる
                double Position_y = double.Parse(StrArray[1]); //高さ
                double Position_z = double.Parse(StrArray[2]); //東西方向 東へ進むほど数値が大きくなる

                Console.Write("Range in current position to x:");
                int Input_x = int.Parse(Console.ReadLine()); //x座標入力
                Console.Write("Range in current position to y:");
                int Input_y = int.Parse(Console.ReadLine()); //y座標入力
                Console.Write("Range in current position to z:");
                int Input_z = int.Parse(Console.ReadLine()); //z座標入力

                Console.WriteLine("\nSetup to Put Torches Range in:");
                //任意の範囲を指定した後の絶対座標
                ShowRange("x", Position_x, Input_x);
                ShowRange("y", Position_y, Input_y);
                ShowRange("z", Position_z, Input_z);
                
                string SearchItem = "tallgrass";
                string PutItem = "torch";

                for (int x = 1; x <= Input_x; x++) //プレイヤーの相対座標に置く x方向
                {
                    if (x % 6 == 0) //6マスごとに設置
                    {
                        for (int z = 1; z <= Input_z; z++) //プレイヤーの相対座標に置く z方向
                        {
                            if (z % 6 == 0)
                            {
                                for (int y = -10; y < 255 - (int)Position_y; y++) //プレイヤーの10ブロック下からスキャニング(255は限界高度)
                                {
                                    string SearchBlock = "/testforblock " + (Position_x + x) + " " + (Position_y + y) + " " + (Position_z + z) + " " + SearchItem;

                                    result = await connection.SendCommandAsync(SearchBlock); //ブロックの照合を行うコマンド
                                    Console.WriteLine(SearchBlock);
                                    Console.WriteLine(result);
                                    Thread.Sleep(100); //ログ確認用

                                    //草を刈って松明を置くロジック
                                    if (result.Contains("Successfully") || result.Contains("air")) //そのブロックが空気ブロックまたは草(装飾)であるか
                                    {
                                        string Removegrass = "/setblock " + (Position_x + x) + " " + (Position_y + y) + " " + (Position_z + z) + " " + "air";
                                        result = await connection.SendCommandAsync(Removegrass);
                                        Console.WriteLine(result);
                                        Console.WriteLine("草刈ったよ\n");

                                        string SetToaches = "/setblock " + (Position_x + x) + " " + (Position_y + y) + " " + (Position_z + z) + " " + PutItem;
                                        result = await connection.SendCommandAsync(SetToaches);
                                        Console.WriteLine(result); //松明を設置
                                        Console.WriteLine("たいまつおいたよ\n");
                                        break; //松明を設置して終了
                                    }
                                }
                            }
                        }
                    }
                }

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

        static void ShowRange(string Vector, double Pos, int Input)
        {
            int Position = (int)Pos;
            Console.Write(Vector + "= {0} to ", Position); // 現在の座標
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0}", Position + Input); // 範囲指定後の座標
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}