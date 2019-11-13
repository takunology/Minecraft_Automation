using System;

namespace MazeGenerator
{
    class Program
    {
        static string[,] Maze = new string[0, 0];

        static void Main(string[] args)
        {
            Input(); //入力
            WallGenerate(); //壁生成
            Generate(); //迷路生成
            Check(); //修正
            //ShowMaze(); //最終完成図
            Console.WriteLine("迷路完成！");
        }

        static void Input()
        {
            while (true)
            {
                Console.Write("X：");
                int X = int.Parse(Console.ReadLine());
                //int X = 21;
                Console.Write("Z：");
                int Y = int.Parse(Console.ReadLine());
                //int Y = 21;

                if (X % 2 == 1 && Y % 2 == 1)
                {
                    Maze = new string[X, Y];
                    Console.WriteLine();
                    break;
                }
                else { Console.WriteLine("奇数で。"); }
            }
        }

        static void WallGenerate()
        {
            for (int x = 0; x < Maze.GetLength(0); x++)
            {
                if (x == 0 || x == (Maze.GetLength(0) - 1)) //最初または最後の行は壁にする
                {
                    for (int y = 0; y < Maze.GetLength(1); y++)
                    {
                        Maze[x, y] = "■";
                    }
                }
                else
                {
                    for (int y = 0; y < Maze.GetLength(1); y++)
                    {
                        if (y == 0 || y == (Maze.GetLength(1) - 1)) //最初または最後の列は壁にする
                        {
                            Maze[x, y] = "■";
                        }
                        else //最初と最後を除く列は空欄
                        {
                            Maze[x, y] = "　";
                        }
                    }
                }
            }
        }

        static void ShowMaze()
        {
            for (int x = 0; x < Maze.GetLength(0); x++)
            {
                for (int y = 0; y < Maze.GetLength(1); y++)
                {
                    Console.Write(Maze[x, y]);
                }
                Console.WriteLine();
            }
        }

        static int Direction() //方向を決める
        {
            Random rnd = new Random();
            int direction = rnd.Next(0, 4);
            return direction;
        }

        /// <summary>生成ロジック
        /// 開始座標を乱数で得る
        /// 開始座標が偶数でなければやり直す
        /// 
        /// 進む方向を乱数で得る
        /// 乱数生成 0, 1, 2, 3 の順で上下左右とする
        /// 
        /// 1つ先と2つ先の座標が壁でないかを見る
        /// 隣接したとその隣の座標が壁でないなら壁にする
        /// 一度進んだら戻らないようにする
        /// 壁に当たるまで伸ばし続ける
        /// 作成してきた壁の座標履歴を格納
        /// その履歴に重ならないようにする
        /// </summary>
        static void Generate()
        {
            int PositionX; //開始座標
            int PositionY; //開始座標
            int count = 0;
            Random rnd = new Random();
        ReGenerate:
            while (true)
            {
                PositionX = rnd.Next(2, (Maze.GetLength(0) - 2));
                PositionY = rnd.Next(2, (Maze.GetLength(1) - 2));
                count++;
                if (PositionX % 2 == 0 && PositionY % 2 == 0)
                {//偶数になるまで再抽選
                    if (Maze[PositionX, PositionY] != "■") //その座標に壁が存在しないか
                        break;
                }
                else if (count > 1000)
                {
                    return;
                }

            }
            int direction = Direction();

            while (PositionX != 0 || PositionY != 0 || PositionX != (Maze.GetLength(0) - 1) || PositionY != (Maze.GetLength(1)))
            {
                direction = Direction();
                Maze[PositionX, PositionY] = "■";

                switch (direction)
                {
                    case 0://上
                        if ((Maze[PositionX - 1, PositionY] == "　") && (Maze[PositionX - 2, PositionY] == "　"))
                        { //隣接が通路かつその隣も通路
                            Maze[PositionX - 1, PositionY] = "■";
                            Maze[PositionX - 2, PositionY] = "■";
                            PositionX -= 2;
                        }
                        else if (PositionX - 2 == 0) //もし現在の座標の2マス上が壁だったら、1マス上を壁にする
                        {//壁に隣接した部分を壁にしたので作り直し
                            Maze[PositionX - 1, PositionY] = "■";
                            break;
                        }
                        else if ((Maze[PositionX - 2, PositionY] == "■") && (Maze[PositionX + 2, PositionY] == "■") && (Maze[PositionX, PositionY - 2] == "■") && (Maze[PositionX, PositionY + 2] == "■"))
                        { //うずまき対策
                            goto ReGenerate;
                        }
                        else
                        {
                            break;
                        }
                        break;

                    case 1://下
                        if ((Maze[PositionX + 1, PositionY] == "　") && (Maze[PositionX + 2, PositionY] == "　"))
                        {
                            Maze[PositionX + 1, PositionY] = "■";
                            Maze[PositionX + 2, PositionY] = "■";
                            PositionX += 2;
                        }
                        else if (PositionX + 2 == (Maze.GetLength(0) - 1)) //もし現在の座標の2マス下が壁だったら、1マス下を壁にする
                        {//壁に隣接した部分を壁にしたので作り直し
                            Maze[PositionX + 1, PositionY] = "■";
                            break;
                        }
                        else if ((Maze[PositionX - 2, PositionY] == "■") && (Maze[PositionX + 2, PositionY] == "■") && (Maze[PositionX, PositionY - 2] == "■") && (Maze[PositionX, PositionY + 2] == "■"))
                        {
                            goto ReGenerate;
                        }
                        else
                        {
                            break;
                        }
                        break;

                    case 2://左
                        if ((Maze[PositionX, PositionY - 1] == "　") && (Maze[PositionX, PositionY - 2] == "　"))
                        {
                            Maze[PositionX, PositionY - 1] = "■";
                            Maze[PositionX, PositionY - 2] = "■";
                            PositionY -= 2;
                        }
                        else if (PositionY - 2 == 0) //もし現在の座標の2マス左が壁だったら、1マス左を壁にする
                        {//壁に隣接した部分を壁にしたので作り直し
                            Maze[PositionX, PositionY - 1] = "■";
                            break;
                        }
                        else if ((Maze[PositionX - 2, PositionY] == "■") && (Maze[PositionX + 2, PositionY] == "■") && (Maze[PositionX, PositionY - 2] == "■") && (Maze[PositionX, PositionY + 2] == "■"))
                        {
                            goto ReGenerate;
                        }
                        else
                        {
                            break;
                        }
                        break;

                    case 3://右
                        if ((Maze[PositionX, PositionY + 1] == "　") && (Maze[PositionX, PositionY + 2] == "　"))
                        {
                            Maze[PositionX, PositionY + 1] = "■";
                            Maze[PositionX, PositionY + 2] = "■";
                            PositionY += 2;
                        }
                        else if (PositionY + 2 == (Maze.GetLength(1) - 1)) //もし現在の座標の2マス右が壁だったら、1マス右を壁にする
                        {//壁に隣接した部分を壁にしたので作り直し
                            Maze[PositionX, PositionY + 1] = "■";
                            break;
                        }
                        else if ((Maze[PositionX - 2, PositionY] == "■") && (Maze[PositionX + 2, PositionY] == "■") && (Maze[PositionX, PositionY - 2] == "■") && (Maze[PositionX, PositionY + 2] == "■"))
                        {
                            goto ReGenerate;
                        }
                        else
                        {
                            break;
                        }
                        break;
                }
                Console.Clear();
                Console.WriteLine($"X:{PositionX} Y:{PositionY} 方向:{direction}");
                ShowMaze();
                //Thread.Sleep(5);
                //Console.ReadKey();
            }
        }

        static void Check()
        {
            for (int i = 1; i < (Maze.GetLength(0) - 1); i += 2)
            {
                for (int j = 1; j < (Maze.GetLength(1) - 1); j += 2)
                {
                    if (Maze[i, j] == "　")
                    {
                        if ((Maze[i - 1, j] == "■") && (Maze[i + 1, j] == "■") && (Maze[i, j - 1] == "■") && (Maze[i, j + 1] == "■"))
                        {
                        redo:
                            int direction = Direction();
                            switch (direction)
                            {
                                case 0:
                                    if ((i - 1) == 0) { goto redo; }
                                    Maze[i - 1, j] = "　";
                                    break;

                                case 1:
                                    if ((i + 1) == (Maze.GetLength(0) - 1)) { goto redo; }
                                    Maze[i + 1, j] = "　";
                                    break;

                                case 2:
                                    if ((j - 1) == 0) { goto redo; }
                                    Maze[i, j - 1] = "　";
                                    break;

                                case 3:
                                    if ((j + 1) == (Maze.GetLength(1) - 1)) { goto redo; }
                                    Maze[i, j + 1] = "　";
                                    break;
                            }
                        }
                    }
                    Console.WriteLine("修正中");
                    Console.Clear();
                    ShowMaze();
                    //Thread.Sleep(5);
                }
            }
        }

    }
}