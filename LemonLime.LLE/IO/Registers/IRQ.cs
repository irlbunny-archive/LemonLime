using LemonLime.Common;

namespace LemonLime.LLE.IO.Registers
{
    class IRQ
    {
        public static void IRQ_IE(Context Ctx)
        {
            Logger.WriteStub("Stubbed.");

            Ctx.SetOutput(0xFFFFFFFF);
        }

        public static void IRQ_IF(Context Ctx)
        {
            Logger.WriteStub("Stubbed.");

            Ctx.SetOutput(0xFFFFFFFF);
        }
    }
}
