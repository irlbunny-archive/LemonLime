using LemonLime.Common;

namespace LemonLime.LLE.IO.Registers
{
    class TIMER
    {
        public static void TIMER_VAL(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read16 = 0x0000;
        }

        public static void TIMER_CNT(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read16 = 0x0000;
        }
    }
}
