using System;

namespace MazeGenerator
{
    class Program
    {
        public static string[,] Maze = new string[0, 0]; //ベースの迷路

        static void Main(string[] args)
        {
            Console.WriteLine("Maze Generator");
#if DEBUG
            while (true)
            {
                Input(); //迷路のサイズ
                Wall(); // 迷路の外壁生成
                Generate();
                ShowMaze();
            }
#endif
        }

        static void Input()
        {
            while (true)
            {
                Console.Write("X：");
                int X = int.Parse(Console.ReadLine());
                Console.Write("Z：");
                int Z = int.Parse(Console.ReadLine());

                if(X % 2 == 1 && Z % 2 == 1)
                {
                    Maze = new string[X, Z];
                    Console.WriteLine();
                    break;
                }
                else { Console.WriteLine("奇数で。"); }
            }
        }

        static void Wall() //壁生成
        {
            for(int i = 0; i < Maze.GetLength(0); i++)
            {
                if(i == 0 || i == (Maze.GetLength(0) - 1)) // X方向の始めと終り
                {
                    for (int j = 0; j < Maze.GetLength(1); j++)
                    {
                        Maze[i, j] = "■";
                    }
                }
                else 
                {
                    for (int j = 0; j < Maze.GetLength(1); j++)
                    {
                        if (j == 0 || j == (Maze.GetLength(1) - 1)) // Z方向の始めと終り
                        {
                            Maze[i, j] = "■";
                        }
                        else
                        {
                            Maze[i, j] = "　";
                        }                   
                    }
                }
            }
        }

        static void Generate() //迷路生成
        {
            /// <summary>
            /// ランダムな初期座標 PointX, PointY を指定。ただし、座標が奇数であれば壁、偶数なら道。
            /// 進む方向は乱数で決めて、その方向の先が壁でない限り繰り返す。
            /// </summary>
            while (true)
            {
                bool Initialize = false; //既に1回目の壁伸ばしをしているか
label:          
                Random rnd = new Random();
                int PointX = 0; 
                int PointZ = 0; 

                while (true) //偶数になるまで再抽選
                {
                    PointX = rnd.Next(2, Maze.GetLength(0) - 2);//壁伸ばし法の開始点X (外壁と通路は除く)
                    PointZ = rnd.Next(2, Maze.GetLength(1) - 2);
                    if (PointX % 2 == 0 && PointZ % 2 == 0)
                    {
                        if (Initialize == true && Maze[PointX, PointZ] == "■") { }                             
                        else { break; }                       
                    }
                }

                Console.WriteLine($"開始点：{PointX}, {PointZ}");
                Initialize = true;
                Maze[PointX, PointZ] = "■"; //とりあえず開始点を壁にしとく

                //道を挟んだ四方が壁になるまで続ける
                if ((Maze[PointX - 2, PointZ - 2] == "■") && (Maze[PointX + 2, PointZ - 2] == "■") && (Maze[PointX - 2, PointZ + 2] == "■") && (Maze[PointX + 2, PointZ + 2] == "■"))
                {                    
                     break;
                }

                while (PointX != 0 || PointX != (Maze.GetLength(0) - 1) || PointZ != 1 || PointZ != (Maze.GetLength(1) - 1)) //外壁でないかぎり続ける
                {
                    int direction = Direction(); //進む方向を決める
                    Console.WriteLine($"進行方向：{direction}");
                    switch (direction)
                    {
                        case 0: //下方向
                            if (PointX == (Maze.GetLength(0) - 1)) { goto label; } //下の壁に衝突した場合
                            else
                            {

                                if (Maze[PointX + 2, PointZ] == "　" /*|| Maze[PointX + 2, PointZ] == "　"*/) //隣接が通路かつその隣接も通路
                                {
                                    Maze[PointX + 1, PointZ] = "■"; //壁
                                    Maze[PointX + 2, PointZ] = "■"; //壁生成
                                    PointX = PointX + 2; //道の分1マス開ける
                                    if (PointX == (Maze.GetLength(0) - 3)) //下側の壁に隣接した場合は伸ばす
                                    {
                                        Maze[PointX, PointZ + 1] = "■";
                                        //if (Maze[PointX - 2, PointZ - 2] == "■" && Maze[PointX + 2, PointZ - 2] == "■" && Maze[PointX - 2, PointZ + 2] == "■" && Maze[PointX + 2, PointZ + 2] == "■")
                                        if(Maze[PointX + 2, PointZ] == "■")
                                        {
                                            //PointX = PointX + 2;
                                            goto label;
                                        }
                                    }
                                    else if (Maze[PointX + 2, PointZ] == "■")
                                    {
                                        //PointX = PointX + 2;
                                        goto label;
                                    }
                                }
                                break;
                            }

                        case 1: //上方向
                            if (PointX == 0) { goto label; } //上の壁に衝突した場合
                            else
                            {
                                if (Maze[PointX - 2, PointZ] == "　" /*|| Maze[PointX - 2, PointZ] == "　"*/)
                                {
                                    Maze[PointX - 1, PointZ] = "■"; //壁
                                    Maze[PointX - 2, PointZ] = "■"; //壁生成
                                    PointX = PointX - 2; //道の分1マス開ける
                                    if (PointX == 2) //上側の壁に隣接した場合は伸ばす
                                    {
                                        Maze[PointX, PointZ - 1] = "■";
                                        //if (Maze[PointX - 2, PointZ - 2] == "■" && Maze[PointX + 2, PointZ - 2] == "■" && Maze[PointX - 2, PointZ + 2] == "■" && Maze[PointX + 2, PointZ + 2] == "■")
                                            if (Maze[PointX - 2, PointZ] == "■")
                                            {
                                            //PointX = PointX - 2;
                                            goto label;
                                        }
                                    }
                                    else if (Maze[PointX - 2, PointZ] == "■")
                                    {
                                        //PointX = PointX - 2;
                                        goto label;
                                    }
                                }
                                break;
                            }

                        case 2: //右方向
                            if (PointZ == (Maze.GetLength(1) - 1)) { goto label; } //右の壁に衝突した場合
                            else
                            {

                                if (Maze[PointX, PointZ + 2] == "　" /*|| Maze[PointX, PointZ + 2] == "　"*/)
                                {
                                    Maze[PointX, PointZ + 1] = "■"; //壁
                                    Maze[PointX, PointZ + 2] = "■"; //壁生成
                                    PointZ = PointZ + 2; //道の分1マス開ける
                                    
                                    if(PointZ == (Maze.GetLength(1) - 3)) //右側の壁に隣接した場合は伸ばす
                                    {
                                        Maze[PointX, PointZ + 1] = "■";

                                        //if (Maze[PointX - 2, PointZ - 2] == "■" && Maze[PointX + 2, PointZ - 2] == "■" && Maze[PointX - 2, PointZ + 2] == "■" && Maze[PointX + 2, PointZ + 2] == "■")
                                        if (Maze[PointX, PointZ + 2] == "■")
                                        {
                                            //PointZ = PointZ + 2;
                                            goto label;
                                        }
                                    }
                                    else if (Maze[PointX, PointZ + 2] == "■")
                                    {
                                        //PointZ = PointZ + 2;
                                        goto label;
                                    }
                                }
                                break;
                            }

                        case 3: //左方向
                            if (PointZ == 0) { goto label; } //左の壁に衝突した場合
                            else
                            {
                                if (Maze[PointX, PointZ - 2] == "　" /*|| Maze[PointX, PointZ - 2] == "　"*/)
                                {
                                    Maze[PointX, PointZ - 1] = "■"; //壁
                                    Maze[PointX, PointZ - 2] = "■"; //壁生成
                                    PointZ = PointZ - 2; //道の分1マス開ける
                                    if (PointZ == 2) //左側の壁に隣接した場合は伸ばす
                                    {
                                        Maze[PointX, PointZ - 1] = "■";

                                        //if (Maze[PointX - 2, PointZ - 2] == "■" && Maze[PointX + 2, PointZ - 2] == "■" && Maze[PointX - 2, PointZ + 2] == "■" && Maze[PointX + 2, PointZ + 2] == "■")
                                        if (Maze[PointX, PointZ - 2] == "■")
                                        {
                                            //PointZ = PointZ - 2;
                                            goto label;
                                        }
                                    }
                                    else if (Maze[PointX, PointZ - 2] == "■")
                                    {
                                        //PointZ = PointZ - 2;
                                        goto label;
                                    }
                                }
                                break;
                            }
                    }
                    Console.WriteLine($"壁を作る座標：{PointX}, {PointZ}");
                    //Console.Clear();
                    ShowMaze();
                    //Console.ReadKey();
                    System.Threading.Thread.Sleep(50);
                }
                Console.WriteLine("壁伸ばし終了");
            }
            Console.WriteLine("\n終了");
        }

        static int Direction() // 進路決め
        {
            Random rnd = new Random();
            int direction = rnd.Next(0, 4);
            return direction;
        }

        static void ShowMaze() //作成状況の表示
        {
            for(int i = 0; i < Maze.GetLength(0); i++)
            {
                for(int j = 0; j < Maze.GetLength(1); j++)
                {
                    if(Maze[i, j] == "■") { Console.ForegroundColor = ConsoleColor.White; }
                    else { Console.ForegroundColor = ConsoleColor.White; }

                    Console.Write(Maze[i, j]);
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}