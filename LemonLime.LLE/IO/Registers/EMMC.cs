using LemonLime.Common;

namespace LemonLime.LLE.IO.Registers
{
    class EMMC
    {
        public static void EMMC_STOP(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read16 = 0x0000;
        }

        public static void EMMC_BLKCOUNT(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read16 = 0x0000;
        }

        public static void EMMC_STATUS0(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read16 = 0x0000;
        }

        public static void EMMC_STATUS1(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read16 = 0x0000;
        }

        public static void EMMC_CLKCTL(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read16 = 0x0000;
        }

        public static void EMMC_OPT(IOData Data)
        {
            Logger.WriteStub("Stubbed.");

            Data.Read16 = 0x0000;
        }
    }
}
