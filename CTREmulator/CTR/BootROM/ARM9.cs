using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CTREmulator.CTR.BootROM
{
    class ARM9
    {
        public byte[] ARM9_BootROM = new byte[0x00010000];

        public ARM9()
        {
            ARM9_BootROM = File.ReadAllBytes("boot9.bin");
        }
    }
}
