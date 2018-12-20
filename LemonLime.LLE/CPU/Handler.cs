using LemonLime.ARM;
using LemonLime.Common;
using System.Threading;

namespace LemonLime.LLE.CPU
{
    class Handler
    {
        // ARM9 & ARM11 memory
        private Memory Arm9Memory, Arm11Memory;

        // ARM9 & ARM11 cores
        private static Interpreter Arm9, Arm11;

        // Enable all cores
        private bool EnableAll = true;

        // All cores are disabled by default
        private static bool Arm9Enabled, Arm11Enabled;

        // CPU sync
        private bool Sync = false;

        // ARM9 & ARM11 execution threads
        private Thread Arm9Thread, Arm11Thread;

        public Handler(Memory Arm9Memory, Memory Arm11Memory)
        {
            this.Arm9Memory  = Arm9Memory;
            this.Arm11Memory = Arm11Memory;

            Arm9  = new Interpreter(Arm9Memory, true);
            Arm11 = new Interpreter(Arm11Memory);

            Arm9Thread  = new Thread(Thread9);
            Arm11Thread = new Thread(Thread11);

            //Memory.SetHandler(this);
        }

        public static void EnableCpu(Type Type, bool Enabled)
        {
            if (Enabled)
                Logger.WriteInfo($"Enabling processor ${Type}.");
            else
                Logger.WriteInfo($"Disabling processor ${Type}.");

            switch (Type)
            {
                case Type.Arm9:
                    Arm9Enabled = Enabled;
                    break;

                case Type.Arm11:
                    Arm11Enabled = Enabled;
                    break;
            }
        }

        public static void SetIrq(Type Type)
        {
            Interpreter Proc = (Type == Type.Arm9) ? Arm9 : Arm11;
            Logger.WriteInfo($"Triggering IRQ on processor ${Type}.");
            Proc.IRQ = true;
        }

        public void Start()
        {
            Arm9Thread.Start();
            Arm11Thread.Start();
        }

        private void Thread9()
        {
            while (EnableAll)
            {
                if (Arm9Enabled)
                {
                    if (Sync) continue;
                    Arm9.Execute();
                    if (Arm11Enabled) Sync = true;
                }
            }
        }

        private void Thread11()
        {
            while (EnableAll)
            {
                if (Arm11Enabled)
                {
                    if (!Sync) continue;
                    Arm11.Execute();
                    Sync = false;
                }
            }
        }

        // TODO: This may cause issues?
        public void Pause()
        {
            EnableAll = false;
        }

        public void Stop()
        {
            EnableAll = false;
            Arm9Thread.Abort();
            Arm11Thread.Abort();
        }
    }
}
