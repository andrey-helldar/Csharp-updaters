using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;


namespace _Hell_Universal_Restarter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] != "")
            {
                int i = 0;

                string s = args[0].Replace(".exe", "");
                Process[] myProcesses = Process.GetProcessesByName(s);

                while (myProcesses.Length > 0)
                {
                    Console.WriteLine("Closing " + args[0] + " (step " + (++i).ToString() + ")");
                    myProcesses[0].Kill();

                    Thread.Sleep(500);
                }

                Process.Start(args[0]);
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine("Parameter not found...");
                Console.ReadLine();
            }
        }
    }
}
