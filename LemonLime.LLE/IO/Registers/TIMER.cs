using LemonLime.Common;
using LemonLime.LLE.Memory;

namespace LemonLime.LLE.IO.Registers
{
    class TIMER
    {
        public static void TIMER_VAL(Context Ctx)
        {
            Logger.WriteStub("Stubbed.");

            Ctx.SetOutput(0xFFFF);
        }

        public static void TIMER_CNT(Context Ctx)
        {
            Logger.WriteStub("Stubbed.");

            Ctx.SetOutput(0xFFFF);
        }
    }
}
