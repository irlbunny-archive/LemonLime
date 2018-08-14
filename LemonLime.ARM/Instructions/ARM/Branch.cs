namespace LemonLime.ARM
{
    public partial class Interpreter
    {
        /// <summary>
        ///     Branch.
        /// </summary>
        private void ARM_B()
        {
            int Value = SignExtend24(Opcode & 0xffffff);
            bool L = IsOpcodeBitSet(24);

            if (L) Registers[14] = Registers[15] - 4;
            Registers[15] = (uint)(Registers[15] + (Value << 2));
        }

        /// <summary>
        ///     Branch with Link and Exchange (1).
        /// </summary>
        private void ARM_BLX()
        {
            int Value = SignExtend24(Opcode & 0xffffff);
            uint H = (Opcode >> 24) & 1;

            Registers[15] = (uint)(Registers[15] + (Value << 2) + (H << 1));
        }

        /// <summary>
        ///     Branch with Link and Exchange (2).
        /// </summary>
        private void ARM_BLX_2()
        {
            int Rm = (int)(Opcode & 0xf);

            Registers[14] = Registers[15] - 4;
            Registers[15] = Registers[Rm] & 0xfffffffe;
            Registers.SetFlag(ARMFlag.Thumb, (Registers[Rm] & 1) != 0);
        }

        /// <summary>
        ///     Branch and Exchange.
        /// </summary>
        private void ARM_BX()
        {
            int Rm = (int)(Opcode & 0xf);

            Registers[15] = Registers[Rm] & 0xfffffffe;
            Registers.SetFlag(ARMFlag.Thumb, (Registers[Rm] & 1) != 0);
        }

        /// <summary>
        ///     Branch and Exchange to Jazelle state.
        /// </summary>
        private void ARM_BXJ()
        {
            //Note: This processor doesn't support Jazelle, act as BX.
            ARM_BX();
        }
    }
}
