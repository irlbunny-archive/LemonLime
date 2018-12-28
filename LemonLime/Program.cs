using LemonLime.Common;
using LemonLime.LLE.CPU;

namespace LemonLime
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.WriteInfo("Starting...");

            Settings.SettingsFile = "config.ini";
            Settings.Load();

            LLE.CTR CTR = new LLE.CTR();
            CTR.SetCPU(Type.ARM9, true);
            CTR.Start();
        }
    }
}
