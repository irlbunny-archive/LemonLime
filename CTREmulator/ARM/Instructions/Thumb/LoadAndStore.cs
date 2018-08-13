namespace CTREmulator.ARM
{
    public partial class Interpreter
    {
        //Load

        /// <summary>
        ///     Load Multiple Increment After.
        /// </summary>
        private void Thumb_LDMIA()
        {
            byte RegisterList = (byte)(Opcode & 0xff);
            int Rn = (int)((Opcode >> 8) & 7);

            uint Count = 0;
            for (int Index = 0; Index < 8; Index++)
            {
                if ((RegisterList & (1 << Index)) != 0) Count += 4;
            }

            uint Address = Registers[Rn];
            Registers[Rn] += Count;

            for (int Index = 0; Index < 8; Index++)
            {
                if ((RegisterList & (1 << Index)) != 0)
                {
                    Registers[Index] = ReadUInt32E(Address);
                    Address += 4;
                }
            }
        }

        /// <summary>
        ///     Load Register (1).
        /// </summary>
        private void Thumb_LDR()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            uint Immediate = (Opcode >> 6) & 0x1f;

            uint Address = Registers[Rn] + (Immediate << 2);
            Registers[Rd] = ROR(ReadUInt32E(Address & 0xfffffffc), (int)(Address & 3) << 3);
        }

        /// <summary>
        ///     Load Register (2).
        /// </summary>
        private void Thumb_LDR_2()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            int Rm = (int)((Opcode >> 6) & 7);

            uint Address = Registers[Rn] + Registers[Rm];
            Registers[Rd] = ROR(ReadUInt32E(Address & 0xfffffffc), (int)(Address & 3) << 3);
        }

        /// <summary>
        ///     Load Register (3).
        /// </summary>
        private void Thumb_LDR_3()
        {
            uint Immediate = Opcode & 0xff;
            int Rd = (int)((Opcode >> 8) & 7);

            uint Address = (Registers[15] & 0xfffffffc) + (Immediate << 2);
            Registers[Rd] = ROR(ReadUInt32E(Address & 0xfffffffc), (int)(Address & 3) << 3);
        }

        /// <summary>
        ///     Load Register (4).
        /// </summary>
        private void Thumb_LDR_4()
        {
            uint Immediate = Opcode & 0xff;
            int Rd = (int)((Opcode >> 8) & 7);

            uint Address = Registers[13] + (Immediate << 2);
            Registers[Rd] = ROR(ReadUInt32E(Address & 0xfffffffc), (int)(Address & 3) << 3);
        }

        /// <summary>
        ///     Load Register Byte (1).
        /// </summary>
        private void Thumb_LDRB()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            uint Immediate = (Opcode >> 6) & 0x1f;

            uint Address = Registers[Rn] + Immediate;
            Registers[Rd] = Bus.ReadUInt8(Address);
        }

        /// <summary>
        ///     Load Register Byte (2).
        /// </summary>
        private void Thumb_LDRB_2()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            int Rm = (int)((Opcode >> 6) & 7);

            uint Address = Registers[Rn] + Registers[Rm];
            Registers[Rd] = Bus.ReadUInt8(Address);
        }

        /// <summary>
        ///     Load Register Halfword (1).
        /// </summary>
        private void Thumb_LDRH()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            uint Immediate = (Opcode >> 6) & 0x1f;

            uint Address = Registers[Rn] + (Immediate << 1);
            Registers[Rd] = ReadUInt16E(Address);
        }

        /// <summary>
        ///     Load Register Halfword (2).
        /// </summary>
        private void Thumb_LDRH_2()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            int Rm = (int)((Opcode >> 6) & 7);

            uint Address = Registers[Rn] + Registers[Rm];
            Registers[Rd] = ReadUInt16E(Address);
        }

        /// <summary>
        ///     Load Register Signed Byte.
        /// </summary>
        private void Thumb_LDRSB()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            int Rm = (int)((Opcode >> 6) & 7);

            uint Address = Registers[Rn] + Registers[Rm];
            sbyte Value = (sbyte)Bus.ReadUInt8(Address);
            Registers[Rd] = (uint)Value;
        }

        /// <summary>
        ///     Load Register Signed Halfword.
        /// </summary>
        private void Thumb_LDRSH()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            int Rm = (int)((Opcode >> 6) & 7);

            uint Address = Registers[Rn] + Registers[Rm];
            short Value = (short)ReadUInt16E(Address);
            Registers[Rd] = (uint)Value;
        }

        /// <summary>
        ///     Set Endian.
        /// </summary>
        private void Thumb_SETEND()
        {
            Registers.SetFlag(ARMFlag.Endianness, IsOpcodeBitSet(3));
        }

        //Store

        /// <summary>
        ///     Store Multiple Increment After.
        /// </summary>
        private void Thumb_STMIA()
        {
            byte RegisterList = (byte)(Opcode & 0xff);
            int Rn = (int)((Opcode >> 8) & 7);

            uint Count = 0;
            for (int Index = 0; Index < 8; Index++)
            {
                if ((RegisterList & (1 << Index)) != 0) Count++;
            }

            uint Address = Registers[Rn];
            Registers[Rn] += Count << 2;

            for (int Index = 0; Index < 8; Index++)
            {
                if ((RegisterList & (1 << Index)) != 0)
                {
                    WriteUInt32E(Address, Registers[Index]);
                    Address += 4;
                }
            }
        }

        /// <summary>
        ///     Store Register (1).
        /// </summary>
        private void Thumb_STR()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            uint Immediate = (Opcode >> 6) & 0x1f;

            uint Address = Registers[Rn] + (Immediate << 2);
            WriteUInt32(Address, Registers[Rd]);
        }

        /// <summary>
        ///     Store Register (2).
        /// </summary>
        private void Thumb_STR_2()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            int Rm = (int)((Opcode >> 6) & 7);

            uint Address = Registers[Rn] + Registers[Rm];
            WriteUInt32E(Address, Registers[Rd]);
        }

        /// <summary>
        ///     Store Register (3).
        /// </summary>
        private void Thumb_STR_3()
        {
            uint Immediate = Opcode & 0xff;
            int Rd = (int)((Opcode >> 8) & 7);

            uint Address = Registers[13] + (Immediate << 2);
            WriteUInt32E(Address, Registers[Rd]);
        }

        /// <summary>
        ///     Store Register Byte (1).
        /// </summary>
        private void Thumb_STRB()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            uint Immediate = (Opcode >> 6) & 0x1f;

            uint Address = Registers[Rn] + Immediate;
            Bus.WriteUInt8(Address, (byte)Registers[Rd]);
        }

        /// <summary>
        ///     Store Register Byte (2).
        /// </summary>
        private void Thumb_STRB_2()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            int Rm = (int)((Opcode >> 6) & 7);

            uint Address = Registers[Rn] + Registers[Rm];
            Bus.WriteUInt8(Address, (byte)Registers[Rd]);
        }

        /// <summary>
        ///     Store Register Halfword (1).
        /// </summary>
        private void Thumb_STRH()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            uint Immediate = (Opcode >> 6) & 0x1f;

            uint Address = Registers[Rn] + (Immediate << 1);
            WriteUInt16E(Address, (ushort)Registers[Rd]);
        }

        /// <summary>
        ///     Store Register Halfword (2).
        /// </summary>
        private void Thumb_STRH_2()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            int Rm = (int)((Opcode >> 6) & 7);

            uint Address = Registers[Rn] + Registers[Rm];
            WriteUInt16E(Address, (ushort)Registers[Rd]);
        }
    }
}
