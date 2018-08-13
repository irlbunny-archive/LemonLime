using CTREmulator.CTR;
using System;

namespace CTREmulator
{
    class Program
    {
        public static string Boot9RomPath;
        public static string Boot11RomPath;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            if (args.Length < 1)
            {
                Console.WriteLine("Arguments: {Required: Path/To/boot9.bin} {Optional: Path/To/boot11.bin}");
                return;
            }

            Boot9RomPath = args[0];
            if (args.Length > 1) Boot11RomPath = args[1];

            Core CTR = new Core();
            CTR.Start();
        }
    }
}
