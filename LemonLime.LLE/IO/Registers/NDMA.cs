using LemonLime.Common;

namespace LemonLime.LLE.IO.Registers
{
    class NDMA
    {
        public static void NDMA_GLOBAL_CNT(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read32 = 0x00000000;
        }

        public static void NDMA_CNT(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read32 = 0x00000000;
        }
    }
}
