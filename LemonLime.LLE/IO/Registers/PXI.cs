using LemonLime.Common;

namespace LemonLime.LLE.IO.Registers
{
    class PXI
    {
        public static void PXI_SYNC(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read32 = 0x00000000;
        }

        public static void PXI_CNT(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read16 = 0x00000000;
        }
    }
}
