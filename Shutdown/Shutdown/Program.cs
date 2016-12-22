using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Shutdown
{
    class MainClass
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [Flags]
        public enum ExitWindows : uint
        {
            // ONE of the following:
            LogOff = 0x00,
            ShutDown = 0x01,
            Reboot = 0x02,
            PowerOff = 0x08,
            RestartApps = 0x40,
            // plus AT MOST ONE of the following two:
            Force = 0x04,
            ForceIfHung = 0x10
        }

        private static bool isNumber(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        public static void Main(string[] args)
        {
            string[] supportedFlags = { "-l", "-r", "-s", "-t" };
            int delay = 0;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-t"))
                {
                    if (i + 1 < args.Length && isNumber(args[i + 1]))
                    {
                        delay = Int32.Parse(args[i + 1]);
                        i++;
                    }
                    else
                    {
                        delay = 20;
                    }
                    continue;
                }
                if (Array.IndexOf(supportedFlags, args[i]) == -1)
                {
                    System.Console.WriteLine("Unsupported flag: " + args[i]);
                    System.Environment.Exit(1);
                }
            }

            uint flags = 0;
            if (Array.IndexOf(args, "-r") != -1)
            {
                flags |= (uint)ExitWindows.Reboot;
            }
            if (Array.IndexOf(args, "-l") != -1)
            {
                flags |= (uint)ExitWindows.LogOff;
            }
            if (Array.IndexOf(args, "-s") != -1)
            {
                flags |= (uint)ExitWindows.ShutDown;
            }
            Thread.Sleep(delay * 1000);
            ExitWindowsEx(flags, 0);
        }
    }
}