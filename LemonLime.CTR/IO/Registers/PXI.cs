using LemonLime.Common;

namespace LemonLime.CTR.IO.Registers
{
    class PXI
    {
        public static void PXI_SYNC(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read32 = 0xFF;
        }

        public static void PXI_CNT(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read16 = 0xFF;
        }
    }
}
