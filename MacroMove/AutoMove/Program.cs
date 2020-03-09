using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;

namespace AutoMove
{
    class Program
    {
        static void Main(string[] args)
        {

            SendToMinecraft();
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void SendToMinecraft()
        {
            //マイクラのプロセス
            var process = Process.GetProcessesByName("javaw");

            SetForegroundWindow(process[0].MainWindowHandle);
            Console.WriteLine("Press W key...");
            SendKeys.SendWait("{W 1000}");
            Thread.Sleep(500);
            Console.WriteLine("Press S key...");
            SendKeys.SendWait("{S 1000}");
            Console.WriteLine("Press D key...");
            SendKeys.SendWait("{D 1000}");
            Thread.Sleep(500);
            Console.WriteLine("Press A key");
            SendKeys.SendWait("{A 2000}");
        }
    }
}