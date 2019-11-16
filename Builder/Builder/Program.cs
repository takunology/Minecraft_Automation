using CoreRCON;
using NPOI.SS.UserModel;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Builder
{
    class Program : ConvertFromExcel
    {
        //static ConvertFromExcel convertFromExcel = new ConvertFromExcel();
        static async Task Main(string[] args)
        {
            OpenExcelFile(); //Excelファイルを開く
            Convert(); //得られたデータ
            ShowSheet();//読み込んだシートを見る

            try
            {
                await Automation();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        static async Task Automation()
        {
            var ServerAddrss = IPAddress.Parse("127.0.0.1");
            var ServerPass = "minecraft";
            ushort Port = 25575;
            var Connection = new RCON(ServerAddrss, Port, ServerPass);

            string PlayerName = "Prau_Splacraft";
            string TPCommand = "/tp " + PlayerName + " ~ ~ ~"; //プレイヤーの位置情報を得るためのコマンド
            var result = await Connection.SendCommandAsync(TPCommand);

            string GetNum = Regex.Replace(result, @"[^0-9-,.]", ""); //座標数値のみ検索
            string[] StrArray = GetNum.Split(','); //カンマで区切られた文字を部分的に取り出して、配列に保持
            double Position_x = double.Parse(StrArray[0]); //南北方向 北へ進むほど数値が小さくなる
            double Position_y = double.Parse(StrArray[1]); //高さ
            double Position_z = double.Parse(StrArray[2]); //東西方向 東へ進むほど数値が大きくなる

            Position_y = 65;

            ShowRange("x", Position_x, Value.GetLength(1));
            ShowRange("y", Position_y, Value.GetLength(0));
            ShowRange("z", Position_z, Value.GetLength(2));

            for (int y = 0; y < Value.GetLength(0); y++)
            {
                for (int x = 0; x < Value.GetLength(1); x++)
                {
                    for (int z = 0; z < Value.GetLength(2); z++)
                    {
                        string SetBlock = $"/setblock { Position_x + x} {Position_y + y} { Position_z + z} {Value[y, x, z]}";
                        result = await Connection.SendCommandAsync(SetBlock);
                        Console.WriteLine(result);
                        await Task.Delay(5);
                    }
                }
            }
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
