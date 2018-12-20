using LemonLime.Common;
using LemonLime.LLE.Memory;

namespace LemonLime.LLE.IO.Registers
{
    class NDMA
    {
        public static void NDMA_GLOBAL_CNT(Context Ctx)
        {
            Logger.WriteStub("Stubbed.");

            Ctx.SetOutput(0xFFFFFFFF);
        }

        public static void NDMA_CNT(Context Ctx)
        {
            Logger.WriteStub("Stubbed.");

            Ctx.SetOutput(0xFFFFFFFF);
        }
    }
}
