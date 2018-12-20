using LemonLime.ARM;
using LemonLime.Common;
using LemonLime.LLE;

namespace LemonLime.LLE
{
    class Memory : IBus
    {
        private CPU.Handler Handler;

        private IO.Handler IO;

        public Memory()
        {
            IO = new IO.Handler();
        }

        public byte ReadUInt8(uint Address)
        {
            if (Type == CPU.Type.Arm9)
            {
                if (Address < 0x08000000)
                {
                    return InstructionTCM[Address];
                }
                else if (Address >= 0x08000000 && Address < 0x08000000 + 0x00100000)
                {
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
                if (Type == CPU.Type.Arm11)
                {
                    if (Address >= 0x17E00000 && Address < 0x17E00000 + 0x00002000)
                    {
                        return MPCore_PrivateMemory[Address - 0x17E00000];
                    }
                }

                return (byte) IO.Call(new IO.Context(Handler, Type, Address, LLE.IO.Type.Read, LLE.IO.Width.Width1));
            }
            else if (Address >= 0x1FF80000 && Address < 0x1FF80000 + 0x00080000)
            {
                return AXIWRAM[Address - 0x1FF80000];
            }
            else if (Address >= 0x20000000 && Address < 0x20000000 + 0x08000000)
            {
                return FCRAM[Address - 0x20000000];
            }

            if (Type == CPU.Type.Arm9)
            {
                if (Address >= 0xFFF00000 && Address < 0xFFF00000 + 0x00004000)
                {
                    return DataTCM[Address - 0xFFF00000];
                }
            }

            if (Type == CPU.Type.Arm9)
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
                if (Type == Type.Arm11)
                {
                    if (Address >= 0x17E00000 && Address < 0x17E00000 + 0x00002000)
                    {
                        return (ushort)(ReadUInt8(Address) |
                            (ReadUInt8(Address + 1) << 8));
                    }
                }

                Context IOInfo = new Context(Handler, Type, Address, LLE.IO.Type.Read, Width.Width2);
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
                if (Type == CPU.Type.Arm11)
                {
                    if (Address >= 0x17E00000 && Address < 0x17E00000 + 0x00002000)
                    {
                        return (uint)(ReadUInt8(Address)   |
                            (ReadUInt8(Address + 1) << 8)  |
                            (ReadUInt8(Address + 2) << 16) |
                            (ReadUInt8(Address + 3) << 24));
                    }
                }

                Context IOInfo = new Context(Handler, Type, Address, LLE.IO.Type.Read, LLE.IO.Width.Width4);
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
            if (Type == Type.Arm9)
            {
                if (Address < 0x08000000)
                {
                    InstructionTCM[Address] = Value;
                    return;
                }
                else if (Address >= 0x08000000 && Address < 0x08000000 + 0x00100000)
                {
                    ARM9_InternalMemory[Address - 0x08000000] = Value;
                    return;
                }
            }

            if (Address >= 0x10000000 && Address < 0x10000000 + 0x08000000)
            {
                if (Type == Type.Arm11)
                {
                    if (Address >= 0x17E00000 && Address < 0x17E00000 + 0x00002000)
                    {
                        MPCore_PrivateMemory[Address - 0x17E00000] = Value;
                        return;
                    }
                }

                IO.Call(new Context(Handler, Type, Address, LLE.IO.Type.Write, Width.Width1, Value));
                return;
            }
            else if (Address >= 0x1FF80000 && Address < 0x1FF80000 + 0x00080000)
            {
                AXIWRAM[Address - 0x1FF80000] = Value;
                return;
            }
            else if (Address >= 0x20000000 && Address < 0x20000000 + 0x08000000)
            {
                FCRAM[Address - 0x20000000] = Value;
                return;
            }

            if (Type == Type.Arm9)
            {
                if (Address >= 0xFFF00000 && Address < 0xFFF00000 + 0x00004000)
                {
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
                if (Type == Type.Arm11)
                {
                    if (Address >= 0x17E00000 && Address < 0x17E00000 + 0x00002000)
                    {
                        WriteUInt8(Address,     (byte)Value);
                        WriteUInt8(Address + 1, (byte)(Value >> 8));
                        return;
                    }
                }

                IO.Call(new Context(Handler, Type, Address, LLE.IO.Type.Write, Width.Width2, 0, Value));
                return;
            }

            WriteUInt8(Address,     (byte)Value);
            WriteUInt8(Address + 1, (byte)(Value >> 8));
        }

        public void WriteUInt32(uint Address, uint Value)
        {
            if (Address >= 0x10000000 && Address < 0x10000000 + 0x08000000)
            {
                if (Type == Type.Arm11)
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

                IO.Call(new Context(Handler, Type, Address, LLE.IO.Type.Write, Width.Width4, 0, 0, Value));
                return;
            }

            WriteUInt8(Address,     (byte)Value);
            WriteUInt8(Address + 1, (byte)(Value >> 8));
            WriteUInt8(Address + 2, (byte)(Value >> 16));
            WriteUInt8(Address + 3, (byte)(Value >> 24));
        }
    }
}
