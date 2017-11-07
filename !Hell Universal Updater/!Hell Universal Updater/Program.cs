using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace _Hell_Universal_Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int i = 0;

                string s = args[1].Replace(".exe", "");
                Process[] myProcesses = Process.GetProcessesByName(s);

                while (myProcesses.Length > 0)
                {
                    Console.WriteLine("Closing " + args[1] + " (step " + (++i).ToString() + ")");
                    myProcesses[0].Kill();

                    Thread.Sleep(500);
                }

                Console.WriteLine("Please, wait a few seconds...");
                if (File.Exists(args[1])) { File.Delete(args[1]); }

                File.Move(args[0], args[1]);

                Console.WriteLine("Starting " + args[1]);
                Process.Start(Directory.GetCurrentDirectory() + @"\" + args[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            try
            {
                Thread.Sleep(500);
                if (args[2] != "") { File.Delete(args[2]); }
            }
            catch (Exception) { Console.WriteLine("OK"); }
        }
    }
}
