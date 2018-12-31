using System;
using System.Threading;

using LemonLime.ARM;
using LemonLime.Common;
using LemonLime.LLE.CPU;
using LemonLime.LLE.Device.ARM9;
using LemonLime.LLE.Device.Generic;

namespace LemonLime.LLE
{
    public class CTR
    {
        private Bus                ARM9Bus,     ARM11Bus;
        private Thread             ARM9Thread,  ARM11Thread;
        private static bool        ARM9Enabled, ARM11Enabled;
        private static Interpreter ARM9Core,    ARM11Core;

        public CTR()
        {
            ARM9Bus  = new Bus();
            ARM11Bus = new Bus();

            // ARM9 exclusive memory
            ROM Boot9 = new ROM("boot9.bin", "ARM9 BootROM");
            RAM ITCM9 = new RAM(0x8000,      "Instruction TCM");
            RAM DTCM9 = new RAM(0x4000,      "Data TCM");
            RAM WRAM9 = new RAM(0x100000,    "AHB Work RAM");

            CFG9  ARM9CFG9  = new CFG9();
            TIMER ARM9TIMER = new TIMER();

            PXI ARM9PXI  = new PXI(7);
            PXI ARM11PXI = new PXI(6);

            ARM9PXI.AttachEndpoint(ARM11PXI);
            ARM11PXI.AttachEndpoint(ARM9PXI);

            // Quick hack: set up the ARM9 to infinitely spin in place
            for (uint i = 0; i < ITCM9.Size(); i += 4)
                ITCM9.WriteWord(i, 0xEAFFFFFE); // b .

            ARM9Bus.Attach(Boot9, 0xFFFF0000);
            ARM9Bus.Attach(ITCM9, 0x00000000); // ITCM mirrors, these are a hack
            ARM9Bus.Attach(ITCM9, 0x01FF8000); // and should be moved to the
            ARM9Bus.Attach(ITCM9, 0x07FF8000); // MMU/MPU layer
            ARM9Bus.Attach(DTCM9, 0xFFF00000); // same goes to DTCM
            ARM9Bus.Attach(WRAM9, 0x08000000);

            // ARM11 exclusive memory
            ROM Boot11 = new ROM("boot11.bin", "ARM11 BootROM");
            ARM11Bus.Attach(Boot11, 0x00000000);
            ARM11Bus.Attach(Boot11, 0xFFFF0000);

            // Shared memory
            RAM AXIWRAM = new RAM(0x100000,   "AXI Work RAM");
            RAM FCRAM   = new RAM(0x08000000, "Fast-Cycle RAM");
            RAM VRAM    = new RAM(0x600000,   "Video RAM");

            ARM9Bus.Attach(AXIWRAM, 0x1FF00000);
            ARM9Bus.Attach(FCRAM,   0x20000000);
            ARM9Bus.Attach(VRAM,    0x18000000);

            ARM11Bus.Attach(AXIWRAM, 0x1FF00000);
            ARM11Bus.Attach(FCRAM,   0x20000000);
            ARM11Bus.Attach(VRAM,    0x18000000);

            // IO Devices
            ARM9Bus.Attach(ARM9CFG9,  0x10000000);
            ARM9Bus.Attach(ARM9TIMER, 0x10003000);
            ARM9Bus.Attach(ARM9PXI,   0x10008000);
            ARM9Bus.Attach(ARM11PXI,  0x10163000);
            ARM11Bus.Attach(ARM11PXI, 0x10163000);

            ARM9Core  = new Interpreter(ARM9Bus, true);
            ARM11Core = new Interpreter(ARM11Bus);

            ARM9Thread  = new Thread(Run9);
            ARM11Thread = new Thread(Run11);

            ARM9Enabled  = false;
            ARM11Enabled = false;
        }

        /// <summary>
        ///     Enable or disable a CPU.
        /// </summary>
        /// <param name="Type">CPU Type (ARM9/ARM11)</param>
        /// <param name="Enabled">True = Enabled, False = Disabled</param>
        public static void SetCPU(CPU.Type Type, bool Enabled)
        {
            Logger.WriteInfo($"{(Enabled ? "Enabling" : "Disabling")} CPU ({Type})");

            switch (Type)
            {
                case CPU.Type.ARM9:
                    ARM9Enabled = Enabled;
                    break;

                case CPU.Type.ARM11:
                    ARM11Enabled = Enabled;
                    break;
            }
        }

        /// <summary>
        ///     Start emulation.
        /// </summary>
        public void Start()
        {
            ARM9Thread.Start();
            ARM11Thread.Start();
        }

        /// <summary>
        ///     Stop emulation.
        /// </summary>
        public void Stop()
        {
            ARM9Thread.Abort();
            ARM11Thread.Abort();

            throw new Exception("Stopped emulation.");
        }

        /// <summary>
        ///     Assert an IRQ on a CPU.
        /// </summary>
        /// <param name="Type">CPU Type (ARM9/ARM11)</param>
        public static void AssertIRQ(CPU.Type Type)
        {
            Interpreter Core = (Type == CPU.Type.ARM9) ? ARM9Core : ARM11Core;
            if (Core != null)
            {
                Logger.WriteInfo($"Triggering IRQ on {Type}");
                Core.IRQ = true;
            }

            throw new Exception($"Core ({Type}) is not initialized!");
        }

        /// <summary>
        ///     Reset a CPU.
        /// </summary>
        /// <param name="Type">CPU Type (ARM9/ARM11)</param>
        public void Reset(CPU.Type Type)
        {
            Interpreter Core = (Type == CPU.Type.ARM9) ? ARM9Core : ARM11Core;
            Logger.WriteInfo($"Resetting {Type} CPU");
            Core.Reset();
        }

        private void Run9()
        {
            while (true)
            {
                if (ARM9Enabled)
                    ARM9Core.Execute();
                else
                    Thread.Sleep(10);
            }
        }

        private void Run11()
        {
            while (true)
            {
                if (ARM11Enabled)
                    ARM11Core.Execute();
                else
                    Thread.Sleep(10);
            }
        }
    }
}
