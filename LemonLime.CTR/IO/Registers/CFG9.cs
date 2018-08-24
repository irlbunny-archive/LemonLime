using LemonLime.Common;

namespace LemonLime.CTR.IO.Registers
{
    class CFG9
    {
        public static void CFG9_RST11(IOData Data)
        {
            bool Reset = (Data.Write8 << 31 != 1);

            Logger.WriteStub($"Stubbed. Reset = {Reset}");

            Data.Read8 = 0xFF;
        }
    }
}
