using LemonLime.Common;

namespace LemonLime.LLE.Device.ARM9
{
    class IRQ : CPU.Device
    {
        private enum IRQ_INT
        {
            // DMAC
            DMAC_1_0 = 0,
            DMAC_1_1 = 1,
            DMAC_1_2 = 2,
            DMAC_1_3 = 3,
            DMAC_1_4 = 4,
            DMAC_1_5 = 5,
            DMAC_1_6 = 6,
            DMAC_1_7 = 7,

            // TIMER
            TIMER_0 = 8,
            TIMER_1 = 9,
            TIMER_2 = 10,
            TIMER_3 = 11,

            // PXI
            PXI_SYNC      = 12,
            PXI_NOT_FULL  = 13,
            PXI_NOT_EMPTY = 14,

            // AES
        }

        private uint IRQ_IE;
        private uint IRQ_IF;

        public IRQ()
        {
            this.IRQ_IE = 0;
            this.IRQ_IF = 0;
        }

        public uint ReadWord(uint Offset)
        {
            switch (Offset)
            {
                case 0:
                    break;

                case 4:
                    break;
            }

            return 0;
        }

        public ushort ReadShort(uint Offset) { return 0; }
        public byte   ReadByte (uint Offset) { return 0; }

        public void WriteWord(uint Offset, uint Value)
        {
            Logger.WriteInfo($"Offset = {Offset}");
        }

        public void WriteShort(uint Offset, ushort Value) { }
        public void WriteByte (uint Offset, byte   Value) { }

        public uint   Size() { return 8;     }
        public string Name() { return "IRQ"; }
    }
}
