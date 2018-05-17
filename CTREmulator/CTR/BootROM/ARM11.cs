using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CTREmulator.CTR.BootROM
{
    class ARM11
    {
        public byte[] ARM11_BootROM = new byte[0x00010000];

        public ARM11()
        {
            ARM11_BootROM = File.ReadAllBytes("boot11.bin");
        }
    }
}
