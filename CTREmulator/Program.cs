using CTREmulator.CTR;
using System;

namespace CTREmulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Core CTR = new Core();
            CTR.Start();
        }
    }
}
