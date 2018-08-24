namespace LemonLime.CTR.IO.Registers
{
    class CFG9
    {
        public static void CFG9_RST11(IOData Data)
        {
            bool Reset = (Data.Write8 << 31 != 1);

            if (Reset && Data.Write8 != 0) Data.CPU.EnableCPU(CPUType.ARM11, Reset);

            Data.Read8 = 0xFF;
        }

        public static void CFG9_UNITINFO(IOData Data)
        {
            Data.Read8 = 0x00; // Retail
        }
    }
}
