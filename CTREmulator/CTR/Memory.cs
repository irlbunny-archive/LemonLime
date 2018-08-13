using CTREmulator.ARM;
using System;

namespace CTREmulator.CTR
{
    class Memory : IBus
    {
        private BootROM.ARM9 BootROM9;

        private byte[] DataTCM = new byte[0x00004000];

        public Memory()
        {
            BootROM9 = new BootROM.ARM9();
        }

        public byte ReadUInt8(uint Address)
        {
            if (Address < 0x08000000)
            {
                Logging.WriteInfo($"Instruction TCM @ 0x{Address.ToString("X")}");

                return 0;
            }
            else if (Address >= 0x01FF8000 && Address < 0x01FF8000 + 0x00008000)
            {
                Logging.WriteInfo($"Instruction TCM (Kernel & Process) @ 0x{Address.ToString("X")}");

                return 0;
            }
            else if (Address >= 0x07FF8000 && Address < 0x07FF8000 + 0x00008000)
            {
                Logging.WriteInfo($"Instruction TCM (BootROM) @ 0x{Address.ToString("X")}");

                return 0;
            }
            else if (Address >= 0x08000000 && Address < 0x08000000 + 0x00100000)
            {
                Logging.WriteInfo($"ARM9 Internal Memory @ 0x{Address.ToString("X")}");

                return 0;
            }
            else if (Address >= 0x10000000 && Address < 0x10000000 + 0x08000000)
            {
                Logging.WriteInfo($"IO Memory @ 0x{Address.ToString("X")}");

                return 0;
            }
            else if (Address >= 0x18000000 && Address < 0x18000000 + 0x00600000)
            {
                Logging.WriteInfo($"VRAM @ 0x{Address.ToString("X")}");

                return 0;
            }
            else if (Address >= 0x20000000 && Address < 0x20000000 + 0x08000000)
            {
                Logging.WriteInfo($"FCRAM @ 0x{Address.ToString("X")}");

                return 0;
            }
            else if (Address >= 0xFFF00000 && Address < 0xFFF00000 + 0x00004000)
            {
                Logging.WriteInfo($"Data TCM (BootROM Mapped) @ 0x{Address.ToString("X")}");

                return DataTCM[Address - 0xFFF00000];
            }
            else if (Address >= 0xFFFF0000)
            {
                return BootROM9.ARM9_BootROM[Address - 0xFFFF0000];
            }

            Logging.WriteInfo($"Read @ 0x{Address.ToString("X")}");

            return 0;
        }

        public void WriteUInt8(uint Address, byte Value)
        {
            if (Address < 0x08000000)
            {
                Logging.WriteInfo($"Instruction TCM @ 0x{Address.ToString("X")}, Value = {Value.ToString("X")}");
                return;
            }
            else if (Address >= 0x01FF8000 && Address < 0x01FF8000 + 0x00008000)
            {
                Logging.WriteInfo($"Instruction TCM (Kernel & Process) @ 0x{Address.ToString("X")}, Value = {Value.ToString("X")}");
                return;
            }
            else if (Address >= 0x07FF8000 && Address < 0x07FF8000 + 0x00008000)
            {
                Logging.WriteInfo($"Instruction TCM (BootROM) @ 0x{Address.ToString("X")}, Value = {Value.ToString("X")}");
                return;
            }
            else if (Address >= 0x08000000 && Address < 0x08000000 + 0x00100000)
            {
                Logging.WriteInfo($"ARM9 Internal Memory @ 0x{Address.ToString("X")}, Value = {Value.ToString("X")}");
                return;
            }
            else if (Address >= 0x10000000 && Address < 0x10000000 + 0x08000000)
            {
                Logging.WriteInfo($"IO Memory @ 0x{Address.ToString("X")}, Value = {Value.ToString("X")}");
                return;
            }
            else if (Address >= 0x18000000 && Address < 0x18000000 + 0x00600000)
            {
                Logging.WriteInfo($"VRAM @ 0x{Address.ToString("X")}, Value = {Value.ToString("X")}");
                return;
            }
            else if (Address >= 0x20000000 && Address < 0x20000000 + 0x08000000)
            {
                Logging.WriteInfo($"FCRAM @ 0x{Address.ToString("X")}, Value = {Value.ToString("X")}");
                return;
            }
            else if (Address >= 0xFFF00000 && Address < 0xFFF00000 + 0x00004000)
            {
                Logging.WriteInfo($"Data TCM (BootROM Mapped) @ 0x{Address.ToString("X")}, Value = {Value.ToString("X")}");

                DataTCM[Address - 0xFFF00000] = Value;
                return;
            }

            Logging.WriteInfo($"Write @ 0x{Address.ToString("X")}, Value = {Value.ToString("X")}");
        }
    }
}
