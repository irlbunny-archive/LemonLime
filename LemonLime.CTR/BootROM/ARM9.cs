using System.IO;

namespace LemonLime.CTR.BootROM
{
    class ARM9
    {
        public byte[] BootROM = new byte[0x00010000];

        public ARM9()
        {
            BootROM = File.ReadAllBytes("boot9.bin");
        }
    }
}
