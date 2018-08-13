namespace CTREmulator.ARM
{
    public partial class Interpreter
    {
        /*
         * Instructions
         */

        //Multiply

        /// <summary>
        ///     Multiply Accumulate.
        /// </summary>
        private void ARM_MLA()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int Rn = (int)((Opcode >> 12) & 0xf);
            int Rd = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);

            Registers[Rd] = Registers[Rm] * Registers[Rs] + Registers[Rn];
            if (S) SetZNFlags(Registers[Rd]);
        }

        /// <summary>
        ///     Multiply.
        /// </summary>
        private void ARM_MUL()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int Rd = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);

            Registers[Rd] = Registers[Rm] * Registers[Rs];
            if (S) SetZNFlags(Registers[Rd]);
        }

        //Signed Multiply

        /// <summary>
        ///     Signed Multiply Accumulate.
        /// </summary>
        private void ARM_SMLA()
        {
            int Rm = (int)(Opcode & 0xf);
            bool x = IsOpcodeBitSet(5);
            bool y = IsOpcodeBitSet(6);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int Rn = (int)((Opcode >> 12) & 0xf);
            int Rd = (int)((Opcode >> 16) & 0xf);

            int RmValue = (short)(x ? Registers[Rm] >> 16 : Registers[Rm]);
            int RsValue = (short)(y ? Registers[Rs] >> 16 : Registers[Rs]);
            uint Result = (uint)(RmValue * RsValue);
            Registers[Rd] = AddSignedSetQ(Result, Registers[Rn]);
        }

        /// <summary>
        ///     Signed Multiply Accumulate Dual.
        /// </summary>
        private void ARM_SMLAD()
        {
            int Rm = (int)(Opcode & 0xf);
            bool X = IsOpcodeBitSet(5);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int Rn = (int)((Opcode >> 12) & 0xf);
            int Rd = (int)((Opcode >> 16) & 0xf);

            short RmLow = (short)Registers[Rm];
            short RmHigh = (short)(Registers[Rm] >> 16);

            uint RsValue = X ? ROR(Registers[Rs], 16) : Registers[Rs];
            short RsLow = (short)RsValue;
            short RsHigh = (short)(RsValue >> 16);

            int ResultLow = RmLow * RsLow;
            int ResultHigh = RmHigh * RsHigh;
            uint Result = (uint)(ResultLow + ResultHigh);
            Registers[Rd] = AddSignedSetQ(Result, Registers[Rn]);
        }

        /// <summary>
        ///     Signed Multiply Accumulate Long.
        /// </summary>
        private void ARM_SMLAL()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int RdLo = (int)((Opcode >> 12) & 0xf);
            int RdHi = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);

            int RmValue = (int)Registers[Rm];
            int RsValue = (int)Registers[Rs];
            long Result = (long)RmValue * RsValue;
            Result += (long)(Registers[RdLo] | ((ulong)Registers[RdHi] << 32));
            Registers[RdLo] = (uint)Result;
            Registers[RdHi] = (uint)(Result >> 32);

            if (S)
            {
                Registers.SetFlag(ARMFlag.Zero, Registers[RdLo] == 0 && Registers[RdHi] == 0);
                Registers.SetFlag(ARMFlag.Negative, (Registers[RdHi] & 0x80000000) != 0);
            }
        }

        /// <summary>
        ///     Signed Multiply Accumulate Long.
        /// </summary>
        private void ARM_SMLAL_2()
        {
            int Rm = (int)(Opcode & 0xf);
            bool x = IsOpcodeBitSet(5);
            bool y = IsOpcodeBitSet(6);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int RdLo = (int)((Opcode >> 12) & 0xf);
            int RdHi = (int)((Opcode >> 16) & 0xf);

            int RmValue = (short)(x ? Registers[Rm] >> 16 : Registers[Rm]);
            int RsValue = (short)(y ? Registers[Rs] >> 16 : Registers[Rs]);
            long Result = (long)RmValue * RsValue;
            Result += (long)(Registers[RdLo] | ((ulong)Registers[RdHi] << 32));
            Registers[RdLo] = (uint)Result;
            Registers[RdHi] = (uint)(Result >> 32);
        }

        /// <summary>
        ///     Signed Multiply Accumulate Long Dual.
        /// </summary>
        private void ARM_SMLALD()
        {
            int Rm = (int)(Opcode & 0xf);
            bool X = IsOpcodeBitSet(5);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int RdLo = (int)((Opcode >> 12) & 0xf);
            int RdHi = (int)((Opcode >> 16) & 0xf);

            short RmLow = (short)Registers[Rm];
            short RmHigh = (short)(Registers[Rm] >> 16);

            uint RsValue = X ? ROR(Registers[Rs], 16) : Registers[Rs];
            short RsLow = (short)RsValue;
            short RsHigh = (short)(RsValue >> 16);

            int ResultLow = RmLow * RsLow;
            int ResultHigh = RmHigh * RsHigh;
            long Result = (long)ResultLow + ResultHigh;
            Result += (long)(Registers[RdLo] | ((ulong)Registers[RdHi] << 32));
            Registers[RdLo] = (uint)Result;
            Registers[RdHi] = (uint)(Result >> 32);
        }

        /// <summary>
        ///     Signed Multiply Accumulate Word.
        /// </summary>
        private void ARM_SMLAW()
        {
            int Rm = (int)(Opcode & 0xf);
            bool y = IsOpcodeBitSet(6);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int Rn = (int)((Opcode >> 12) & 0xf);
            int Rd = (int)((Opcode >> 16) & 0xf);

            int RmValue = (int)Registers[Rm];
            short RsValue = (short)(y ? Registers[Rs] >> 16 : Registers[Rs]);
            uint Result = (uint)((long)RmValue * RsValue) >> 16;
            Registers[Rd] = AddSignedSetQ(Result, Registers[Rn]);
        }

        /// <summary>
        ///     Signed Multiply Subtract Accumulate Dual.
        /// </summary>
        private void ARM_SMLSD()
        {
            int Rm = (int)(Opcode & 0xf);
            bool X = IsOpcodeBitSet(5);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int Rn = (int)((Opcode >> 12) & 0xf);
            int Rd = (int)((Opcode >> 16) & 0xf);

            short RmLow = (short)Registers[Rm];
            short RmHigh = (short)(Registers[Rm] >> 16);

            uint RsValue = X ? ROR(Registers[Rs], 16) : Registers[Rs];
            short RsLow = (short)RsValue;
            short RsHigh = (short)(RsValue >> 16);

            int ResultLow = RmLow * RsLow;
            int ResultHigh = RmHigh * RsHigh;
            uint Result = (uint)((long)ResultLow - ResultHigh);
            Registers[Rd] = AddSignedSetQ(Result, Registers[Rn]);
        }

        /// <summary>
        ///     Signed Multiply Subtract Accumulate Long Dual.
        /// </summary>
        private void ARM_SMLSLD()
        {
            int Rm = (int)(Opcode & 0xf);
            bool X = IsOpcodeBitSet(5);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int RdLo = (int)((Opcode >> 12) & 0xf);
            int RdHi = (int)((Opcode >> 16) & 0xf);

            short RmLow = (short)Registers[Rm];
            short RmHigh = (short)(Registers[Rm] >> 16);

            uint RsValue = X ? ROR(Registers[Rs], 16) : Registers[Rs];
            short RsLow = (short)RsValue;
            short RsHigh = (short)(RsValue >> 16);

            int ResultLow = RmLow * RsLow;
            int ResultHigh = RmHigh * RsHigh;
            ulong Result = (ulong)(ResultLow - ResultHigh);
            Result += Registers[RdLo] | ((ulong)Registers[RdHi] << 32);
            Registers[RdLo] = (uint)Result;
            Registers[RdHi] = (uint)(Result >> 32);
        }

        /// <summary>
        ///     Signed Most significant Word Multiply Accumulate.
        /// </summary>
        private void ARM_SMMLA()
        {
            int Rm = (int)(Opcode & 0xf);
            bool R = IsOpcodeBitSet(5);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int Rn = (int)((Opcode >> 12) & 0xf);
            int Rd = (int)((Opcode >> 16) & 0xf);

            int RmValue = (int)Registers[Rm];
            int RsValue = (int)Registers[Rs];
            int RnValue = (int)Registers[Rn];
            long Result = (long)RmValue * RsValue;
            if (R)
                Registers[Rd] = (uint)((((long)RnValue << 32) + Result + 0x80000000) >> 32);
            else
                Registers[Rd] = (uint)((((long)RnValue << 32) + Result) >> 32);
        }

        /// <summary>
        ///     Signed Most significant Word Multiply Subtract.
        /// </summary>
        private void ARM_SMMLS()
        {
            int Rm = (int)(Opcode & 0xf);
            bool R = IsOpcodeBitSet(5);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int Rn = (int)((Opcode >> 12) & 0xf);
            int Rd = (int)((Opcode >> 16) & 0xf);

            int RmValue = (int)Registers[Rm];
            int RsValue = (int)Registers[Rs];
            int RnValue = (int)Registers[Rn];
            long Result = (long)RmValue * RsValue;
            if (R)
                Registers[Rd] = (uint)((((long)RnValue << 32) - Result + 0x80000000) >> 32);
            else
                Registers[Rd] = (uint)((((long)RnValue << 32) - Result) >> 32);
        }

        /// <summary>
        ///     Signed Most significant Word Multiply.
        /// </summary>
        private void ARM_SMMUL()
        {
            int Rm = (int)(Opcode & 0xf);
            bool R = IsOpcodeBitSet(5);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int Rd = (int)((Opcode >> 16) & 0xf);

            int RmValue = (int)Registers[Rm];
            int RsValue = (int)Registers[Rs];
            long Result = (long)RmValue * RsValue;
            if (R)
                Registers[Rd] = (uint)((Result + 0x80000000) >> 32);
            else
                Registers[Rd] = (uint)(Result >> 32);
        }

        /// <summary>
        ///     Signed Multiply Add Dual.
        /// </summary>
        private void ARM_SMUAD()
        {
            int Rm = (int)(Opcode & 0xf);
            bool X = IsOpcodeBitSet(5);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int Rn = (int)((Opcode >> 12) & 0xf);
            int Rd = (int)((Opcode >> 16) & 0xf);

            short RmLow = (short)Registers[Rm];
            short RmHigh = (short)(Registers[Rm] >> 16);

            uint RsValue = X ? ROR(Registers[Rs], 16) : Registers[Rs];
            short RsLow = (short)RsValue;
            short RsHigh = (short)(RsValue >> 16);

            uint ResultLow = (uint)(RmLow * RsLow);
            uint ResultHigh = (uint)(RmHigh * RsHigh);
            Registers[Rd] = AddSignedSetQ(ResultLow, ResultHigh);
        }

        /// <summary>
        ///     Signed Multiply.
        /// </summary>
        private void ARM_SMUL()
        {
            int Rm = (int)(Opcode & 0xf);
            bool x = IsOpcodeBitSet(5);
            bool y = IsOpcodeBitSet(6);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int Rd = (int)((Opcode >> 16) & 0xf);

            int RmValue = (short)(x ? Registers[Rm] >> 16 : Registers[Rm]);
            int RsValue = (short)(y ? Registers[Rs] >> 16 : Registers[Rs]);
            Registers[Rd] = (uint)(RmValue * RsValue);
        }

        /// <summary>
        ///     Signed Multiply Long.
        /// </summary>
        private void ARM_SMULL()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int RdLo = (int)((Opcode >> 12) & 0xf);
            int RdHi = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);

            int RmValue = (int)Registers[Rm];
            int RsValue = (int)Registers[Rs];
            long Result = (long)RmValue * RsValue;
            Registers[RdLo] = (uint)Result;
            Registers[RdHi] = (uint)(Result >> 32);

            if (S)
            {
                Registers.SetFlag(ARMFlag.Zero, Registers[RdLo] == 0 && Registers[RdHi] == 0);
                Registers.SetFlag(ARMFlag.Negative, (Registers[RdHi] & 0x80000000) != 0);
            }
        }

        /// <summary>
        ///     Signed Multiply Word.
        /// </summary>
        private void ARM_SMULW()
        {
            int Rm = (int)(Opcode & 0xf);
            bool y = IsOpcodeBitSet(6);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int Rd = (int)((Opcode >> 16) & 0xf);

            int RmValue = (int)Registers[Rm];
            int RsValue = (short)(y ? Registers[Rs] >> 16 : Registers[Rs]);
            Registers[Rd] = (uint)(RmValue * RsValue);
        }

        /// <summary>
        ///     Signed Multiply Subtract Dual.
        /// </summary>
        private void ARM_SMUSD()
        {
            int Rm = (int)(Opcode & 0xf);
            bool X = IsOpcodeBitSet(5);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int Rn = (int)((Opcode >> 12) & 0xf);
            int Rd = (int)((Opcode >> 16) & 0xf);

            short RmLow = (short)Registers[Rm];
            short RmHigh = (short)(Registers[Rm] >> 16);

            uint RsValue = X ? ROR(Registers[Rs], 16) : Registers[Rs];
            short RsLow = (short)RsValue;
            short RsHigh = (short)(RsValue >> 16);

            uint ResultLow = (uint)(RmLow * RsLow);
            uint ResultHigh = (uint)(RmHigh * RsHigh);
            Registers[Rd] = ResultLow - ResultHigh;
        }

        //Unsigned Multiply

        /// <summary>
        ///     Unsigned Multiply Accumulate Accumulate Long.
        /// </summary>
        private void ARM_UMAAL()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int RdLo = (int)((Opcode >> 12) & 0xf);
            int RdHi = (int)((Opcode >> 16) & 0xf);

            ulong Result = (ulong)Registers[Rm] * Registers[Rs];
            Result += Registers[RdLo] + Registers[RdHi];
            Registers[RdLo] = (uint)Result;
            Registers[RdHi] = (uint)(Result >> 32);
        }

        /// <summary>
        ///     Unsigned Multiply Accumulate Long.
        /// </summary>
        private void ARM_UMLAL()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int RdLo = (int)((Opcode >> 12) & 0xf);
            int RdHi = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);

            ulong Result = (ulong)Registers[Rm] * Registers[Rs];
            Result += Registers[RdLo] | ((ulong)Registers[RdHi] << 32);
            Registers[RdLo] = (uint)Result;
            Registers[RdHi] = (uint)(Result >> 32);

            if (S)
            {
                Registers.SetFlag(ARMFlag.Zero, Registers[RdLo] == 0 && Registers[RdHi] == 0);
                Registers.SetFlag(ARMFlag.Negative, (Registers[RdHi] & 0x80000000) != 0);
            }
        }

        /// <summary>
        ///     Unsigned Multiply Long.
        /// </summary>
        private void ARM_UMULL()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int RdLo = (int)((Opcode >> 12) & 0xf);
            int RdHi = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);

            ulong Result = (ulong)Registers[Rm] * Registers[Rs];
            Registers[RdLo] = (uint)Result;
            Registers[RdHi] = (uint)(Result >> 32);

            if (S)
            {
                Registers.SetFlag(ARMFlag.Zero, Registers[RdLo] == 0 && Registers[RdHi] == 0);
                Registers.SetFlag(ARMFlag.Negative, (Registers[RdHi] & 0x80000000) != 0);
            }
        }

        /*
         * Utils
         */

        /// <summary>
        ///     Adds two 32-bits values, and automatically sets the Saturation (Q) flag if a saturation occurs.
        ///     Note that the result will not be saturated, but the Q flag is set on overflow.
        /// </summary>
        /// <param name="Left">First value of the addition</param>
        /// <param name="Right">Second value of the addition</param>
        /// <returns>The addition result</returns>
        private uint AddSignedSetQ(uint Left, uint Right)
        {
            uint Result = Left + Right;
            if (AddOverflow(Left, Right, Result)) Registers.SetFlag(ARMFlag.Saturation, true);

            return Result;
        }
    }
}