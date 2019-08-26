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
                string GetNum = Regex.Replace(result, @"[^0-9-,.]", ""); //座標数値のみ検索
                string[] StrArray = GetNum.Split(','); //カンマで区切られた文字を部分的に取り出して、配列に保持

                Console.WriteLine(result);//生のデータ

                double Position_x = double.Parse(StrArray[0]); //南北方向 北へ進むほど数値が小さくなる
                double Position_y = double.Parse(StrArray[1]); //高さ
                double Position_z = double.Parse(StrArray[2]); //東西方向 東へ進むほど数値が大きくなる

                Console.Write("Range in current position to x:");
                double Input_x = double.Parse(Console.ReadLine()); //x座標入力
                Console.Write("Range in current position to y:");
                double Input_y = double.Parse(Console.ReadLine()); //y座標入力
                Console.Write("Range in current position to z:");
                double Input_z = double.Parse(Console.ReadLine()); //z座標入力

                Console.WriteLine("\nSetup to Put Torches Range in:");

                int Range_x = (int)Absolute_Range(Position_x, Input_x); //移動後の座標x
                int Range_y = (int)Absolute_Range(Position_y, Input_y); //移動後の座標y
                int Range_z = (int)Absolute_Range(Position_z, Input_z); //移動後の座標z

                string Item = "torch";

                Console.WriteLine("Range x:{0} Range y:{1} Range z:{2}", Range_x, Range_y, Range_z); //範囲の確認

                for (int x = 1; x <= Range_x; x++) //プレイヤーの相対座標に置く x方向
                {
                    if (x % 6 == 0)
                    {
                        Console.WriteLine("Set (0, {0})", x);
                        string SetToaches = "/setblock " + (Position_x + x) + " " + (Position_y) + " " + (Position_z) + " " + Item;
                        Console.WriteLine(SetToaches);
                        result = await connection.SendCommandAsync(SetToaches);
                        Console.WriteLine(result);
                    }
                }

                for (int z = 1; z <= Range_z; z++) //プレイヤーの相対座標に置く z方向
                {
                    if (z % 6 == 0)
                    {
                        Console.WriteLine("Set (0, {0})", z);
                        string SetToaches = "/setblock " + (Position_x) + " " + (Position_y) + " " + (Position_z + z) + " " + Item;
                        Console.WriteLine(SetToaches);
                        result = await connection.SendCommandAsync(SetToaches);
                        Console.WriteLine(result);
                    }
                }

                for (int x = 1; x <= Range_x; x++) //6マス離れた後の座標 以降6マスごとにたいまつを配置
                {
                    if(x % 6 == 0)
                    {
                        for (int z = 1; z <= Range_z; z++)
                        {
                            if(z % 6 == 0)
                            {
                                Console.Write("Set ({0}, {1})\n", x, z);
                                string SetToaches = "/setblock " + (Position_x + x) + " " + (Position_y) + " " + (Position_z + z) + " " + Item;
                                Console.WriteLine(SetToaches);
                                result = await connection.SendCommandAsync(SetToaches);
                                Console.WriteLine(result);
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

        static double Absolute_Range(double Position, double Input) //絶対値に変換
        {
            if (Position - Input < 0)
            {
                return (Position - Input) * (-1) + 0.9;
            }
            return Position - Input;
        }
    }
}
