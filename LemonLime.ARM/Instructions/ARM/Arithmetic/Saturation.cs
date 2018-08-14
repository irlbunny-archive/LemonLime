namespace LemonLime.ARM
{
    public partial class Interpreter
    {
        /*
         * Instructions
         */

        //Signed

        /// <summary>
        ///     Signed Saturate.
        /// </summary>
        private void ARM_SSAT()
        {
            int Rm = (int)(Opcode & 0xf);
            bool Shift = IsOpcodeBitSet(6);
            int ShiftImmediate = (int)((Opcode >> 7) & 0x1f);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int SaturateImmediate = (int)(((Opcode >> 16) & 0x1f) + 1);

            int RmValue = (int)Registers[Rm];
            if (Shift)
                RmValue >>= ShiftImmediate == 0 ? 32 : ShiftImmediate;
            else
                RmValue <<= ShiftImmediate;

            Registers[Rd] = SignedSaturate(RmValue, SaturateImmediate);
        }

        /// <summary>
        ///     Signed Saturate 16-bits.
        /// </summary>
        private void ARM_SSAT16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int SaturateImmediate = (int)(((Opcode >> 16) & 0xf) + 1);

            Registers[Rd] = SignedSaturate((int)(Registers[Rm] & 0xffff), SaturateImmediate);
            Registers[Rd] |= SignedSaturate((int)(Registers[Rm] >> 16), SaturateImmediate) << 16;
        }

        //Unsigned

        /// <summary>
        ///     Unsigned Saturate.
        /// </summary>
        private void ARM_USAT()
        {
            int Rm = (int)(Opcode & 0xf);
            bool Shift = IsOpcodeBitSet(6);
            int ShiftImmediate = (int)((Opcode >> 7) & 0x1f);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int SaturateImmediate = (int)(((Opcode >> 16) & 0x1f) + 1);

            int RmValue = (int)Registers[Rm];
            if (Shift)
                RmValue >>= ShiftImmediate == 0 ? 32 : ShiftImmediate;
            else
                RmValue <<= ShiftImmediate;

            Registers[Rd] = UnsignedSaturate(RmValue, SaturateImmediate);
        }

        /// <summary>
        ///     Unsigned Saturate 16-bits.
        /// </summary>
        private void ARM_USAT16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int SaturateImmediate = (int)(((Opcode >> 16) & 0xf) + 1);

            Registers[Rd] = UnsignedSaturate((int)(Registers[Rm] & 0xffff), SaturateImmediate);
            Registers[Rd] |= UnsignedSaturate((int)(Registers[Rm] >> 16), SaturateImmediate) << 16;
        }

        /*
         * Utils
         */

        /// <summary>
        ///     Saturates a signed 32-bits Value into a given range of Bits.
        /// </summary>
        /// <param name="Value">The value that should be saturated</param>
        /// <param name="Bits">The bit-range to saturate the value into</param>
        /// <returns>The saturated value</returns>
        private uint SignedSaturate(int Value, int Bits)
        {
            uint Mask = (uint)((1 << Bits) - 1);
            int High = Value >> Bits;
            if (High > 0 || High < -1)
            {
                Registers.SetFlag(ARMFlag.Saturation, true);
                return High > 0 ? Mask : ~Mask;
            }
            else
                return (uint)Value;
        }

        /// <summary>
        ///     Saturates a unsigned 32-bits Value into a given range of Bits.
        /// </summary>
        /// <param name="Value">The value that should be saturated</param>
        /// <param name="Bits">The bit-range to saturate the value into</param>
        /// <returns>The saturated value</returns>
        private uint UnsignedSaturate(int Value, int Bits)
        {
            uint Mask = (uint)((1 << Bits) - 1);
            if (Value > Mask || (Value & 0x80000000) != 0)
            {
                Registers.SetFlag(ARMFlag.Saturation, true);
                return Value > Mask ? Mask : 0;
            }
            else
                return (uint)Value;
        }
    }
}
