using LemonLime.Common;
using LemonLime.LLE.Memory;

namespace LemonLime.LLE.IO.Registers
{
    class HID
    {
        public static void HID_PAD(Context Ctx)
        {
            Logger.WriteStub("Stubbed.");

            Ctx.SetOutput(0xFFF);
        }
    }
}
