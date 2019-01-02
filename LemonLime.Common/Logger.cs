using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace LemonLime.Common
{
    public class Logger
    {
        public static string LogFile = string.Empty;

        public static void WriteInfo(string Input, [CallerMemberName] string CallerName = "")
        {
            Console.ForegroundColor = ConsoleColor.White;
            WriteString($"{DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()} [INFO] | {CallerName} > {Input}");
            Console.ResetColor();
        }

        public static void WriteWarning(string Input, [CallerMemberName] string CallerName = "")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            WriteString($"{DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()} [WARNING] | {CallerName} > {Input}");
            Console.ResetColor();
        }

        public static void WriteError(string Input, [CallerMemberName] string CallerName = "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteString($"{DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()} [ERROR] | {CallerName} > {Input}");
            Console.ResetColor();
        }

        public static void WriteStub(string Input, [CallerMemberName] string CallerName = "")
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            WriteString($"{DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()} [STUB] | {CallerName} > {Input}");
            Console.ResetColor();
        }

        private static void WriteString(string Input)
        {
            Console.WriteLine(Input);

            if (LogFile != string.Empty)
            {
                File.AppendAllText(LogFile, Input + "\r\n");
            }
        }
    }
}
