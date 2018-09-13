using LemonLime.Common;

namespace LemonLime.LLE.IO.Registers
{
    class IRQ
    {
        public static uint IRQ_IE_FLAGS { get; private set; }

        public static uint IRQ_IF_FLAGS { get; private set; }

        public static void SetFlag(int Flag)
        {
            if (((1 << Flag) & IRQ_IE_FLAGS) == 0) return;

            IRQ_IF_FLAGS |= (uint)(1 << Flag);

            // TODO: Trigger Interpreter level IRQ.
        }

        private static void IF_WriteLong(uint Value)
        {
            IRQ_IF_FLAGS &= (~Value);
        }

        public static void IRQ_IE(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            if (Data.Type == IOType.Write)
            {
                IRQ_IE_FLAGS = Data.Write32;
            }

            Data.Read32 = IRQ_IE_FLAGS;
        }

        public static void IRQ_IF(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            if (Data.Type == IOType.Write)
            {
                IF_WriteLong(Data.Write32);
            }

            Data.Read32 = IRQ_IF_FLAGS;
        }
    }
}
