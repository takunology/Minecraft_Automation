using System;

namespace MazeGenerator
{
    class Program
    {
        public static int[,] Maze = new int[0, 0];

        static void Main(string[] args)
        {
            Console.WriteLine("Maze Generator");
            Input(); //迷路のサイズ
            Wall(); // 迷路の外壁生成
        }

        static void Input()
        {
            Console.Write("X：");
            int X = int.Parse(Console.ReadLine());
            Console.Write("Z：");
            int Y = int.Parse(Console.ReadLine());

            Maze = new int[X, Y];
            Console.WriteLine();
        }

        static void Wall()
        {
            for(int i = 0; i < Maze.GetLength(0); i++)
            {
                if(i == 0 || i == (Maze.GetLength(0) - 1)) // X方向の始めと終り
                {
                    for (int j = 0; j < Maze.GetLength(1); j++)
                    {
                        Maze[i, j] = 1;
                        Console.Write(Maze[i, j]); //壁生成
                    }
                    Console.WriteLine();
                }
                else 
                {
                    for (int j = 0; j < Maze.GetLength(1); j++)
                    {
                        if (j == 0 || j == (Maze.GetLength(1) - 1)) // Z方向の始めと終り
                        {
                            Maze[i, j] = 1;
                            Console.Write(Maze[i, j]); //壁生成
                        }
                        else
                        {
                            Maze[i, j] = 0;
                            Console.Write(Maze[i, j]);
                        }                   
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
