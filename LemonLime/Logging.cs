using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LemonLime
{
    class Logging
    {
        public static void WriteInfo(string Input, [CallerMemberName] string CallerName = "")
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()} | {CallerName} > {Input}");
            Console.ResetColor();
        }
    }
}
