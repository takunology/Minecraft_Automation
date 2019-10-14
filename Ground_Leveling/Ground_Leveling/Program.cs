using CoreRCON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Ground_Leveling
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("RCON is starting...");
            await Minecraft_Automation();
        }

        static async Task Minecraft_Automation()
        {
            var ServerAddrss = IPAddress.Parse("127.0.0.1");
            var ServerPass = "minecraft";
            ushort Port = 25575;
            var Connection = new RCON(ServerAddrss, Port, ServerPass);

            try
            {
                string PlayerName = "Player_Name";
                string TPCommand = "/tp " + PlayerName + " ~ ~ ~"; //プレイヤーの位置情報を得るためのコマンド
                var result = await Connection.SendCommandAsync(TPCommand);
                string GetNum = Regex.Replace(result, @"[^0-9-,.]", ""); //座標数値のみ検索
                string[] StrArray = GetNum.Split(','); //カンマで区切られた文字を部分的に取り出して、配列に保持
                double Position_x = double.Parse(StrArray[0]); //南北方向 北へ進むほど数値が小さくなる
                double Position_y = double.Parse(StrArray[1]); //高さ
                double Position_z = double.Parse(StrArray[2]); //東西方向 東へ進むほど数値が大きくなる
                string SearchBlock = "";
                string SetBlock = "";

                //Position_x = Position_x + 0.9; //小数点切り捨て対策
                //Position_y = Position_y + 0.9;
                //Position_z = Position_z + 0.9;

                Console.Write("Range in current position to x:");
                int Input_x = int.Parse(Console.ReadLine()); //x座標入力
                //Console.Write("Range in current position to y:");
                //int Input_y = int.Parse(Console.ReadLine()); //y座標入力
                Console.Write("Range in current position to z:");
                int Input_z = int.Parse(Console.ReadLine()); //z座標入力
                Console.WriteLine("\nSetup to Range in:");
                //任意の範囲を指定した後の絶対座標
                ShowRange("x", Position_x, Input_x);
                //ShowRange("y", Position_y, 0);
                ShowRange("z", Position_z, Input_z);

                //高さの自動取得
                int HighestY = 0;
                int HighestX = 0, HighestZ = 0;
                for (int x = 1; x <= Input_x; x++)
                {
                    for (int z = 1; z <= Input_z; z++)
                    {
                        Console.WriteLine("Now (X:{0}, Z:{1})", (Position_x + x), (Position_z + z));
                        for (int y = 255 - (int)Position_y; y > 0; y--) //空から探索
                        {
                            SearchBlock = "/testforblock " + (Position_x + x) + " " + (Position_y + y) + " " + (Position_z + z) + " " + "air";
                            result = await Connection.SendCommandAsync(SearchBlock); //ブロックの照合を行うコマンド
                            if (result.Contains("Successfully")) { }//空気ブロックだった場合
                            else //何かしらのブロックだった場合
                            {
                                if (HighestY < (int)Position_y + y) //今までの値より大きければ記録更新
                                {
                                    HighestY = (int)Position_y + y;
                                    HighestX = (int)Position_x + x; //最も高い地点のX座標
                                    HighestZ = (int)Position_z + z; //最も高い地点のZ座標
                                }
                                //Console.WriteLine(result);
                                Console.WriteLine("Now Highest:{0}", HighestY);
                                Thread.Sleep(10);
                                break; //探索中断
                            }
                        }
                    }
                }
                Console.WriteLine("\nHighest:{0}", HighestY);

                //自動整地
                Console.WriteLine("Start Ground Leveling. Press any key...\n");
                Console.ReadKey();
                for (int x = 1; x <= Input_x; x++)
                {
                    for (int z = 1; z <= Input_z; z++)
                    {
                        for (int y = 0; y < HighestY - (int)Position_y; y++) 
                        {
                            SetBlock = "/setblock " + (Position_x + x) + " " + (Position_y + y) + " " + (Position_z + z) + " " + "air";
                            result = await Connection.SendCommandAsync(SetBlock);
                            Console.WriteLine(result);
                        }
                    }
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Done.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
