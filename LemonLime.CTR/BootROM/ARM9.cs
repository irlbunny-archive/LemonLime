using System.IO;

namespace LemonLime.CTR.BootROM
{
    class ARM9
    {
        public byte[] ARM9_BootROM = new byte[0x00010000];

        public ARM9()
        {
            ARM9_BootROM = File.ReadAllBytes("boot9.bin");
        }
    }
}
