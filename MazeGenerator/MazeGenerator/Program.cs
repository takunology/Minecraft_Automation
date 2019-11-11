using System;

namespace MazeGenerator
{
    class Program
    {
        public static int[,] Maze = new int[0, 0]; //ベースの迷路

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
            Console.Write("X：");
            int X = int.Parse(Console.ReadLine());
            Console.Write("Z：");
            int Z = int.Parse(Console.ReadLine());

            Maze = new int[X, Z];
            Console.WriteLine();
        }

        static void Wall() //壁生成
        {
            for(int i = 0; i < Maze.GetLength(0); i++)
            {
                if(i == 0 || i == (Maze.GetLength(0) - 1)) // X方向の始めと終り
                {
                    for (int j = 0; j < Maze.GetLength(1); j++)
                    {
                        Maze[i, j] = 1;
                    }
                }
                else 
                {
                    for (int j = 0; j < Maze.GetLength(1); j++)
                    {
                        if (j == 0 || j == (Maze.GetLength(1) - 1)) // Z方向の始めと終り
                        {
                            Maze[i, j] = 1;
                        }
                        else
                        {
                            Maze[i, j] = 0;
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
            
            Random rnd = new Random();
            int PointX = rnd.Next(2, Maze.GetLength(0) - 2); //壁伸ばし法の開始点X (外壁と通路は除く)
            int PointZ = rnd.Next(2, Maze.GetLength(1) - 2); //壁伸ばし法の開始点Z (外壁と通路除く)
            
            Console.WriteLine($"開始点：{PointX}, {PointZ}");
            
            while(PointX != 1 && PointX != Maze.GetLength(0) - 1 || PointZ != 1 && PointZ != Maze.GetLength(1) - 1) //外壁でないかぎり続ける
            {
                Maze[PointX, PointZ] = 1; //とりあえず開始点を壁にしとく
                int direction = Direction(); //進む方向を決める
                Console.WriteLine($"進行方向：{direction}");

                /*if (Maze[PointX + 1, PointZ] == 1 && Maze[PointX - 1, PointZ] == 1 && Maze[PointX, PointZ + 1] == 1 && Maze[PointX, PointZ - 1] == 1)
                {
                    break; //隣接したマスが壁ならループを抜ける
                }
                else*/
                {
                    switch (direction)
                    {
                        case 0: //下方向
                            if (Maze[PointX + 1, PointZ] == 0 && Maze[PointX + 2, PointZ] == 0) //隣接が通路かつその隣接も通路
                            {
                                Maze[PointX + 1, PointZ] = 1; //壁生成
                                PointX = PointX + 1; //道の分1マス開ける
                            }                           
                            break;

                        case 1: //上方向
                            if (Maze[PointX - 1, PointZ] == 0 && Maze[PointX - 2, PointZ] == 0)
                            {
                                Maze[PointX - 1, PointZ] = 1; //壁
                                PointX = PointX - 1; //道の分1マス開ける
                            }
                            break;

                        case 2: //右方向
                            if (Maze[PointX, PointZ + 1] == 0 && Maze[PointX, PointZ + 2] == 0)
                            {
                                Maze[PointX, PointZ + 1] = 1; //壁
                                PointZ = PointZ + 1; //道の分1マス開ける
                            }
                            break;

                        case 3: //左方向
                            if (Maze[PointX, PointZ - 1] == 0 && Maze[PointX, PointZ - 2] == 0)
                            {
                                Maze[PointX, PointZ - 1] = 1; //壁
                                PointZ = PointZ - 1; //道の分1マス開ける
                            }
                            break;
                    }
                    Console.WriteLine($"壁を作る座標：{PointX}, {PointZ}");
                    //Console.Clear();
                    ShowMaze();
                    System.Threading.Thread.Sleep(100);
                    //Console.Clear();
                }
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
                    if(Maze[i, j] == 1) { Console.ForegroundColor = ConsoleColor.Yellow; }
                    else { Console.ForegroundColor = ConsoleColor.White; }

                    Console.Write(Maze[i, j]);
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}