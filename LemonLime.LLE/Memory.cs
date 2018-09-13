using LemonLime.ARM;
using LemonLime.Common;
using LemonLime.LLE.IO;
using System.IO;

namespace LemonLime.LLE
{
    public class Memory : IBus
    {
        private CPUHandler CPU;

        private IOHandler IO;

        private BootROM.ARM9 BootROM9;

        private BootROM.ARM11 BootROM11;

        private CPUType Type;

        private byte[] InstructionTCM = new byte[0x08000000]; // TODO: Repeat each 0x8000 bytes?

        private byte[] ARM9_InternalMemory = new byte[0x00100000];

        private byte[] MPCore_PrivateMemory = new byte[0x00002000];

        private byte[] AXIWRAM = new byte[0x00080000];

        private byte[] FCRAM = new byte[0x08000000];

        private byte[] DataTCM = new byte[0x00004000];

        public byte[] TopScreenVRAM = new byte[0x0008CA00];

        private byte[] ARM9_Payload = File.ReadAllBytes("arm9loaderhax.bin");

        public Memory()
        {
            BootROM9 = new BootROM.ARM9();

            BootROM11 = new BootROM.ARM11();

            IO = new IOHandler();
        }

        public void SetType(CPUType Type)
        {
            this.Type = Type;
        }

        public void SetHandler(CPUHandler CPU)
        {
            this.CPU = CPU;
        }

        public byte ReadUInt8(uint Address)
        {
            if (Type == CPUType.ARM9)
            {
                if (Address < 0x08000000)
                {
                    // Logger.WriteInfo($"InstructionTCM - Read [{Type}] @ 0x{Address.ToString("X8")}");
                    return InstructionTCM[Address];
                }
                else if (Address >= 0x08000000 && Address < 0x08000000 + 0x00100000)
                {
                    // Logger.WriteInfo($"ARM9_InternalMemory - Read [{Type}] @ 0x{Address.ToString("X8")}");
                    return ARM9_InternalMemory[Address - 0x08000000];
                }
            }
            else
            {
                if (Address < 0x00010000)
                {
                    return BootROM11.BootROM[Address];
                }
                else if (Address >= 0x00010000 && Address < 0x00010000 + 0x00010000)
                {
                    return BootROM11.BootROM[Address - 0x00010000];
                }
            }

            if (Address >= 0x10000000 && Address < 0x10000000 + 0x08000000)
            {
                if (Type == CPUType.ARM11)
                {
                    if (Address >= 0x17E00000 && Address < 0x17E00000 + 0x00002000)
                    {
                        // Logger.WriteInfo($"MPCore_PrivateMemory - Read [{Type}] @ 0x{Address.ToString("X8")}");
                        return MPCore_PrivateMemory[Address - 0x17E00000];
                    }
                }

                IOData IOInfo = new IOData(CPU, Type, Address, IOType.Read, IOWidth.Width1);
                IO.Call(IOInfo);
                return IOInfo.Read8;
            }
            else if (Address >= 0x1FF80000 && Address < 0x1FF80000 + 0x00080000)
            {
                // Logger.WriteInfo($"AXIWRAM - Read [{Type}] @ 0x{Address.ToString("X8")}");
                return AXIWRAM[Address - 0x1FF80000];
            }
            else if (Address >= 0x20000000 && Address < 0x20000000 + 0x08000000)
            {
                if (Address >= 0x23F00000 && Address < 0x23F00000 + ARM9_Payload.Length)
                {
                    // Logger.WriteInfo($"ARM9_Payload - Read [{Type}] @ 0x{Address.ToString("X8")}");
                    return ARM9_Payload[Address - 0x23F00000];
                }

                if (Address >= 0x20000000 && Address < 0x20000000 + 0x0008CA00)
                {
                    // Logger.WriteInfo($"VRAM [Top Screen] - Read [{Type}] @ 0x{Address.ToString("X8")}");
                    return TopScreenVRAM[Address - 0x20000000];
                }

                // Logger.WriteInfo($"FCRAM - Read [{Type}] @ 0x{Address.ToString("X8")}");
                return FCRAM[Address - 0x20000000];
            }

            if (Type == CPUType.ARM9)
            {
                if (Address >= 0xFFF00000 && Address < 0xFFF00000 + 0x00004000)
                {
                    // Logger.WriteInfo($"DataTCM - Read [{Type}] @ 0x{Address.ToString("X8")}");
                    return DataTCM[Address - 0xFFF00000];
                }
            }

            if (Type == CPUType.ARM9)
            {
                if (Address >= 0xFFFF0000)
                {
                    return BootROM9.BootROM[Address - 0xFFFF0000];
                }
            }
            else
            {
                if (Address >= 0xFFFF0000)
                {
                    return BootROM11.BootROM[Address - 0xFFFF0000];
                }
            }

            Logger.WriteInfo($"Read [{Type}] @ 0x{Address.ToString("X8")}");

            return 0;
        }

        public ushort ReadUInt16(uint Address)
        {
            if (Address >= 0x10000000 && Address < 0x10000000 + 0x08000000)
            {
                if (Type == CPUType.ARM11)
                {
                    if (Address >= 0x17E00000 && Address < 0x17E00000 + 0x00002000)
                    {
                        return (ushort)(ReadUInt8(Address) |
                            (ReadUInt8(Address + 1) << 8));
                    }
                }

                IOData IOInfo = new IOData(CPU, Type, Address, IOType.Read, IOWidth.Width2);
                IO.Call(IOInfo);
                return IOInfo.Read16;
            }

            return (ushort)(ReadUInt8(Address) |
                (ReadUInt8(Address + 1) << 8));
        }

        public uint ReadUInt32(uint Address)
        {
            if (Address >= 0x10000000 && Address < 0x10000000 + 0x08000000)
            {
                if (Type == CPUType.ARM11)
                {
                    if (Address >= 0x17E00000 && Address < 0x17E00000 + 0x00002000)
                    {
                        return (uint)(ReadUInt8(Address)   |
                            (ReadUInt8(Address + 1) << 8)  |
                            (ReadUInt8(Address + 2) << 16) |
                            (ReadUInt8(Address + 3) << 24));
                    }
                }

                IOData IOInfo = new IOData(CPU, Type, Address, IOType.Read, IOWidth.Width4);
                IO.Call(IOInfo);
                return IOInfo.Read32;
            }

            return (uint)(ReadUInt8(Address)   |
                (ReadUInt8(Address + 1) << 8)  |
                (ReadUInt8(Address + 2) << 16) |
                (ReadUInt8(Address + 3) << 24));
        }

        public void WriteUInt8(uint Address, byte Value)
        {
            if (Type == CPUType.ARM9)
            {
                if (Address < 0x08000000)
                {
                    // Logger.WriteInfo($"InstructionTCM - Write [{Type}] @ 0x{Address.ToString("X8")}, Value = {Value.ToString("X")}");
                    InstructionTCM[Address] = Value;
                    return;
                }
                else if (Address >= 0x08000000 && Address < 0x08000000 + 0x00100000)
                {
                    // Logger.WriteInfo($"ARM9_InternalMemory - Write [{Type}] @ 0x{Address.ToString("X8")}, Value = {Value.ToString("X")}");
                    ARM9_InternalMemory[Address - 0x08000000] = Value;
                    return;
                }
            }

            if (Address >= 0x10000000 && Address < 0x10000000 + 0x08000000)
            {
                if (Type == CPUType.ARM11)
                {
                    if (Address >= 0x17E00000 && Address < 0x17E00000 + 0x00002000)
                    {
                        // Logger.WriteInfo($"MPCore_PrivateMemory- Write [{Type}] @ 0x{Address.ToString("X8")}, Value = {Value.ToString("X")}");
                        MPCore_PrivateMemory[Address - 0x17E00000] = Value;
                        return;
                    }
                }

                IO.Call(new IOData(CPU, Type, Address, IOType.Write, IOWidth.Width1, Value));
                return;
            }
            else if (Address >= 0x1FF80000 && Address < 0x1FF80000 + 0x00080000)
            {
                // Logger.WriteInfo($"AXIWRAM - Write [{Type}] @ 0x{Address.ToString("X8")}, Value = {Value.ToString("X")}");
                AXIWRAM[Address - 0x1FF80000] = Value;
                return;
            }
            else if (Address >= 0x20000000 && Address < 0x20000000 + 0x08000000)
            {
                if (Address >= 0x23F00000 && Address < 0x23F00000 + ARM9_Payload.Length)
                {
                    // Logger.WriteInfo($"ARM9_Payload - Write [{Type}] @ 0x{Address.ToString("X8")}, Value = {Value.ToString("X")}");
                    ARM9_Payload[Address - 0x23F00000] = Value;
                    return;
                }

                if (Address >= 0x20000000 && Address < 0x20000000 + 0x0008CA00)
                {
                    // Logger.WriteInfo($"VRAM [Top Screen] - Write [{Type}] @ 0x{Address.ToString("X8")}");
                    TopScreenVRAM[Address - 0x20000000] = Value;
                    return;
                }

                // Logger.WriteInfo($"FCRAM - Write [{Type}] @ 0x{Address.ToString("X8")}, Value = {Value.ToString("X")}");
                FCRAM[Address - 0x20000000] = Value;
                return;
            }

            if (Type == CPUType.ARM9)
            {
                if (Address >= 0xFFF00000 && Address < 0xFFF00000 + 0x00004000)
                {
                    // Logger.WriteInfo($"DataTCM - Write [{Type}] @ 0x{Address.ToString("X8")}, Value = {Value.ToString("X")}");
                    DataTCM[Address - 0xFFF00000] = Value;
                    return;
                }
            }

            Logger.WriteInfo($"Write [{Type}] @ 0x{Address.ToString("X8")}, Value = {Value.ToString("X")}");
        }

        public void WriteUInt16(uint Address, ushort Value)
        {
            if (Address >= 0x10000000 && Address < 0x10000000 + 0x08000000)
            {
                if (Type == CPUType.ARM11)
                {
                    if (Address >= 0x17E00000 && Address < 0x17E00000 + 0x00002000)
                    {
                        WriteUInt8(Address,     (byte)Value);
                        WriteUInt8(Address + 1, (byte)(Value >> 8));
                        return;
                    }
                }

                IO.Call(new IOData(CPU, Type, Address, IOType.Write, IOWidth.Width2, 0, Value));
                return;
            }

            WriteUInt8(Address,     (byte)Value);
            WriteUInt8(Address + 1, (byte)(Value >> 8));
        }

        public void WriteUInt32(uint Address, uint Value)
        {
            if (Address >= 0x10000000 && Address < 0x10000000 + 0x08000000)
            {
                if (Type == CPUType.ARM11)
                {
                    if (Address >= 0x17E00000 && Address < 0x17E00000 + 0x00002000)
                    {
                        WriteUInt8(Address,     (byte)Value);
                        WriteUInt8(Address + 1, (byte)(Value >> 8));
                        WriteUInt8(Address + 2, (byte)(Value >> 16));
                        WriteUInt8(Address + 3, (byte)(Value >> 24));
                        return;
                    }
                }

                IO.Call(new IOData(CPU, Type, Address, IOType.Write, IOWidth.Width4, 0, 0, Value));
                return;
            }

            WriteUInt8(Address,     (byte)Value);
            WriteUInt8(Address + 1, (byte)(Value >> 8));
            WriteUInt8(Address + 2, (byte)(Value >> 16));
            WriteUInt8(Address + 3, (byte)(Value >> 24));
        }
    }
}
