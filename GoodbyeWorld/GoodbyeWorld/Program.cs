using System;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace GoodbyeWorld
{
    class Program
    {
        private static string getILDASMDirectory()
        {
            return @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools\";
        }

        private static string getILDASMPath()
        {
            return "\"" + getILDASMDirectory() + "ildasm.exe\"";
        }

        private static string getILASMDirectory()
        {
            return @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\";
        }

        private static string getILASMPath()
        {
            return getILASMDirectory() + "ilasm.exe";
        }

        public static void Main(string[] args)
        {
            string curDir = Directory.GetCurrentDirectory();
            string[] pathElements = { curDir, "..", "..", "..", "..", "HelloWorld", "HelloWorld", "bin", "Debug", "HelloWorld.exe" };
            string helloWorldPath = "\"" + Path.Combine(pathElements) + "\"";
            string[] arguments = { helloWorldPath, "/output:HelloWorld.il" };
            ProcessStartInfo info = new ProcessStartInfo(getILDASMPath());
            info.WindowStyle = ProcessWindowStyle.Minimized;
            info.Arguments = string.Join(" ", arguments);
            info.UseShellExecute = false;
            info.CreateNoWindow = false;
            try
            {
                Process.Start(info);
            }
            catch (Win32Exception e)
            {
                Console.WriteLine(e.Message);
            }
            string fileContents = File.ReadAllText(@"HelloWorld.il");
            fileContents = fileContents.Replace("Hello, world!", "Goodbye, world!");
            File.WriteAllText(Path.Combine(curDir, @"tmp.il"), fileContents);
            
            info = new ProcessStartInfo(getILASMPath());
            info.WindowStyle = ProcessWindowStyle.Minimized;
            info.Arguments = "tmp";
            info.UseShellExecute = false;
            info.CreateNoWindow = false;
            try
            {
                Process.Start(info);
            }
            catch (Win32Exception e)
            {
                Console.WriteLine(e.Message);
            }
            File.Copy(@"tmp.exe", Path.Combine(pathElements), true);
            File.Delete("tmp.exe");
            File.Delete("tmp.il");
            File.Delete("HelloWorld.il");
            File.Delete("HelloWorld.res");
        }
    }
}
