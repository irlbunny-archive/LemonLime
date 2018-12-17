namespace LemonLime.LLE.IO.Registers
{
    class CFG9
    {
        public static void CFG9_RST11(Context Ctx)
        {
            bool Reset = Ctx.GetInputUInt8() << 31 != 1;
            if (Reset && Ctx.GetInputUInt8() != 0) Ctx.ProcHandler.EnableCpu(CPU.Type.Arm11, Reset);

            Ctx.SetOutput(0xFF);
        }

        public static void CFG9_UNITINFO(Context Ctx)
        {
            Ctx.SetOutput(0x00); // Retail
        }
    }
}
