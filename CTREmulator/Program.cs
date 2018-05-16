using System;

namespace CTREmulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int num = 0xf0;
            num = (num >> 4) | (num << (32 - 4));
            Console.WriteLine(num.ToString("X2"));
            Console.ReadKey();
        }
    }
}
