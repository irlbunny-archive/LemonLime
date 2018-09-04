using LemonLime.Common;

namespace LemonLime.LLE.IO.Registers
{
    class HID
    {
        public static void HID_PAD(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read16 = 0xFFF;
        }
    }
}
