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

        public Memory()
        {
            FCRAM = new byte[Settings.FCRAMSize * MB];
        }
    }
}
