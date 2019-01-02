using LemonLime.Common;

namespace LemonLime.LLE.Device.ARM9
{
    // TODO
    class TIMER : CPU.Device
    {
        public uint ReadWord(uint Offset) { return 0; }

        public ushort ReadShort(uint Offset)
        {
            switch (Offset)
            {
                case (0 + (4 * 0)): // TIMER_VAL(1)
                    Logger.WriteInfo("TIMER_VAL(1), stubbed.");
                    break;
                case (0 + (4 * 1)): // TIMER_VAL(2)
                    Logger.WriteInfo("TIMER_VAL(2), stubbed.");
                    break;
                case (0 + (4 * 2)): // TIMER_VAL(3)
                    Logger.WriteInfo("TIMER_VAL(3), stubbed.");
                    break;
                case (0 + (4 * 3)): // TIMER_VAL(4)
                    Logger.WriteInfo("TIMER_VAL(4), stubbed.");
                    break;

                case (2 + (4 * 0)): // TIMER_CNT(1)
                    Logger.WriteInfo("TIMER_CNT(1), stubbed.");
                    break;
                case (2 + (4 * 1)): // TIMER_CNT(2)
                    Logger.WriteInfo("TIMER_CNT(2), stubbed.");
                    break;
                case (2 + (4 * 2)): // TIMER_CNT(3)
                    Logger.WriteInfo("TIMER_CNT(3), stubbed.");
                    break;
                case (2 + (4 * 3)): // TIMER_CNT(4)
                    Logger.WriteInfo("TIMER_CNT(4), stubbed.");
                    break;

            }

            return 0;
        }

        public byte ReadByte(uint Offset) { return 0; }

        public void WriteWord(uint Offset, uint Value) { }

        public void WriteShort(uint Offset, ushort Value)
        {
            switch (Offset)
            {
                case (0 + (4 * 0)): // TIMER_VAL(1)
                    Logger.WriteInfo("TIMER_VAL(1), stubbed.");
                    break;
                case (0 + (4 * 1)): // TIMER_VAL(2)
                    Logger.WriteInfo("TIMER_VAL(2), stubbed.");
                    break;
                case (0 + (4 * 2)): // TIMER_VAL(3)
                    Logger.WriteInfo("TIMER_VAL(3), stubbed.");
                    break;
                case (0 + (4 * 3)): // TIMER_VAL(4)
                    Logger.WriteInfo("TIMER_VAL(4), stubbed.");
                    break;

                case (2 + (4 * 0)): // TIMER_CNT(1)
                    Logger.WriteInfo("TIMER_CNT(1), stubbed.");
                    break;
                case (2 + (4 * 1)): // TIMER_CNT(2)
                    Logger.WriteInfo("TIMER_CNT(2), stubbed.");
                    break;
                case (2 + (4 * 2)): // TIMER_CNT(3)
                    Logger.WriteInfo("TIMER_CNT(3), stubbed.");
                    break;
                case (2 + (4 * 3)): // TIMER_CNT(4)
                    Logger.WriteInfo("TIMER_CNT(4), stubbed.");
                    break;

            }
        }

        public void WriteByte(uint Offset, byte Value) { }

        public uint   Size() { return 16;      } // Is this wrong?
        public string Name() { return "TIMER"; }
    }
}
