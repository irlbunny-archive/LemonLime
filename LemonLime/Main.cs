using LemonLime.Common;

namespace LemonLime
{
    class Program
    {
        static void Main(string[] args)
        {
            // Logger.LogFile = "lemonlime_log-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            Logger.WriteInfo("Starting...");

            Settings.SettingsFile = "settings.txt";
            Settings.Load();

            LLE.SoC MainSoC = new LLE.SoC();
            MainSoC.SetCPU(LLE.CPUType.ARM9, true);
            MainSoC.Start();
        }
    }
}
