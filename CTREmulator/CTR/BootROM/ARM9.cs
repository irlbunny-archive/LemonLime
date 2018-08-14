using System.IO;

namespace CTREmulator.CTR.BootROM
{
    class ARM9
    {
        public byte[] ARM9_BootROM;

        public ARM9()
        {
            ARM9_BootROM = File.ReadAllBytes(Program.Boot9RomPath);
        }
    }
}
