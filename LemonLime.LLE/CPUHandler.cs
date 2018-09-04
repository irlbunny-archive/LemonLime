using LemonLime.ARM;
using LemonLime.Common;
using System;
using System.Threading;

namespace LemonLime.LLE
{
    class CPUHandler
    {
        private Memory Memory;

        private Interpreter ARM9;

        private Interpreter ARM11;

        private bool EnableAll = true;

        private bool ARM9_Enabled = false;

        private bool ARM11_Enabled = false;

        private bool Sync = false;

        private Thread ARM9_Thread;

        private Thread ARM11_Thread;

        public CPUHandler(Memory Memory)
        {
            this.Memory = Memory;

            Memory.SetType(CPUType.ARM9);

            ARM9 = new Interpreter(Memory, true);

            Memory.SetType(CPUType.ARM11);

            ARM11 = new Interpreter(Memory);

            ARM9_Thread = new Thread(Run9);

            ARM11_Thread = new Thread(Run11);

            Memory.SetHandler(this);
        }

        public void EnableCPU(CPUType Type, bool Enabled)
        {
            if (Enabled != true) throw new Exception("Disabling CPUs are not allowed.");

            switch (Type)
            {
                case CPUType.ARM9:
                    Logger.WriteInfo("Enabling ARM9 CPU.");
                    ARM9_Enabled = Enabled;
                    break;

                case CPUType.ARM11:
                    Logger.WriteInfo("Enabling ARM11 CPU.");
                    ARM11_Enabled = Enabled;
                    break;
            }
        }

        public void EnableIRQ(CPUType Type)
        {
            switch (Type)
            {
                case CPUType.ARM9:
                    if (ARM9_Enabled != true) throw new Exception("ARM9 is not enabled.");
                    Logger.WriteInfo("Enabling ARM9 IRQ.");
                    ARM9.IRQ = true;
                    break;

                case CPUType.ARM11:
                    if (ARM11_Enabled != true) throw new Exception("ARM11 is not enabled.");
                    Logger.WriteInfo("Enabling ARM11 IRQ.");
                    ARM11.IRQ = true;
                    break;
            }
        }

        public void Start()
        {
            ARM9_Thread.Start();
            ARM11_Thread.Start();
        }

        private void Run9()
        {
            while (EnableAll)
            {
                if (ARM9_Enabled)
                {
                    if (Sync) continue;
                    Memory.SetType(CPUType.ARM9);
                    ARM9.Execute();
                    if (ARM11_Enabled) Sync = true;
                }
            }
        }

        private void Run11()
        {
            while (EnableAll)
            {
                if (ARM11_Enabled)
                {
                    if (!Sync) continue;
                    Memory.SetType(CPUType.ARM11);
                    ARM11.Execute();
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
