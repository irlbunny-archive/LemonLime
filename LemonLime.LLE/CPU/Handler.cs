using LemonLime.ARM;
using LemonLime.Common;
using System;
using System.Threading;

namespace LemonLime.LLE.CPU
{
    class Handler
    {
        private Memory Memory;

        private Interpreter Arm9;
        private Interpreter Arm11;

        private bool EnableAll = true;

        private bool Arm9Enabled = false;
        private bool Arm11Enabled = false;

        private bool Sync = false;

        private Thread Arm9Thread;
        private Thread Arm11Thread;

        public Handler(Memory Memory)
        {
            this.Memory = Memory;

            Memory.SetType(Type.Arm9);
            Arm9 = new Interpreter(Memory, true);

            Memory.SetType(Type.Arm11);
            Arm11 = new Interpreter(Memory);

            Arm9Thread = new Thread(Thread9);
            Arm11Thread = new Thread(Thread11);

            Memory.SetHandler(this);
        }

        public void EnableCpu(Type Type, bool Enabled)
        {
            if (Enabled != true) throw new Exception("Disabling CPUs are not allowed.");

            switch (Type)
            {
                case Type.Arm9:
                    Logger.WriteInfo("Enabling ARM9 CPU.");
                    Arm9Enabled = Enabled;
                    break;

                case Type.Arm11:
                    Logger.WriteInfo("Enabling ARM11 CPU.");
                    Arm11Enabled = Enabled;
                    break;
            }
        }

        public void EnableIRQ(Type Type)
        {
            switch (Type)
            {
                case Type.Arm9:
                    if (Arm9Enabled != true) throw new Exception("ARM9 is not enabled.");
                    Logger.WriteInfo("Enabling ARM9 IRQ.");
                    Arm9.IRQ = true;
                    break;

                case Type.Arm11:
                    if (Arm11Enabled != true) throw new Exception("ARM11 is not enabled.");
                    Logger.WriteInfo("Enabling ARM11 IRQ.");
                    Arm11.IRQ = true;
                    break;
            }
        }

        public void Run()
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
                    Memory.SetType(Type.Arm9);
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
                    Memory.SetType(Type.Arm11);
                    Arm11.Execute();
                    Sync = false;
                }
            }
        }

        public void Stop()
        {
            EnableAll = false;
        }
    }
}
