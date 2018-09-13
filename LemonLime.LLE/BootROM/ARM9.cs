using System.IO;

namespace LemonLime.LLE.BootROM
{
    class ARM9
    {
        public byte[] BootROM = new byte[0x00010000];

        public ARM9()
        {
            if (File.Exists("boot9.bin"))
            {
                BootROM = File.ReadAllBytes("boot9.bin");
            }
        }
    }
}
