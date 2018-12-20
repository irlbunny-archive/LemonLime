using LemonLime.ARM;
using LemonLime.Common;
using System.Threading;

namespace LemonLime.LLE.CPU
{
    class Handler
    {
        private Memory Arm9Memory, Arm11Memory;

        private static Interpreter Arm9, Arm11;

        private bool EnableAll = true;

        private static bool Arm9Enabled, Arm11Enabled;

        private bool Sync = false;

        private Thread Arm9Thread, Arm11Thread;

        public Handler(Memory Arm9Memory, Memory Arm11Memory)
        {
            this.Arm9Memory  = Arm9Memory;
            this.Arm11Memory = Arm11Memory;

            Arm9  = new Interpreter(Arm9Memory, true);
            Arm11 = new Interpreter(Arm11Memory);

            Arm9Thread  = new Thread(Thread9);
            Arm11Thread = new Thread(Thread11);
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
