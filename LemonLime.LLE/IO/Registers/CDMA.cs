using LemonLime.Common;
using LemonLime.LLE.Memory;

namespace LemonLime.LLE.IO.Registers
{
    class CDMA
    {
        public static void CDMA_UNKNOWN(Context Ctx)
        {
            Logger.WriteStub("Stubbed.");

            Ctx.SetOutput(0x00000000);
        }
    }
}
