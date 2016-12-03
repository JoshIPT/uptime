using System;
using System.Reflection;
using System.Management;
using System.Diagnostics;

namespace uptime
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeSpan upspan;
            /*using (var uptime = new PerformanceCounter("System", "System Up Time"))
            {
                uptime.NextValue(); 
                upspan = TimeSpan.FromSeconds(uptime.NextValue());
            }*/

            var ticks = Stopwatch.GetTimestamp();
            var uptime = ((double)ticks) / Stopwatch.Frequency;
            upspan = TimeSpan.FromSeconds(uptime);

            string upstr = upspan.Days.ToString() + " days, ";
            upstr += padInt(upspan.Hours) + ":";
            upstr += padInt(upspan.Minutes) + ":";
            upstr += padInt(upspan.Seconds);

            Console.WriteLine("Uptime: " + upstr);

            /*if ((!isParentCmd()) && (args.Length < 2))
            {
                Console.WriteLine("");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }*/
        }

        public static string padInt(int num)
        {
            if (num < 10)
            {
                return "0" + num.ToString();
            }
            else
            {
                return num.ToString();
            }
        }

        public static bool isParentCmd()
        {
            var myId = Process.GetCurrentProcess().Id;
            var query = string.Format("SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = {0}", myId);
            var search = new ManagementObjectSearcher("root\\CIMV2", query);
            var results = search.Get().GetEnumerator();
            results.MoveNext();
            var queryObj = results.Current;
            var parentId = (uint)queryObj["ParentProcessId"];
            var parent = Process.GetProcessById((int)parentId);
            if (parent.ProcessName.ToLower() == "cmd") { return true; }
            else { return false; }
        }
    }
}
