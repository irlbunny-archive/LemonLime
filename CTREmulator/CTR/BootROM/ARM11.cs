using System.IO;

namespace CTREmulator.CTR.BootROM
{
    // Not needed yet, just here for when it is required.
    class ARM11
    {
        public byte[] ARM11_BootROM;

        public ARM11()
        {
            ARM11_BootROM = File.ReadAllBytes(Program.Boot11RomPath);
        }
    }
}
