using System;
using System.Diagnostics;
using System.Threading;

namespace KillProcess
{
    class Program
    {
        private static string processName;
        private static int timeToLive;
        private static int interval;


        static void Main(string[] args)
        {
            Initialize(args);

            var timer = new Timer(e => KillProcess(), null, TimeSpan.Zero, TimeSpan.FromMinutes(interval));

            Console.WriteLine("Monitoring processes... press any key to quit");
            Console.ReadKey();

            timer.Dispose();
        }

        private static void Initialize(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException("Invalid number of arguments");
            }

            processName = args[0];

            if (!int.TryParse(args[1], out timeToLive))
            {
                throw new ArgumentException("Process maximum lifetime is invalid");
            }

            if (!int.TryParse(args[2], out interval))
            {
                throw new ArgumentException("Interval value is invalid");
            }
        }

        private static void KillProcess()
        {
            Process[] processes = Process.GetProcessesByName(processName);

            foreach (var process in processes)
            {
                var duration = DateTime.Now - process.StartTime;
                if (duration > TimeSpan.FromMinutes(timeToLive))
                {
                    process.Kill();
                    Console.WriteLine($"{processName} was killed");
                }
            }
        }
    }
}
