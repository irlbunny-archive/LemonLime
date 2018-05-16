namespace CTREmulator.ARM
{
    public partial class ARMInterpreter
    {
        //Addition

        /// <summary>
        ///     Add with Carry.
        /// </summary>
        private void Thumb_ADC()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            Registers[Rd] = Add(Registers[Rd], Registers[Rm], true, true);
        }

        /// <summary>
        ///     Add (1).
        /// </summary>
        private void Thumb_ADD()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            uint Immediate = (Opcode >> 6) & 7;

            Registers[Rd] = Add(Registers[Rn], Immediate, false, true);
        }

        /// <summary>
        ///     Add (2).
        /// </summary>
        private void Thumb_ADD_2()
        {
            uint Immediate = Opcode & 0xff;
            int Rd = (int)((Opcode >> 8) & 7);
            
            Registers[Rd] = Add(Registers[Rd], Immediate, false, true);
        }

        /// <summary>
        ///     Add (3).
        /// </summary>
        private void Thumb_ADD_3()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            int Rm = (int)((Opcode >> 6) & 7);

            Registers[Rd] = Add(Registers[Rn], Registers[Rm], false, true);
        }

        /// <summary>
        ///     Add (4).
        /// </summary>
        private void Thumb_ADD_4()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            if (IsOpcodeBitSet(7)) Rd += 8;
            if (IsOpcodeBitSet(6)) Rm += 8;
            Registers[Rd] = Add(Registers[Rd], Registers[Rm], false, false);
        }

        /// <summary>
        ///     Add (5).
        /// </summary>
        private void Thumb_ADD_5()
        {
            uint Immediate = Opcode & 0xff;
            int Rd = (int)((Opcode >> 8) & 7);

            Registers[Rd] = Add(Registers[15] & 0xfffffffc, Immediate << 2, false, false);
        }

        /// <summary>
        ///     Add (6).
        /// </summary>
        private void Thumb_ADD_6()
        {
            uint Immediate = Opcode & 0xff;
            int Rd = (int)((Opcode >> 8) & 7);

            Registers[Rd] = Add(Registers[13], Immediate << 2, false, false);
        }

        /// <summary>
        ///     Add (7).
        /// </summary>
        private void Thumb_ADD_7()
        {
            uint Immediate = Opcode & 0x7f;

            Registers[13] = Add(Registers[13], Immediate << 2, false, false);
        }

        //Miscellaneous

        /// <summary>
        ///     Arithmetic Shift Right (1).
        /// </summary>
        private void Thumb_ASR()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);
            int Shift = (int)((Opcode >> 6) & 0x1f);

            if (Shift == 0) Shift = 32;
            int RmValue = (int)Registers[Rm];
            Registers[Rd] = (uint)((long)RmValue >> Shift);
            Registers.SetFlag(ARMFlag.Carry, (RmValue & (1 << (Shift - 1))) != 0);

            SetZNFlags(Registers[Rd]);
        }

        /// <summary>
        ///     Arithmetic Shift Right (2).
        /// </summary>
        private void Thumb_ASR_2()
        {
            int Rd = (int)(Opcode & 7);
            int Rs = (int)((Opcode >> 3) & 7);

            int Shift = (int)(Registers[Rs] & 0xff);
            if (Shift > 0)
            {
                bool Carry = false;
                if (Shift <= 32)
                {
                    int RdValue = (int)Registers[Rd];
                    Registers[Rd] = (uint)((long)RdValue >> Shift);
                    Carry = (RdValue & (1 << (Shift - 1))) != 0;
                }
                else
                {
                    Carry = (Registers[Rd] & 0x80000000) != 0;
                    Registers[Rd] = Carry ? 0xffffffff : 0;
                }

                Registers.SetFlag(ARMFlag.Carry, Carry);
            }

            SetZNFlags(Registers[Rd]);
        }

        /// <summary>
        ///     Multiply.
        /// </summary>
        private void Thumb_MUL()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            Registers[Rd] *= Registers[Rm];
            SetZNFlags(Registers[Rd]);
        }

        //Subtraction

        /// <summary>
        ///     Negate.
        /// </summary>
        private void Thumb_NEG()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            Registers[Rd] = Subtract(0, Registers[Rm], false, true);
        }

        /// <summary>
        ///     Subtract with Carry.
        /// </summary>
        private void Thumb_SBC()
        {
            int Rd = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            Registers[Rd] = Subtract(Registers[Rd], Registers[Rm], true, true);
        }

        /// <summary>
        ///     Subtract (1).
        /// </summary>
        private void Thumb_SUB()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            uint Immediate = (Opcode >> 6) & 7;

            Registers[Rd] = Subtract(Registers[Rn], Immediate, false, true);
        }

        /// <summary>
        ///     Subtract (2).
        /// </summary>
        private void Thumb_SUB_2()
        {
            uint Immediate = Opcode & 0xff;
            int Rd = (int)((Opcode >> 8) & 7);

            Registers[Rd] = Subtract(Registers[Rd], Immediate, false, true);
        }

        /// <summary>
        ///     Subtract (3).
        /// </summary>
        private void Thumb_SUB_3()
        {
            int Rd = (int)(Opcode & 7);
            int Rn = (int)((Opcode >> 3) & 7);
            int Rm = (int)((Opcode >> 6) & 7);

            Registers[Rd] = Subtract(Registers[Rn], Registers[Rm], false, true);
        }

        /// <summary>
        ///     Subtract (4).
        /// </summary>
        private void Thumb_SUB_4()
        {
            uint Immediate = Opcode & 0x7f;

            Registers[13] = Subtract(Registers[13], Immediate << 2, false, false);
        }
    }
}
