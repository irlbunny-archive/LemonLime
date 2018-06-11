using System;
using System.Collections.Generic;
using System.Text;

namespace CTREmulator.CTR.Memory
{
    class Memory
    {
        const uint KB = 1024;
        const uint MB = 1024 * KB;

        public byte[] FCRAM;
        public byte[] ARM9_ITCM;
        public byte[] ARM11_PRIV_MEM;
        public byte[] AXI_WRAM;

        public Memory()
        {
            FCRAM = new byte[Settings.FCRAMSize * MB];
            ARM9_ITCM = new byte[0x8000];
            ARM11_PRIV_MEM = new byte[0x2000];
            AXI_WRAM = new byte[0x80000];
        }
    }
}
