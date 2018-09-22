using LemonLime.ARM;
using LemonLime.Common;
using LemonLime.LLE.Device;
using System;
using System.Threading;

namespace LemonLime.LLE
{
    class CPUHandler
    {
        private CPUBus ARM9_Bus, ARM11_Bus;
        private Thread ARM9_Thread, ARM11_Thread;
        private bool ARM9_Enabled, ARM11_Enabled;
        private Interpreter ARM9_Core, ARM11_Core;

        public CPUHandler()
        {
            ARM9_Bus = new CPUBus();
            ARM11_Bus = new CPUBus();

            // ARM9 exclusive memory
            CPUROM Boot9 = new CPUROM("boot9.bin");
            CPURAM ITCM9 = new CPURAM(32768);
            CPURAM DTCM9 = new CPURAM(16384);
            CPURAM WRAM9 = new CPURAM(0x100000);

            // Quick hack: set up the ARM9 to infinitely spin in place
            for (uint i = 0; i < 32768; i++)
                ITCM9.WriteUInt32(i, 0xEAFFFFFE); // b .

            ARM9_Bus.Attach(Boot9, 0xFFFF0000, 0xFFFFFFFF);
            ARM9_Bus.Attach(ITCM9, 0x00000000, 0x00007FFF);
            ARM9_Bus.Attach(ITCM9, 0x01FF8000, 0x01FFFFFF);
            ARM9_Bus.Attach(ITCM9, 0x07FF8000, 0x07FFFFFF);
            ARM9_Bus.Attach(DTCM9, 0xFFF00000, 0xFFF03FFF);
            ARM9_Bus.Attach(WRAM9, 0x08000000, 0x080FFFFF);

            // ARM11 exclusive memory
            CPUROM Boot11 = new CPUROM("boot11.bin");
            ARM11_Bus.Attach(Boot11, 0x00000000, 0x0000FFFF);
            ARM11_Bus.Attach(Boot11, 0xFFFF0000, 0xFFFFFFFF);

            // Shared memory
            CPURAM AXIWRAM = new CPURAM(0x100000);
            CPURAM FCRAM = new CPURAM(0x08000000);
            CPURAM VRAM = new CPURAM(0x600000);

            ARM9_Bus.Attach(AXIWRAM, 0x1FF00000, 0x1FFFFFFF);
            ARM9_Bus.Attach(FCRAM, 0x20000000, 0x27FFFFFF);
            ARM9_Bus.Attach(VRAM, 0x18000000, 0x185FFFFF);

            ARM11_Bus.Attach(AXIWRAM, 0x1FF00000, 0x1FFFFFFF);
            ARM11_Bus.Attach(FCRAM, 0x20000000, 0x27FFFFFF);
            ARM11_Bus.Attach(VRAM, 0x18000000, 0x185FFFFF);

            ARM9_Core = new Interpreter(ARM9_Bus);
            ARM11_Core = new Interpreter(ARM11_Bus);

            ARM9_Thread = new Thread(Run9);
            ARM11_Thread = new Thread(Run11);

            ARM9_Enabled = false;
            ARM11_Enabled = false;
        }

        public void EnableCPU(CPUType Type, bool Enabled)
        {
            Logger.WriteInfo($"Toggling {Type} {Enabled}");
            switch(Type) {
                case CPUType.ARM9:
                    ARM9_Enabled = Enabled;
                    break;

                case CPUType.ARM11:
                    ARM11_Enabled = Enabled;
                    break;
            }
        }

        public void EnableIRQ(CPUType Type)
        {
            Interpreter Core = (Type == CPUType.ARM9) ? ARM9_Core : ARM11_Core;
            Logger.WriteInfo($"Triggering IRQ on ${Type}");
            Core.IRQ = true;
        }

        public void Start()
        {
            ARM9_Thread.Start();
            ARM11_Thread.Start();
        }

        private void Run9()
        {
            while(true) {
                if (ARM9_Enabled) {
                    ARM9_Core.Execute();
                } else {
                    Thread.Sleep(10);
                }

            }
        }

        private void Run11()
        {
            while(true) {
                if (ARM11_Enabled) {
                    ARM11_Core.Execute();
                } else {
                    Thread.Sleep(10);
                }
            }
        }

        public void Stop()
        {
            throw new Exception("Stopped CPU emulation");
        }
    }
}
