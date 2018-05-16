namespace CTREmulator.ARM
{
    public partial class ARMInterpreter
    {
        /// <summary>
        ///     And.
        /// </summary>
        private void ARM_AND()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);
            uint ShifterOperand = ARM_GetShifterOperand(S && Rd != 15);

            Registers[Rd] = Registers[Rn] & ShifterOperand;
            
            if (S)
            {
                if (Rd != 15)
                    SetZNFlags(Registers[Rd]);
                else
                    Registers.CPSR = Registers.SPSR;
            }
        }

        /// <summary>
        ///     Bit Clear.
        /// </summary>
        private void ARM_BIC()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);
            uint ShifterOperand = ARM_GetShifterOperand(S && Rd != 15);

            Registers[Rd] = Registers[Rn] & ~ShifterOperand;

            if (S)
            {
                if (Rd != 15)
                    SetZNFlags(Registers[Rd]);
                else
                    Registers.CPSR = Registers.SPSR;
            }
        }

        /// <summary>
        ///     Count Leading Zeros.
        /// </summary>
        private void ARM_CLZ()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);

            if (Registers[Rm] == 0)
                Registers[Rd] = 32;
            else
            {
                for (int Count = 31; Count >= 0; Count--)
                {
                    if ((Registers[Rm] & (1 << Count)) != 0)
                    {
                        Registers[Rd] = (uint)(31 - Count);
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     Exclusive Or.
        /// </summary>
        private void ARM_EOR()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);
            uint ShifterOperand = ARM_GetShifterOperand(S && Rd != 15);

            Registers[Rd] = Registers[Rn] ^ ShifterOperand;

            if (S)
            {
                if (Rd != 15)
                    SetZNFlags(Registers[Rd]);
                else
                    Registers.CPSR = Registers.SPSR;
            }
        }

        /// <summary>
        ///     Or.
        /// </summary>
        private void ARM_ORR()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);
            uint ShifterOperand = ARM_GetShifterOperand(S && Rd != 15);

            Registers[Rd] = Registers[Rn] | ShifterOperand;

            if (S)
            {
                if (Rd != 15)
                    SetZNFlags(Registers[Rd]);
                else
                    Registers.CPSR = Registers.SPSR;
            }
        }

        /// <summary>
        ///     Pack Halfword Bottom Top.
        /// </summary>
        private void ARM_PKHBT()
        {
            int Rm = (int)(Opcode & 0xf);
            int ShiftImmediate = (int)((Opcode >> 7) & 0x1f);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            Registers[Rd] = (Registers[Rn] & 0xffff);
            Registers[Rd] |= ((Registers[Rm] << ShiftImmediate) & 0xffff0000);
        }

        /// <summary>
        ///     Pack Halfword Top Bottom.
        /// </summary>
        private void ARM_PKHTB()
        {
            int Rm = (int)(Opcode & 0xf);
            int ShiftImmediate = (int)((Opcode >> 7) & 0x1f);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            Registers[Rd] = (Registers[Rn] & 0xffff0000);

            if (ShiftImmediate == 0)
                Registers[Rd] |= (uint)((Registers[Rm] & 0x80000000) != 0 ? 0xffff : 0);
            else
                Registers[Rd] |= ((Registers[Rm] >> ShiftImmediate) & 0xffff);
        }

        /// <summary>
        ///     Byte-Reverse Word.
        /// </summary>
        private void ARM_REV()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);

            Registers[Rd] = (Registers[Rm] >> 24) |
                ((Registers[Rm] >> 8) & 0xff00) |
                ((Registers[Rm] & 0xff00) << 8) |
                ((Registers[Rm] & 0xff) << 24);
        }

        /// <summary>
        ///     Byte-Reverse Packed Halfword.
        /// </summary>
        private void ARM_REV16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);

            Registers[Rd] = ((Registers[Rm] >> 8) & 0xff0000) |
                ((Registers[Rm] & 0xff0000) << 8) |
                ((Registers[Rm] >> 8) & 0xff) |
                ((Registers[Rm] & 0xff) << 8);
        }

        /// <summary>
        ///     Byte-Reverse Signed Halfword.
        /// </summary>
        private void ARM_REVSH()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);

            Registers[Rd] = ((Registers[Rm] >> 8) & 0xff) | ((Registers[Rm] & 0xff) << 8);
            if ((Registers[Rm] & 0x80) != 0) Registers[Rd] |= 0xffff0000;
        }

        /// <summary>
        ///     Select.
        /// </summary>
        private void ARM_SEL()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            Registers[Rd] = (Registers.GE & 1) != 0 ? Registers[Rn] & 0xff : Registers[Rm] & 0xff;
            Registers[Rd] |= (Registers.GE & 2) != 0 ? Registers[Rn] & 0xff00 : Registers[Rm] & 0xff00;
            Registers[Rd] |= (Registers.GE & 4) != 0 ? Registers[Rn] & 0xff0000 : Registers[Rm] & 0xff0000;
            Registers[Rd] |= (Registers.GE & 8) != 0 ? Registers[Rn] & 0xff000000 : Registers[Rm] & 0xff000000;
        }
    }
}
