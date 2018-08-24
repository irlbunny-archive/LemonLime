using LemonLime.Common;

namespace LemonLime.CTR.IO.Registers
{
    class CDMA
    {
        public static void CDMA_1000CD00(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read32 = 0xFF;
        }
    }
}
