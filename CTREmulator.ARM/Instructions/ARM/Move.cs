namespace CTREmulator.ARM
{
    public partial class Interpreter
    {
        const uint UserMask = 0xf80f0200;
        const uint PrivilegedMask = 0x1df;
        const uint StateMask = 0x1000020;

        /// <summary>
        ///     Move.
        /// </summary>
        private void ARM_MOV()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);
            bool S = IsOpcodeBitSet(20);
            uint ShifterOperand = ARM_GetShifterOperand(S && Rd != 15);

            Registers[Rd] = ShifterOperand;
            if (S)
            {
                if (Rd != 15)
                    SetZNFlags(Registers[Rd]);
                else
                    Registers.CPSR = Registers.SPSR;
            }
        }

        /// <summary>
        ///     Move General-Purpose Register to Status Register.
        /// </summary>
        private void ARM_MRS()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);
            bool R = IsOpcodeBitSet(22);

            Registers[Rd] = R ? Registers.SPSR : Registers.CPSR;
        }

        /// <summary>
        ///     Move Status Register to General-Purpose Register.
        /// </summary>
        private void ARM_MSR()
        {
            uint Mask = (Opcode >> 16) & 0xf;
            bool R = IsOpcodeBitSet(22);
            bool I = IsOpcodeBitSet(25);

            uint ByteMask = 0;
            if ((Mask & 1) != 0) ByteMask |= 0xff;
            if ((Mask & 2) != 0) ByteMask |= 0xff00;
            if ((Mask & 4) != 0) ByteMask |= 0xff0000;
            if ((Mask & 8) != 0) ByteMask |= 0xff000000;

            uint Value;
            if (I)
            {
                uint Immediate = Opcode & 0xff;
                int Rotate = (int)((Opcode >> 7) & 0x1e);
                Value = ROR(Immediate, Rotate);
            }
            else
            {
                int Rm = (int)(Opcode & 0xf);
                Value = Registers[Rm];
            }

            if (R)
            {
                Mask = ByteMask & (UserMask | PrivilegedMask | StateMask);
                Registers.SPSR = (Registers.SPSR & ~Mask) | (Value & Mask);
            }
            else
            {
                Mask = ByteMask & (Registers.Mode == ARMMode.User ? UserMask : UserMask | PrivilegedMask);
                Registers.CPSR = (Registers.CPSR & ~Mask) | (Value & Mask);
            }
        }

        /// <summary>
        ///     Move Not.
        /// </summary>
        private void ARM_MVN()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);
            bool S = IsOpcodeBitSet(20);
            uint ShifterOperand = ARM_GetShifterOperand(S && Rd != 15);

            Registers[Rd] = ~ShifterOperand;
            if (S)
            {
                if (Rd != 15)
                    SetZNFlags(Registers[Rd]);
                else
                    Registers.CPSR = Registers.SPSR;
            }
        }

        /// <summary>
        ///     Swap.
        /// </summary>
        private void ARM_SWP()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            uint Value = ReadUInt32E(Registers[Rn]);
            WriteUInt32E(Registers[Rn], Registers[Rm]);
            Registers[Rd] = Value;
        }

        /// <summary>
        ///     Swap Byte.
        /// </summary>
        private void ARM_SWPB()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            byte Value = Bus.ReadUInt8(Registers[Rn]);
            Bus.WriteUInt8(Registers[Rn], (byte)Registers[Rm]);
            Registers[Rd] = Value;
        }

        /// <summary>
        ///     Signed Extend Accumulate Byte.
        /// </summary>
        private void ARM_SXTAB()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rotate = (int)((Opcode >> 10) & 3);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            uint Operand = ROR(Registers[Rm], Rotate << 3);
            Registers[Rd] = (uint)(Registers[Rn] + (sbyte)Operand);
        }

        /// <summary>
        ///     Signed Extend Accumulate Byte 16-bits.
        /// </summary>
        private void ARM_SXTAB16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rotate = (int)((Opcode >> 10) & 3);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            uint Operand = ROR(Registers[Rm], Rotate << 3);
            Registers[Rd] = (ushort)(Registers[Rn] + (sbyte)Operand);
            Registers[Rd] |= (uint)((Registers[Rn] + (sbyte)(Operand >> 16)) << 16);
        }

        /// <summary>
        ///     Signed Extend Accumulate Halfword.
        /// </summary>
        private void ARM_SXTAH()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rotate = (int)((Opcode >> 10) & 3);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            uint Operand = ROR(Registers[Rm], Rotate << 3);
            Registers[Rd] = (uint)(Registers[Rn] + (short)Operand);
        }

        /// <summary>
        ///     Signed Extend Byte.
        /// </summary>
        private void ARM_SXTB()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rotate = (int)((Opcode >> 10) & 3);
            int Rd = (int)((Opcode >> 12) & 0xf);

            sbyte Operand = (sbyte)ROR(Registers[Rm], Rotate << 3);
            Registers[Rd] = (uint)Operand;
        }

        /// <summary>
        ///     Signed Extend Byte 16-bits.
        /// </summary>
        private void ARM_SXTB16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rotate = (int)((Opcode >> 10) & 3);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            uint Operand = ROR(Registers[Rm], Rotate << 3);
            Registers[Rd] = (ushort)((short)Operand);
            Registers[Rd] |= (uint)((short)(Operand >> 16) << 16);
        }

        /// <summary>
        ///     Signed Extend Halfword.
        /// </summary>
        private void ARM_SXTH()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rotate = (int)((Opcode >> 10) & 3);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            short Operand = (short)ROR(Registers[Rm], Rotate << 3);
            Registers[Rd] = (uint)Operand;
        }

        /// <summary>
        ///     Unsigned Extend Accumulate Byte.
        /// </summary>
        private void ARM_UXTAB()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rotate = (int)((Opcode >> 10) & 3);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            uint Operand = ROR(Registers[Rm], Rotate << 3);
            Registers[Rd] = Registers[Rn] + (byte)Operand;
        }

        /// <summary>
        ///     Unsigned Extend Accumulate Byte 16-bits.
        /// </summary>
        private void ARM_UXTAB16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rotate = (int)((Opcode >> 10) & 3);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            uint Operand = ROR(Registers[Rm], Rotate << 3);
            Registers[Rd] = (ushort)(Registers[Rn] + (byte)Operand);
            Registers[Rd] |= (Registers[Rn] + (byte)(Operand >> 16)) << 16;
        }

        /// <summary>
        ///     Unsigned Extend Accumulate Halfword.
        /// </summary>
        private void ARM_UXTAH()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rotate = (int)((Opcode >> 10) & 3);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            uint Operand = ROR(Registers[Rm], Rotate << 3);
            Registers[Rd] = Registers[Rn] + (ushort)Operand;
        }

        /// <summary>
        ///     Unsigned Extend Byte.
        /// </summary>
        private void ARM_UXTB()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rotate = (int)((Opcode >> 10) & 3);
            int Rd = (int)((Opcode >> 12) & 0xf);

            Registers[Rd] = ROR(Registers[Rm], Rotate << 3) & 0xff;
        }

        /// <summary>
        ///     Unsigned Extend Byte 16-bits.
        /// </summary>
        private void ARM_UXTB16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rotate = (int)((Opcode >> 10) & 3);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            Registers[Rd] = ROR(Registers[Rm], Rotate << 3) & 0xff00ff;
        }

        /// <summary>
        ///     Unsigned Extend Halfword.
        /// </summary>
        private void ARM_UXTH()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rotate = (int)((Opcode >> 10) & 3);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            Registers[Rd] = ROR(Registers[Rm], Rotate << 3) & 0xffff;
        }
    }
}
