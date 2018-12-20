using LemonLime.Common;
using LemonLime.LLE.Memory;

namespace LemonLime.LLE.IO.Registers
{
    class PXI
    {
        public static void PXI_SYNC(Context Ctx)
        {
            Logger.WriteStub("Stubbed.");

            Ctx.SetOutput(0xFFFF);
        }

        public static void PXI_CNT(Context Ctx)
        {
            Logger.WriteStub("Stubbed.");

            Ctx.SetOutput(0xFFFF);
        }
    }
}
