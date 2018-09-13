using LemonLime.Common;
using System;

namespace LemonLime
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Logger.LogFile = "lemonlime_log-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            Logger.WriteInfo("Starting...");

            using (GLScreen screen = new GLScreen())
            {
                screen.Run(60.0);
            }
        }
    }
}
