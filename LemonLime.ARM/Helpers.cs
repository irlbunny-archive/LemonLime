namespace LemonLime.ARM
{
    public partial class Interpreter
    {
        /// <summary>
        ///     Checks whenever a bit on the 32-bits Opcode value is set or not.
        /// </summary>
        /// <param name="Bit">The bit that should be tested</param>
        /// <returns>True if the bit is set, false otherwise</returns>
        private bool IsOpcodeBitSet(int Bit)
        {
            return (Opcode & (1 << Bit)) != 0;
        }

        /// <summary>
        ///     Shift the bits to the right, and make the exceeding bits fill the other way around.
        ///     Example: For a 8 bits value, ROR(0b00000011, 1) = 0b10000001.
        /// </summary>
        /// <param name="Value">Value that should be shifted</param>
        /// <param name="Shift">Number of bits to shift</param>
        /// <returns>The shifted value</returns>
        private uint ROR(uint Value, int Shift)
        {
            return (Value >> Shift) | (Value << (32 - Shift));
        }

        /// <summary>
        ///     Sets the Zero flag if the value is zero, and clear it otherwise.
        ///     Sets the Negative flag if the value is negative (most heavy bit set), and clear it otherwise.
        /// </summary>
        /// <param name="Value">The resulting 32-bits value that should be tested</param>
        private void SetZNFlags(uint Value)
        {
            Registers.SetFlag(ARMFlag.Zero, Value == 0);
            Registers.SetFlag(ARMFlag.Negative, (Value & 0x80000000) != 0);
        }

        /// <summary>
        ///     Sign extends a 11-bits value to a signed 32-bits value.
        /// </summary>
        /// <param name="Value">The 11-bits value that should be extended</param>
        /// <returns>The value extended into 32-bits</returns>
        private int SignExtend11(uint Value)
        {
            if ((Value & 0x400) != 0)
                return (int)(Value | 0xfffff800);
            else
                return (int)Value;
        }

        /// <summary>
        ///     Sign extends a 12-bits value to a signed 32-bits value.
        /// </summary>
        /// <param name="Value">The 12-bits value that should be extended</param>
        /// <returns>The value extended into 32-bits</returns>
        private int SignExtend12(uint Value)
        {
            if ((Value & 0x800) != 0)
                return (int)(Value | 0xfffff000);
            else
                return (int)Value;
        }

        /// <summary>
        ///     Sign extends a 24-bits value to a signed 32-bits value.
        /// </summary>
        /// <param name="Value">The 24-bits value that should be extended</param>
        /// <returns>The value extended into 32-bits</returns>
        private int SignExtend24(uint Value)
        {
            if ((Value & 0x800000) != 0)
                return (int)(Value | 0xff000000);
            else
                return (int)Value;
        }
    }
}
