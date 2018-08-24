using LemonLime.Common;

namespace LemonLime.CTR.IO.Registers
{
    class TIMER
    {
        public static void TIMER_VAL(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read16 = 0xFF;
        }

        public static void TIMER_CNT(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read16 = 0xFF;
        }
    }
}
