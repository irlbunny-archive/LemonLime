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
            else if (ProcessorId == LayoutTypes.ARM9)
            {
                BootROM9 = new ARM9();
            }
        }

        public byte ReadUInt8(uint Address)
        {
            Address &= 0x3fffffff;

            Logging.WriteInfo($"Read @ 0x{Address.ToString("X")}");

            if (ProcessorId == LayoutTypes.ARM11)
            {
                if (Address >= 0x00000000 && Address < 0x00020000)
                    return BootROM11.ARM11_BootROM[Address & 0x0000FFFF];
                if (Address >= 0x17E00000 && Address < 0x17E02000)
                    return Memory.ARM11_PRIV_MEM[Address & 0x00001FFF];
                if (Address >= 0x1FF80000 && Address < 0x20000000)
                    return Memory.AXI_WRAM[Address & 0x0007FFFF];
                if (Address >= 0x20000000 && Address < 0x28000000)
                    return Memory.FCRAM[Address & 0x07FFFFFF];
                if (Address >= 0xFFFF0000)
                    return BootROM11.ARM11_BootROM[Address & 0x0000FFFF];
            }
            else
            {
                if (Address < 0x08000000)
                    return Memory.ARM9_ITCM[Address & 0x00007FFF];
                if (Address >= 0x1FF80000 && Address < 0x20000000)
                    return Memory.AXI_WRAM[Address & 0x0007FFFF];
                if (Address >= 0x20000000 && Address < 0x28000000)
                    return Memory.FCRAM[Address & 0x07FFFFFF];
                if (Address >= 0xFFFF0000)
                    return BootROM9.ARM9_BootROM[Address & 0x0000FFFF]; // this starts at 0x8000?
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
                    BootROM11.ARM11_BootROM[Address & 0x0000FFFF] = Value;
                else if (Address >= 0x17E00000 && Address < 0x17E02000)
                    Memory.ARM11_PRIV_MEM[Address & 0x00001FFF] = Value;
                else if (Address >= 0x1FF80000 && Address < 0x20000000)
                    Memory.AXI_WRAM[Address & 0x0007FFFF] = Value;
                else if (Address >= 0x20000000 && Address < 0x28000000)
                    Memory.FCRAM[Address & 0x07FFFFFF] = Value;
                else if (Address >= 0xFFFF0000 && Address < 0x10000000)
                    BootROM11.ARM11_BootROM[Address - 0xFFFF0000] = Value;
                else return;
            }
            else if(ProcessorId == LayoutTypes.ARM9)
            {
                if (Address < 0x08000000)
                    Memory.ARM9_ITCM[Address & 0x00007FFF] = Value;
                else if (Address >= 0x1FF80000 && Address < 0x20000000)
                    Memory.AXI_WRAM[Address & 0x0007FFFF] = Value;
                else if (Address >= 0x20000000 && Address < 0x28000000)
                    Memory.FCRAM[Address & 0x07FFFFFF] = Value;
                else if (Address >= 0xFFFF0000)
                    BootROM9.ARM9_BootROM[Address - 0xFFFF0000] = Value; // this starts at 0x8000?
                else return;
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
