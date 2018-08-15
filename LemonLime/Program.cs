using LemonLime.Common;
using LemonLime.CTR;

namespace LemonLime
{
    class Program
    {
        static void Main(string[] args)
        {
            //Logger.LogFile = "lemonlime_log-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            Logger.WriteInfo("Starting...");

            Core CTR = new Core();
            CTR.Start();
        }
    }
}
