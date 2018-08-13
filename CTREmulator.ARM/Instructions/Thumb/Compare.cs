namespace CTREmulator.ARM
{
    public partial class Interpreter
    {
        /// <summary>
        ///     Compare Negative.
        /// </summary>
        private void Thumb_CMN()
        {
            int Rn = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            Add(Registers[Rn], Registers[Rm], false, true);
        }

        /// <summary>
        ///     Compare (1).
        /// </summary>
        private void Thumb_CMP()
        {
            uint Immediate = Opcode & 0xff;
            int Rn = (int)((Opcode >> 8) & 7);

            Subtract(Registers[Rn], Immediate, false, true);
        }

        /// <summary>
        ///     Compare (2).
        /// </summary>
        private void Thumb_CMP_2()
        {
            int Rn = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            Subtract(Registers[Rn], Registers[Rm], false, true);
        }

        /// <summary>
        ///     Compare (3).
        /// </summary>
        private void Thumb_CMP_3()
        {
            int Rn = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            if (IsOpcodeBitSet(7)) Rn += 8;
            if (IsOpcodeBitSet(6)) Rm += 8;
            Subtract(Registers[Rn], Registers[Rm], false, true);
        }

        /// <summary>
        ///     Test.
        /// </summary>
        private void Thumb_TST()
        {
            int Rn = (int)(Opcode & 7);
            int Rm = (int)((Opcode >> 3) & 7);

            SetZNFlags(Registers[Rn] & Registers[Rm]);
        }
    }
}
