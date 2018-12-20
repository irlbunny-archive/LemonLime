using LemonLime.ARM;
using LemonLime.Common;
using System.Threading;

namespace LemonLime.LLE.CPU
{
    class Handler
    {
        private Memory.Handler ARM9Memory, ARM11Memory;

        private static Interpreter ARM9, ARM11;

        private bool EnableAll = true;

        private static bool ARM9Enabled, ARM11Enabled;

        private bool Sync = false;

        private Thread ARM9Thread, ARM11Thread;

        public Handler(Memory.Handler ARM9Memory, Memory.Handler ARM11Memory)
        {
            this.ARM9Memory  = ARM9Memory;
            this.ARM11Memory = ARM11Memory;

            ARM9  = new Interpreter(ARM9Memory, true);
            ARM11 = new Interpreter(ARM11Memory);

            ARM9Thread  = new Thread(Thread9);
            ARM11Thread = new Thread(Thread11);
        }

        public static void EnableCPU(Type Type, bool Enabled)
        {
            if (Enabled)
                Logger.WriteInfo($"Enabling processor ${Type}.");
            else
                Logger.WriteInfo($"Disabling processor ${Type}.");

            switch (Type)
            {
                case Type.ARM9:
                    ARM9Enabled = Enabled;
                    break;

                case Type.ARM11:
                    ARM11Enabled = Enabled;
                    break;
            }
        }

        public static void SetIrq(Type Type)
        {
            Interpreter Proc = (Type == Type.ARM9) ? ARM9 : ARM11;
            Logger.WriteInfo($"Triggering IRQ on processor ${Type}.");
            Proc.IRQ = true;
        }

        public void Start()
        {
            ARM9Thread.Start();
            ARM11Thread.Start();
        }

        private void Thread9()
        {
            while (EnableAll)
            {
                if (ARM9Enabled)
                {
                    if (Sync) continue;
                    ARM9.Execute();
                    if (ARM11Enabled) Sync = true;
                }
            }
        }

        private void Thread11()
        {
            while (EnableAll)
            {
                if (ARM11Enabled)
                {
                    if (!Sync) continue;
                    ARM11.Execute();
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
            ARM9Thread.Abort();
            ARM11Thread.Abort();
        }
    }
}
