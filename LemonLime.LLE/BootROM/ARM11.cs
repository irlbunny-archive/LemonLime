using System.IO;

namespace LemonLime.LLE.BootROM
{
    class ARM11
    {
        public byte[] BootROM = new byte[0x00010000];

        public ARM11()
        {
            BootROM = File.ReadAllBytes("boot11.bin");
        }
    }
}
