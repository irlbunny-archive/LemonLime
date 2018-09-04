using LemonLime.Common;

namespace LemonLime.LLE.IO.Registers
{
    class CDMA
    {
        public static void CDMA_UNKNOWN(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read32 = 0x00;
        }
    }
}
