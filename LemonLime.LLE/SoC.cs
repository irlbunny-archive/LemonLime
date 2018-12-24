using System;
using System.Threading;

using LemonLime.ARM;
using LemonLime.Common;
using LemonLime.LLE.Device;
using LemonLime.LLE.Device.Generic;

namespace LemonLime.LLE
{
    public class SoC
    {
        private CPUBus ARM9_Bus, ARM11_Bus;
        private Thread ARM9_Thread, ARM11_Thread;
        private bool ARM9_Enabled, ARM11_Enabled;
        private Interpreter ARM9_Core, ARM11_Core;

        public SoC()
        {
            ARM9_Bus = new CPUBus();
            ARM11_Bus = new CPUBus();

            // ARM9 exclusive memory
            ROM Boot9 = new ROM("boot9.bin", "ARM9 BootROM");
            RAM ITCM9 = new RAM(0x8000, "Instruction TCM");
            RAM DTCM9 = new RAM(0x4000, "Data TCM");
            RAM WRAM9 = new RAM(0x100000, "AHB Work RAM");

            // Quick hack: set up the ARM9 to infinitely spin in place
            for (uint i = 0; i < 32768; i += 4)
                ITCM9.WriteUInt32(i, 0xEAFFFFFE); // b .

            ARM9_Bus.Attach(Boot9, 0xFFFF0000);
            ARM9_Bus.Attach(ITCM9, 0x00000000);
            ARM9_Bus.Attach(ITCM9, 0x01FF8000);
            ARM9_Bus.Attach(ITCM9, 0x07FF8000);
            ARM9_Bus.Attach(DTCM9, 0xFFF00000);
            ARM9_Bus.Attach(WRAM9, 0x08000000);

            // ARM11 exclusive memory
            ROM Boot11 = new ROM("boot11.bin", "ARM11 BootROM");
            ARM11_Bus.Attach(Boot11, 0x00000000);
            ARM11_Bus.Attach(Boot11, 0xFFFF0000);

            // Shared memory
            RAM AXIWRAM = new RAM(0x100000, "AXI Work RAM");
            RAM FCRAM = new RAM(0x08000000, "Fast-Cycle RAM");
            RAM VRAM = new RAM(0x600000, "Video RAM");

            ARM9_Bus.Attach(AXIWRAM, 0x1FF00000);
            ARM9_Bus.Attach(FCRAM, 0x20000000);
            ARM9_Bus.Attach(VRAM, 0x18000000);

            ARM11_Bus.Attach(AXIWRAM, 0x1FF00000);
            ARM11_Bus.Attach(FCRAM, 0x20000000);
            ARM11_Bus.Attach(VRAM, 0x18000000);

            ARM9_Core = new Interpreter(ARM9_Bus);
            ARM11_Core = new Interpreter(ARM11_Bus);

            ARM9_Thread = new Thread(Run9);
            ARM11_Thread = new Thread(Run11);

            ARM9_Enabled = false;
            ARM11_Enabled = false;
        }

        public void SetCPU(CPUType Type, bool Enabled)
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

        public void Start()
        {
            ARM9_Thread.Start();
            ARM11_Thread.Start();
        }

        public void Stop()
        {
            throw new Exception("Stopped CPU emulation");
        }

        public void AssertIRQ(CPUType Type)
        {
            Interpreter Core = (Type == CPUType.ARM9) ? ARM9_Core : ARM11_Core;
            Logger.WriteInfo($"Triggering IRQ on ${Type}");
            Core.IRQ = true;
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
    }
}
