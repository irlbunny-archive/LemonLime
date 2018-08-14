namespace CTREmulator.ARM
{
    public partial class Interpreter
    {
        /// <summary>
        ///     And.
        /// </summary>
        private void Thumb_AND()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            Registers[Rd] &= Registers[Rm];
            SetZNFlags(Registers[Rd]);
        }

        /// <summary>
        ///     Bit Clear.
        /// </summary>
        private void Thumb_BIC()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            Registers[Rd] &= ~Registers[Rm];
            SetZNFlags(Registers[Rd]);
        }

        /// <summary>
        ///     Exclusive Or.
        /// </summary>
        private void Thumb_EOR()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            Registers[Rd] ^= Registers[Rm];
            SetZNFlags(Registers[Rd]);
        }

        /// <summary>
        ///     Logical Shift Left (1).
        /// </summary>
        private void Thumb_LSL()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);
            int Shift = (int)((Opcode >> 6) & 0x1f);

            if (Shift > 0) Registers.SetFlag(ARMFlag.Carry, (Registers[Rm] & (1 << (32 - Shift))) != 0);
            Registers[Rd] = Registers[Rm] << Shift;

            SetZNFlags(Registers[Rd]);
        }

        /// <summary>
        ///     Logical Shift Left (2).
        /// </summary>
        private void Thumb_LSL_2()
        {
            int Rd = (int)(Opcode & 7);
            int Rs = (int)((Opcode >> 3) & 7);

            int Shift = (int)(Registers[Rs] & 0xff);
            if (Shift > 0)
            {
                bool Carry = false;
                if (Shift <= 32)
                {
                    Carry = (Registers[Rd] & (1 << (32 - Shift))) != 0;
                    Registers[Rd] = (uint)((ulong)Registers[Rd] << Shift);
                }
                else
                    Registers[Rd] = 0;

                Registers.SetFlag(ARMFlag.Carry, Carry);
            }

            SetZNFlags(Registers[Rd]);
        }

        /// <summary>
        ///     Logical Shift Right (1).
        /// </summary>
        private void Thumb_LSR()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);
            int Shift = (int)((Opcode >> 6) & 0x1f);

            if (Shift == 0) Shift = 32;
            Registers.SetFlag(ARMFlag.Carry, (Registers[Rm] & (1 << (Shift - 1))) != 0);
            Registers[Rd] = (uint)((ulong)Registers[Rm] >> Shift);

            SetZNFlags(Registers[Rd]);
        }

        /// <summary>
        ///     Logical Shift Right (2).
        /// </summary>
        private void Thumb_LSR_2()
        {
            int Rd = (int)(Opcode & 7);
            int Rs = (int)((Opcode >> 3) & 7);

            int Shift = (int)(Registers[Rs] & 0xff);
            if (Shift > 0)
            {
                bool Carry = false;
                if (Shift <= 32)
                {
                    Carry = (Registers[Rd] & (1 << (Shift - 1))) != 0;
                    Registers[Rd] = (uint)((ulong)Registers[Rd] >> Shift);
                }
                else
                    Registers[Rd] = 0;

                Registers.SetFlag(ARMFlag.Carry, Carry);
            }

            SetZNFlags(Registers[Rd]);
        }

        /// <summary>
        ///     Or.
        /// </summary>
        private void Thumb_ORR()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            Registers[Rd] |= Registers[Rm];
            SetZNFlags(Registers[Rd]);
        }

        /// <summary>
        ///     Byte-Reverse Word.
        /// </summary>
        private void Thumb_REV()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);

            Registers[Rd] = (Registers[Rn] >> 24) |
                ((Registers[Rn] >> 8) & 0xff00) |
                ((Registers[Rn] & 0xff00) << 8) |
                ((Registers[Rn] & 0xff) << 24);
        }

        /// <summary>
        ///     Byte-Reverse Packed Halfword.
        /// </summary>
        private void Thumb_REV16()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);

            Registers[Rd] = ((Registers[Rn] >> 8) & 0xff0000) |
                ((Registers[Rn] & 0xff0000) << 8) |
                ((Registers[Rn] >> 8) & 0xff) |
                ((Registers[Rn] & 0xff) << 8);
        }

        /// <summary>
        ///     Byte-Reverse Signed Halfword.
        /// </summary>
        private void Thumb_REVSH()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);

            Registers[Rd] = ((Registers[Rn] >> 8) & 0xff) | ((Registers[Rn] & 0xff) << 8);
            if ((Registers[Rn] & 0x80) != 0) Registers[Rd] |= 0xffff0000;
        }

        /// <summary>
        ///     Rotate Right.
        /// </summary>
        private void Thumb_ROR()
        {
            int Rd = (int)(Opcode & 7);
            int Rs = (int)((Opcode >> 3) & 7);

            int Shift = (int)(Registers[Rs] & 0xff);
            if (Shift > 0)
            {
                Shift &= 0x1f;
                if (Shift > 0)
                {
                    Registers.SetFlag(ARMFlag.Carry, (Registers[Rd] & (1 << (Shift - 1))) != 0);
                    Registers[Rd] = ROR(Registers[Rd], Shift);
                }
                else
                    Registers.SetFlag(ARMFlag.Carry, (Registers[Rd] & 0x80000000) != 0);               
            }

            SetZNFlags(Registers[Rd]);
        }
    }
}
