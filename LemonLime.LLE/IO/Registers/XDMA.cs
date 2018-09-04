using LemonLime.Common;

namespace LemonLime.LLE.IO.Registers
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
