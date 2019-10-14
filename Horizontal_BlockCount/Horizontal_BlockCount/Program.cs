using CoreRCON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Horizontal_BlockCount
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

                int GrassBlock = 0;
                int Stone = 0;
                int Dirt = 0;
                int Other = 0;
                int Total = 0;

                for (int x = 1; x <= Input_x; x++)
                {
                    for(int z = 1; z <= Input_z; z++)
                    {
                        for(int y = 0; y < Input_y; y++)
                        {
                            string SearchBlock = "/testforblock " + (Position_x + x) + " " + (Position_y + y) + " " + (Position_z + z) + " " + "air";
                            result = await Connection.SendCommandAsync(SearchBlock); //ブロックの照合を行うコマンド
                            if (result.Contains("Successfully")){ //空気ブロックは加算しない
                                Console.WriteLine(SearchBlock);
                            }
                            else
                            {
                                Console.WriteLine(SearchBlock);
                                Console.WriteLine(result);
                                if (result.Contains("Grass")) { GrassBlock++; Total++; }
                                else if (result.Contains("Stone")) { Stone++; Total++; }
                                else if (result.Contains("Dirt")) { Dirt++; Total++; }
                                else { Other++; Total++; }
                            }
                            
                            //Thread.Sleep(100); //ログ確認用
                        }
                    }
                }
                Console.WriteLine("\nGrass:{0} Stone:{1} Dirt:{2} Other:{3} Total:{4}", GrassBlock, Stone, Dirt, Other, Total);

                //完了コマンド(固定)
                Console.ForegroundColor = ConsoleColor.Green; //コンソール文字列に色付け
                Console.WriteLine("Done."); //ちょっとカッコつけて完了の表示
                Console.ForegroundColor = ConsoleColor.White; //色を戻す

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
