using LemonLime.Common;

namespace LemonLime.CTR.IO.Registers
{
    class CFG9
    {
        public static byte CFG9_RST11(IOData Data)
        {
            bool Reset = (Data.Data8 << 31 != 1);

            Logger.WriteStub($"Stubbed. Reset = {Reset}");

            return 0xFF;
        }
    }
}
