namespace CTREmulator.ARM
{
    public partial class Interpreter
    {
        /// <summary>
        ///     Copy.
        /// </summary>
        private void Thumb_CPY()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);
            bool H2 = IsOpcodeBitSet(6);
            bool H1 = IsOpcodeBitSet(7);

            if (H1) Rd += 8;
            if (H2) Rm += 8;
            Registers[Rd] = Registers[Rm];
            if (Rd == 15) Registers[Rd] &= 0xfffffffe;
        }

        /// <summary>
        ///     Move (1).
        /// </summary>
        private void Thumb_MOV()
        {
            uint Immediate = Opcode & 0xff;
            int Rd = (int)((Opcode >> 8) & 7);

            Registers[Rd] = Immediate;
            SetZNFlags(Registers[Rd]);
        }

        /// <summary>
        ///     Move (2).
        /// </summary>
        private void Thumb_MOV_2()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);

            Registers[Rd] = Registers[Rn];
            SetZNFlags(Registers[Rd]);
        }

        /// <summary>
        ///     Move Not.
        /// </summary>
        private void Thumb_MVN()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            Registers[Rd] = ~Registers[Rm];
            SetZNFlags(Registers[Rd]);
        }

        /// <summary>
        ///     Pop.
        /// </summary>
        private void Thumb_POP()
        {
            for (int Index = 0; Index < 8; Index++)
            {
                if ((Opcode & (1 << Index)) != 0)
                {
                    Registers[Index] = ReadUInt32E(Registers[13]);
                    Registers[13] += 4;
                }
            }

            if (IsOpcodeBitSet(8))
            {
                uint Value = ReadUInt32E(Registers[13]);
                Registers[15] = Value & 0xfffffffe;
                Registers.SetFlag(ARMFlag.Thumb, (Value & 1) != 0);
                Registers[13] += 4;
            }
        }

        /// <summary>
        ///     Push.
        /// </summary>
        private void Thumb_PUSH()
        {
            if (IsOpcodeBitSet(8))
            {
                Registers[13] -= 4;
                WriteUInt32E(Registers[13], Registers[14]);
            }

            for (int Index = 7; Index >= 0; Index--)
            {
                if ((Opcode & (1 << Index)) != 0)
                {
                    Registers[13] -= 4;
                    WriteUInt32E(Registers[13], Registers[Index]);
                }
            }
        }

        /// <summary>
        ///     Signed Extend Byte.
        /// </summary>
        private void Thumb_SXTB()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);
            
            sbyte RmValue = (sbyte)Registers[Rm];
            Registers[Rd] = (uint)RmValue;
        }

        /// <summary>
        ///     Signed Extend Halfword.
        /// </summary>
        private void Thumb_SXTH()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            short RmValue = (short)Registers[Rm];
            Registers[Rd] = (uint)RmValue;
        }

        /// <summary>
        ///     Unsigned Extend Byte.
        /// </summary>
        private void Thumb_UXTB()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            Registers[Rd] = Registers[Rm] & 0xff;
        }

        /// <summary>
        ///     Unsigned Extend Halfword.
        /// </summary>
        private void Thumb_UXTH()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            Registers[Rd] = Registers[Rm] & 0xffff;
        }
    }
}
