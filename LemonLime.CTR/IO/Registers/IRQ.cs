using LemonLime.Common;

namespace LemonLime.CTR.IO.Registers
{
    class IRQ
    {
        public static byte IRQ_IE(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            return 0xFF;
        }

        public static byte IRQ_IF(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            return 0xFF;
        }
    }
}
