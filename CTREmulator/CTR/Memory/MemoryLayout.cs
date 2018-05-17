using CTREmulator.ARM;
using CTREmulator.CTR.BootROM;
using System;
using System.Collections.Generic;
using System.Text;

namespace CTREmulator.CTR.Memory
{
    class MemoryLayout : IBus
    {
        private Memory Memory;
        private LayoutTypes ProcessorId;
        private ARM11 BootROM11;
        private ARM9 BootROM9;

        public MemoryLayout(Memory Memory, LayoutTypes ProcessorId)
        {
            this.Memory = Memory;
            this.ProcessorId = ProcessorId;
            if (ProcessorId == LayoutTypes.ARM11)
            {
                BootROM11 = new ARM11();
            }
            else
            {
                BootROM9 = new ARM9();
            }
        }

        public byte ReadUInt8(uint Address)
        {
            Address &= 0x3fffffff;

            Logging.WriteInfo($"Read @ 0x{Address.ToString("X4")}");

            if (ProcessorId == LayoutTypes.ARM11)
            {
                if (Address >= 0x00000000 && Address < 0x00010000)
                    return BootROM11.ARM11_BootROM[Address];
                if (Address >= 0x00010000 && Address < 0x00020000)
                    return BootROM11.ARM11_BootROM[Address - 0x00010000];
            }
            if (Address >= 0x20000000 && Address < 0x28000000)
                return Memory.FCRAM[Address];
            if (ProcessorId == LayoutTypes.ARM11)
            {
                if (Address >= 0xFFFF0000 && Address < 0x10000000)
                    return BootROM11.ARM11_BootROM[Address - 0xFFFF0000];
            }
            else
            {
                if (Address >= 0xFFFF0000 && Address < 0x10000000)
                    return BootROM9.ARM9_BootROM[Address - 0xFFFF0000]; // this starts at 0x8000?
            }

            return 0;
        }

        public uint ReadUInt32(uint Address)
        {
            return (uint)(ReadUInt8(Address) |
                (ReadUInt8(Address + 1) << 8) |
                (ReadUInt8(Address + 2) << 16) |
                (ReadUInt8(Address + 3) << 24));
        }

        public void WriteUInt8(uint Address, byte Value)
        {
            Address &= 0x3fffffff;

            Logging.WriteInfo($"Write @ 0x{Address.ToString("X")}, Value = {Value.ToString("X")}");

            if (ProcessorId == LayoutTypes.ARM11)
            {
                if (Address >= 0x00000000 && Address < 0x00010000)
                    BootROM11.ARM11_BootROM[Address] = Value;
                if (Address >= 0x00010000 && Address < 0x00020000)
                    BootROM11.ARM11_BootROM[Address - 0x00010000] = Value;
            }
            if (Address >= 0x20000000 && Address < 0x28000000)
                Memory.FCRAM[Address] = Value;
            if (ProcessorId == LayoutTypes.ARM11)
            {
                if (Address >= 0xFFFF0000 && Address < 0x10000000)
                    BootROM11.ARM11_BootROM[Address - 0xFFFF0000] = Value;
            }
            else
            {
                if (Address >= 0xFFFF0000 && Address < 0x10000000)
                    BootROM9.ARM9_BootROM[Address - 0xFFFF0000] = Value; // this starts at 0x8000?
            }
        }

        public void WriteUInt32(uint Address, uint Value)
        {
            WriteUInt8(Address, (byte)Value);
            WriteUInt8(Address + 1, (byte)(Value >> 8));
            WriteUInt8(Address + 2, (byte)(Value >> 16));
            WriteUInt8(Address + 3, (byte)(Value >> 24));
        }

        public void CopyData(uint Source, uint Destination, uint Length)
        {
            Buffer.BlockCopy(Memory.FCRAM, (int)Source, Memory.FCRAM, (int)Destination, (int)Length);
        }

        public byte[] GetData(uint Address, uint Length)
        {
            Address &= 0x3fffffff;

            byte[] Output = new byte[Length];
            Buffer.BlockCopy(Memory.FCRAM, (int)Address, Output, 0, Output.Length);
            return Output;
        }
    }
}
