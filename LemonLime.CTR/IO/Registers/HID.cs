using LemonLime.Common;

namespace LemonLime.CTR.IO.Registers
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
