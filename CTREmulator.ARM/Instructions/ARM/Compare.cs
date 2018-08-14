namespace CTREmulator.ARM
{
    public partial class Interpreter
    {
        /// <summary>
        ///     Compare Negative.
        /// </summary>
        private void ARM_CMN()
        {
            int Rn = (int)((Opcode >> 16) & 0xf);
            uint ShifterOperand = ARM_GetShifterOperand(true);

            Add(Registers[Rn], ShifterOperand, false, true);
        }
        
        /// <summary>
        ///     Compare.
        /// </summary>
        private void ARM_CMP()
        {
            int Rn = (int)((Opcode >> 16) & 0xf);
            uint ShifterOperand = ARM_GetShifterOperand(true);

            Subtract(Registers[Rn], ShifterOperand, false, true);
        }

        /// <summary>
        ///     Test Equivalence.
        /// </summary>
        private void ARM_TEQ()
        {
            int Rn = (int)((Opcode >> 16) & 0xf);
            uint ShifterOperand = ARM_GetShifterOperand(true);

            SetZNFlags(Registers[Rn] ^ ShifterOperand);
        }

        /// <summary>
        ///     Test.
        /// </summary>
        private void ARM_TST()
        {
            int Rn = (int)((Opcode >> 16) & 0xf);
            uint ShifterOperand = ARM_GetShifterOperand(true);

            SetZNFlags(Registers[Rn] & ShifterOperand);
        }
    }
}
