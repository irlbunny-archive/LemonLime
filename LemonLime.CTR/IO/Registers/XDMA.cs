using LemonLime.Common;

namespace LemonLime.CTR.IO.Registers
{
    class XDMA
    {
        public static void XDMA_UNKNOWN(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read32 = 0x00;
        }
    }
}
