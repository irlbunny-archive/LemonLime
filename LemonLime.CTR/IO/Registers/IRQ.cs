using LemonLime.Common;

namespace LemonLime.CTR.IO.Registers
{
    class IRQ
    {
        public static void IRQ_IE(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read32 = 0xFF;
        }

        public static void IRQ_IF(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read32 = 0xFF;
        }
    }
}
