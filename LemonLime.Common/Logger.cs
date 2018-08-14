using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LemonLime.Common
{
    public class Logger
    {
        public static void WriteInfo(string Input, [CallerMemberName] string CallerName = "")
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()} | {CallerName} > {Input}");
            Console.ResetColor();
        }

        public static void WriteWarning(string Input, [CallerMemberName] string CallerName = "")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()} | {CallerName} > {Input}");
            Console.ResetColor();
        }

        public static void WriteError(string Input, [CallerMemberName] string CallerName = "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()} | {CallerName} > {Input}");
            Console.ResetColor();
        }

        public static void WriteStub(string Input, [CallerMemberName] string CallerName = "")
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"{DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()} | {CallerName} > {Input}");
            Console.ResetColor();
        }
    }
}
