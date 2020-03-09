using System;
using System.Diagnostics;

namespace ProcessGetApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Get Process...");
            Processes();
            GetMinecraftProcess();
        }

        public static void GetMinecraftProcess()
        {
            //マイクラのプロセス名が "javaw" だったので詳細を得る
            Process[] process = Process.GetProcessesByName("javaw");

            Console.WriteLine($"プロセスID : {process[0].Id}");
            Console.WriteLine($"プロセス名 : {process[0].ProcessName}");
            Console.WriteLine($"ウィンドウタイトル : {process[0].MainWindowTitle}");
            Console.WriteLine($"メインモジュール : {process[0].MainModule}");
            Console.WriteLine($"プロセスの優先順位 : {process[0].BasePriority}");
            Console.WriteLine($"順位の種類 : {process[0].PriorityClass}");
            Console.WriteLine($"メモリ割り当てサイズ : {process[0].PagedMemorySize64}");
            Console.WriteLine($"仮想メモリサイズ : {process[0].VirtualMemorySize64}");
        }

        public static void Processes()
        {
            Process[] processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                Console.WriteLine($"{process.Id}:{process.ProcessName}");
            }
            Console.WriteLine();
        }
    }
}
