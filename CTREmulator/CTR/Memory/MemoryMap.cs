using System;
using System.Collections.Generic;
using System.Text;

namespace CTREmulator.CTR.Memory
{
    class MemoryMap
    {
        private int ProcessorId;

        public MemoryMap(int ProcessorId = 0) // 0 = ARM9, 1 = ARM11
        {
            this.ProcessorId = ProcessorId;
        }
    }
}
