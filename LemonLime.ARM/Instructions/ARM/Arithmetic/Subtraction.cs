namespace LemonLime.ARM
{
    public partial class Interpreter
    {
        /*
         * Instructions
         */

        //Saturating Subtract

        /// <summary>
        ///     Saturating Double and Subtract.
        /// </summary>
        private void ARM_QDSUB()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            uint Double = AddSignedSaturate(Registers[Rn], Registers[Rn]);
            Registers[Rd] = SubtractSignedSaturate(Registers[Rm], Double);
        }

        /// <summary>
        ///     Saturating Subtract.
        /// </summary>
        private void ARM_QSUB()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            Registers[Rd] = SubtractSignedSaturate(Registers[Rn], Registers[Rm]);
        }

        /// <summary>
        ///     Saturating Subtract 16-bits.
        /// </summary>
        private void ARM_QSUB16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            ushort RmLow = (ushort)Registers[Rm];
            ushort RmHigh = (ushort)(Registers[Rm] >> 16);

            ushort RnLow = (ushort)Registers[Rn];
            ushort RnHigh = (ushort)(Registers[Rn] >> 16);

            Registers[Rd] = SubtractSignedSaturate16(RnLow, RmLow);
            Registers[Rd] |= SubtractSignedSaturate16(RnHigh, RmHigh) << 16;
        }

        /// <summary>
        ///     Saturating Subtract 8-bits.
        /// </summary>
        private void ARM_QSUB8()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            byte Rm0 = (byte)Registers[Rm];
            byte Rm1 = (byte)(Registers[Rm] >> 8);
            byte Rm2 = (byte)(Registers[Rm] >> 16);
            byte Rm3 = (byte)(Registers[Rm] >> 24);

            byte Rn0 = (byte)Registers[Rn];
            byte Rn1 = (byte)(Registers[Rn] >> 8);
            byte Rn2 = (byte)(Registers[Rn] >> 16);
            byte Rn3 = (byte)(Registers[Rn] >> 24);

            Registers[Rd] = SubtractSignedSaturate8(Rn0, Rm0);
            Registers[Rd] |= SubtractSignedSaturate8(Rn1, Rm1) << 8;
            Registers[Rd] |= SubtractSignedSaturate8(Rn2, Rm2) << 16;
            Registers[Rd] |= SubtractSignedSaturate8(Rn3, Rm3) << 24;
        }

        /// <summary>
        ///     Saturating Subtract and Add with Exchange.
        /// </summary>
        private void ARM_QSUBADDX()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            ushort RmLow = (ushort)Registers[Rm];
            ushort RmHigh = (ushort)(Registers[Rm] >> 16);

            ushort RnLow = (ushort)Registers[Rn];
            ushort RnHigh = (ushort)(Registers[Rn] >> 16);

            Registers[Rd] = AddSignedSaturate16(RnLow, RmHigh);
            Registers[Rd] |= SubtractSignedSaturate16(RnHigh, RmLow) << 16;
        }

        //Subtract

        /// <summary>
        ///     Reverse Subtract.
        /// </summary>
        private void ARM_RSB()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);
            bool UpdateFlags = S && Rd != 15;
            uint ShifterOperand = ARM_GetShifterOperand(UpdateFlags);

            Registers[Rd] = Subtract(ShifterOperand, Registers[Rn], false, UpdateFlags);
            if (S && Rd == 15) Registers.CPSR = Registers.SPSR;
        }

        /// <summary>
        ///     Reverse Subtract with Carry.
        /// </summary>
        private void ARM_RSC()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);
            bool UpdateFlags = S && Rd != 15;
            uint ShifterOperand = ARM_GetShifterOperand(UpdateFlags);

            Registers[Rd] = Subtract(ShifterOperand, Registers[Rn], true, UpdateFlags);
            if (S && Rd == 15) Registers.CPSR = Registers.SPSR;
        }

        /// <summary>
        ///     Subtract with Carry.
        /// </summary>
        private void ARM_SBC()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);
            bool UpdateFlags = S && Rd != 15;
            uint ShifterOperand = ARM_GetShifterOperand(UpdateFlags);

            Registers[Rd] = Subtract(Registers[Rn], ShifterOperand, true, UpdateFlags);
            if (S && Rd == 15) Registers.CPSR = Registers.SPSR;
        }

        //Signed Subtract

        /// <summary>
        ///     Signed Halving Subtract 16-bits.
        /// </summary>
        private void ARM_SHSUB16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            short RmLow = (short)Registers[Rm];
            short RmHigh = (short)(Registers[Rm] >> 16);

            short RnLow = (short)Registers[Rn];
            short RnHigh = (short)(Registers[Rn] >> 16);

            int ResultLow = (RnLow - RmLow) >> 1;
            int ResultHigh = (RnHigh - RmHigh) >> 1;

            Registers[Rd] = (uint)(ResultLow & 0xffff);
            Registers[Rd] |= (uint)((ResultHigh & 0xffff) << 16);
        }

        /// <summary>
        ///     Signed Halving Subtract 8-bits.
        /// </summary>
        private void ARM_SHSUB8()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            sbyte Rm0 = (sbyte)Registers[Rm];
            sbyte Rm1 = (sbyte)(Registers[Rm] >> 8);
            sbyte Rm2 = (sbyte)(Registers[Rm] >> 16);
            sbyte Rm3 = (sbyte)(Registers[Rm] >> 24);

            sbyte Rn0 = (sbyte)Registers[Rn];
            sbyte Rn1 = (sbyte)(Registers[Rn] >> 8);
            sbyte Rn2 = (sbyte)(Registers[Rn] >> 16);
            sbyte Rn3 = (sbyte)(Registers[Rn] >> 24);

            int Result0 = (Rn0 - Rm0) >> 1;
            int Result1 = (Rn1 - Rm1) >> 1;
            int Result2 = (Rn2 - Rm2) >> 1;
            int Result3 = (Rn3 - Rm3) >> 1;

            Registers[Rd] = (uint)(Result0 & 0xff);
            Registers[Rd] |= (uint)((Result1 & 0xff) << 8);
            Registers[Rd] |= (uint)((Result2 & 0xff) << 16);
            Registers[Rd] |= (uint)((Result3 & 0xff) << 24);
        }

        /// <summary>
        ///     Signed Halving Subtract and Add with Exchange.
        /// </summary>
        private void ARM_SHSUBADDX()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            short RmLow = (short)Registers[Rm];
            short RmHigh = (short)(Registers[Rm] >> 16);

            short RnLow = (short)Registers[Rn];
            short RnHigh = (short)(Registers[Rn] >> 16);

            int ResultLow = (RnLow + RmHigh) >> 1;
            int ResultHigh = (RnHigh - RmLow) >> 1;

            Registers[Rd] = (uint)(ResultLow & 0xffff);
            Registers[Rd] |= (uint)((ResultHigh & 0xffff) << 16);
        }

        /// <summary>
        ///     Signed Subtract 16-bits.
        /// </summary>
        private void ARM_SSUB16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            short RmLow = (short)Registers[Rm];
            short RmHigh = (short)(Registers[Rm] >> 16);

            short RnLow = (short)Registers[Rn];
            short RnHigh = (short)(Registers[Rn] >> 16);

            int ResultLow = RnLow - RmLow;
            int ResultHigh = RnHigh - RmHigh;

            Registers.GE = 0;
            if (ResultLow >= 0) Registers.GE = 3;
            if (ResultHigh >= 0) Registers.GE |= 0xc;

            Registers[Rd] = (uint)(ResultLow & 0xffff);
            Registers[Rd] |= (uint)((ResultHigh & 0xffff) << 16);
        }

        /// <summary>
        ///     Signed Subtract 8-bits.
        /// </summary>
        private void ARM_SSUB8()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            sbyte Rm0 = (sbyte)Registers[Rm];
            sbyte Rm1 = (sbyte)(Registers[Rm] >> 8);
            sbyte Rm2 = (sbyte)(Registers[Rm] >> 16);
            sbyte Rm3 = (sbyte)(Registers[Rm] >> 24);

            sbyte Rn0 = (sbyte)Registers[Rn];
            sbyte Rn1 = (sbyte)(Registers[Rn] >> 8);
            sbyte Rn2 = (sbyte)(Registers[Rn] >> 16);
            sbyte Rn3 = (sbyte)(Registers[Rn] >> 24);

            int Result0 = Rn0 - Rm0;
            int Result1 = Rn1 - Rm1;
            int Result2 = Rn2 - Rm2;
            int Result3 = Rn3 - Rm3;

            Registers.GE = 0;
            if (Result0 >= 0) Registers.GE = 1;
            if (Result1 >= 0) Registers.GE |= 2;
            if (Result2 >= 0) Registers.GE |= 4;
            if (Result3 >= 0) Registers.GE |= 8;

            Registers[Rd] = (uint)(Result0 & 0xff);
            Registers[Rd] |= (uint)((Result1 & 0xff) << 8);
            Registers[Rd] |= (uint)((Result2 & 0xff) << 16);
            Registers[Rd] |= (uint)((Result3 & 0xff) << 24);
        }

        /// <summary>
        ///     Signed Subtract and Add with Exchange.
        /// </summary>
        private void ARM_SSUBADDX()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            short RmLow = (short)Registers[Rm];
            short RmHigh = (short)(Registers[Rm] >> 16);

            short RnLow = (short)Registers[Rn];
            short RnHigh = (short)(Registers[Rn] >> 16);

            int ResultLow = RnLow + RmHigh;
            int ResultHigh = RnHigh - RmLow;

            Registers.GE = 0;
            if (ResultLow >= 0) Registers.GE = 3;
            if (ResultHigh >= 0) Registers.GE |= 0xc;

            Registers[Rd] = (uint)(ResultLow & 0xffff);
            Registers[Rd] |= (uint)((ResultHigh & 0xffff) << 16);
        }

        /// <summary>
        ///     Subtract.
        /// </summary>
        private void ARM_SUB()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);
            bool UpdateFlags = S && Rd != 15;
            uint ShifterOperand = ARM_GetShifterOperand(UpdateFlags);

            Registers[Rd] = Subtract(Registers[Rn], ShifterOperand, false, UpdateFlags);
            if (S && Rd == 15) Registers.CPSR = Registers.SPSR;
        }

        //Unsigned Subtract

        /// <summary>
        ///     Unsigned Saturating Subtract 16-bits.
        /// </summary>
        private void ARM_UQSUB16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            ushort RmLow = (ushort)Registers[Rm];
            ushort RmHigh = (ushort)(Registers[Rm] >> 16);

            ushort RnLow = (ushort)Registers[Rn];
            ushort RnHigh = (ushort)(Registers[Rn] >> 16);

            Registers[Rd] = SubtractUnsignedSaturate16(RnLow, RmLow);
            Registers[Rd] |= SubtractUnsignedSaturate16(RnHigh, RmHigh) << 16;
        }

        /// <summary>
        ///     Unsigned Saturating Subtract 8-bits.
        /// </summary>
        private void ARM_UQSUB8()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            byte Rm0 = (byte)Registers[Rm];
            byte Rm1 = (byte)(Registers[Rm] >> 8);
            byte Rm2 = (byte)(Registers[Rm] >> 16);
            byte Rm3 = (byte)(Registers[Rm] >> 24);

            byte Rn0 = (byte)Registers[Rn];
            byte Rn1 = (byte)(Registers[Rn] >> 8);
            byte Rn2 = (byte)(Registers[Rn] >> 16);
            byte Rn3 = (byte)(Registers[Rn] >> 24);

            Registers[Rd] = SubtractUnsignedSaturate8(Rn0, Rm0);
            Registers[Rd] |= SubtractUnsignedSaturate8(Rn1, Rm1) << 8;
            Registers[Rd] |= SubtractUnsignedSaturate8(Rn2, Rm2) << 16;
            Registers[Rd] |= SubtractUnsignedSaturate8(Rn3, Rm3) << 24;
        }

        /// <summary>
        ///     Unsigned Saturating Subtract and Add with Exchange.
        /// </summary>
        private void ARM_UQSUBADDX()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            ushort RmLow = (ushort)Registers[Rm];
            ushort RmHigh = (ushort)(Registers[Rm] >> 16);

            ushort RnLow = (ushort)Registers[Rn];
            ushort RnHigh = (ushort)(Registers[Rn] >> 16);

            Registers[Rd] = AddUnsignedSaturate16(RnLow, RmHigh);
            Registers[Rd] |= SubtractUnsignedSaturate16(RnHigh, RmLow) << 16;
        }

        /// <summary>
        ///     Unsigned Subtract 16-bits.
        /// </summary>
        private void ARM_USUB16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            ushort RmLow = (ushort)Registers[Rm];
            ushort RmHigh = (ushort)(Registers[Rm] >> 16);

            ushort RnLow = (ushort)Registers[Rn];
            ushort RnHigh = (ushort)(Registers[Rn] >> 16);

            int ResultLow = RnLow - RmLow;
            int ResultHigh = RnHigh - RmHigh;

            Registers.GE = 0;
            if (ResultLow >= 0) Registers.GE = 3;
            if (ResultHigh >= 0) Registers.GE |= 0xc;

            Registers[Rd] = (uint)(ResultLow & 0xffff);
            Registers[Rd] |= (uint)((ResultHigh & 0xffff) << 16);
        }

        /// <summary>
        ///     Unsigned Subtract 8-bits.
        /// </summary>
        private void ARM_USUB8()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            byte Rm0 = (byte)Registers[Rm];
            byte Rm1 = (byte)(Registers[Rm] >> 8);
            byte Rm2 = (byte)(Registers[Rm] >> 16);
            byte Rm3 = (byte)(Registers[Rm] >> 24);

            byte Rn0 = (byte)Registers[Rn];
            byte Rn1 = (byte)(Registers[Rn] >> 8);
            byte Rn2 = (byte)(Registers[Rn] >> 16);
            byte Rn3 = (byte)(Registers[Rn] >> 24);

            int Result0 = Rn0 - Rm0;
            int Result1 = Rn1 - Rm1;
            int Result2 = Rn2 - Rm2;
            int Result3 = Rn3 - Rm3;

            Registers.GE = 0;
            if (Result0 >= 0) Registers.GE = 1;
            if (Result1 >= 0) Registers.GE |= 2;
            if (Result2 >= 0) Registers.GE |= 4;
            if (Result3 >= 0) Registers.GE |= 8;

            Registers[Rd] = (uint)(Result0 & 0xff);
            Registers[Rd] |= (uint)((Result1 & 0xff) << 8);
            Registers[Rd] |= (uint)((Result2 & 0xff) << 16);
            Registers[Rd] |= (uint)((Result3 & 0xff) << 24);
        }

        /// <summary>
        ///     Unsigned Subtract and Add with Exchange.
        /// </summary>
        private void ARM_USUBADDX()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            ushort RmLow = (ushort)Registers[Rm];
            ushort RmHigh = (ushort)(Registers[Rm] >> 16);

            ushort RnLow = (ushort)Registers[Rn];
            ushort RnHigh = (ushort)(Registers[Rn] >> 16);

            int ResultLow = RnLow + RmHigh;
            int ResultHigh = RnHigh - RmLow;

            Registers.GE = 0;
            if (ResultLow >= 0) Registers.GE = 3;
            if (ResultHigh >= 0) Registers.GE |= 0xc;

            Registers[Rd] = (uint)(ResultLow & 0xffff);
            Registers[Rd] |= (uint)((ResultHigh & 0xffff) << 16);
        }

        /*
         * Utils
         */

        /// <summary>
        ///     Subtracts two 32-bits values (Left and Right), and then subtracts the negated Carry value.
        ///     It automatically set the Overflow, Carry, Zero and Negative flags (if S is true).
        /// </summary>
        /// <param name="Left">The first value to be subtracted</param>
        /// <param name="Right">The second value to be subtracted</param>
        /// <param name="C">True if the Carry flag should be used, false otherwise</param>
        /// <param name="S">True if the flags should be updated, false otherwise</param>
        /// <returns>The result of the subtraction</returns>
        private uint Subtract(uint Left, uint Right, bool C, bool S)
        {
            uint Carry = C ? (~Registers.CPSR >> 29) & 1 : 0;
            ulong Result64 = (ulong)Left - Right - Carry;
            uint Result = (uint)Result64;

            if (S)
            {
                Registers.SetFlag(ARMFlag.Carry, Result64 < 0x100000000);
                Registers.SetFlag(ARMFlag.Overflow, SubtractOverflow(Left, Right, Result));
                SetZNFlags(Result);
            }

            return Result;
        }

        //Overflow checks

        /// <summary>
        ///     Checks whenever a 32-bits subtraction overflowed.
        /// </summary>
        /// <param name="Left">First value of the subtraction</param>
        /// <param name="Right">Second value of the subtraction</param>
        /// <param name="Result">Result of the subtraction</param>
        /// <returns>True if the result overflowed, false otherwise</returns>
        private bool SubtractOverflow(uint Left, uint Right, uint Result)
        {
            return ((Left ^ Right) & (Left ^ Result) & 0x80000000) != 0;
        }

        /// <summary>
        ///     Checks whenever a 16-bits subtraction overflowed.
        /// </summary>
        /// <param name="Left">First value of the subtraction</param>
        /// <param name="Right">Second value of the subtraction</param>
        /// <param name="Result">Result of the subtraction</param>
        /// <returns>True if the result overflowed, false otherwise</returns>
        private bool SubtractOverflow16(ushort Left, ushort Right, uint Result)
        {
            return ((Left ^ Right) & (Left ^ Result) & 0x8000) != 0;
        }

        /// <summary>
        ///     Checks whenever a 8-bits subtraction overflowed.
        /// </summary>
        /// <param name="Left">First value of the subtraction</param>
        /// <param name="Right">Second value of the subtraction</param>
        /// <param name="Result">Result of the subtraction</param>
        /// <returns>True if the result overflowed, false otherwise</returns>
        private bool SubtractOverflow8(byte Left, byte Right, uint Result)
        {
            return ((Left ^ Right) & (Left ^ Result) & 0x80) != 0;
        }

        //Signed

        /// <summary>
        ///     Subtracts two 32-bits values, and saturates the result to the signed 32-bits range.
        ///     It also automatically sets the Saturation (Q) flag if a saturation occurs.
        /// </summary>
        /// <param name="Left">First value of the subtraction</param>
        /// <param name="Right">Second value of the subtraction</param>
        /// <returns>The result saturated into the 32-bits range</returns>
        private uint SubtractSignedSaturate(uint Left, uint Right)
        {
            uint Result = Left - Right;
            if (SubtractOverflow(Left, Right, Result))
            {
                Result = (Result & 0x80000000) != 0 ? 0x80000000 : 0x7fffffff;
                Registers.SetFlag(ARMFlag.Saturation, true);
            }

            return Result;
        }

        /// <summary>
        ///     Subtracts two 16-bits values, and saturates the result to the signed 16-bits range.
        ///     It also automatically sets the Saturation (Q) flag if a saturation occurs.
        /// </summary>
        /// <param name="Left">First value of the subtraction</param>
        /// <param name="Right">Second value of the subtraction</param>
        /// <returns>The result saturated into the 16-bits range</returns>
        private uint SubtractSignedSaturate16(ushort Left, ushort Right)
        {
            uint Result = (uint)(Left - Right);
            if (SubtractOverflow16(Left, Right, Result))
            {
                Result = (uint)((Result & 0x8000) != 0 ? 0x8000 : 0x7fff);
                Registers.SetFlag(ARMFlag.Saturation, true);
            }

            return Result;
        }

        /// <summary>
        ///     Subtracts two 8-bits values, and saturates the result to the signed 8-bits range.
        ///     It also automatically sets the Saturation (Q) flag if a saturation occurs.
        /// </summary>
        /// <param name="Left">First value of the subtraction</param>
        /// <param name="Right">Second value of the subtraction</param>
        /// <returns>The result saturated into the 8-bits range</returns>
        private uint SubtractSignedSaturate8(byte Left, byte Right)
        {
            uint Result = (uint)(Left - Right);
            if (SubtractOverflow8(Left, Right, Result))
            {
                Result = (uint)((Result & 0x80) != 0 ? 0x80 : 0x7f);
                Registers.SetFlag(ARMFlag.Saturation, true);
            }

            return Result;
        }

        //Unsigned

        /// <summary>
        ///     Subtracts two 32-bits values, and saturates the result to the unsigned 32-bits range.
        ///     It also automatically sets the Saturation (Q) flag if a saturation occurs.
        /// </summary>
        /// <param name="Left">First value of the subtraction</param>
        /// <param name="Right">Second value of the subtraction</param>
        /// <returns>The result saturated into the 32-bits range</returns>
        private uint SubtractUnsignedSaturate(uint Left, uint Right)
        {
            ulong Result = (ulong)Left - Right;
            if (Result > 0xffffffff)
            {
                Result = 0;
                Registers.SetFlag(ARMFlag.Saturation, true);
            }

            return (uint)Result;
        }

        /// <summary>
        ///     Subtracts two 16-bits values, and saturates the result to the unsigned 16-bits range.
        ///     It also automatically sets the Saturation (Q) flag if a saturation occurs.
        /// </summary>
        /// <param name="Left">First value of the subtraction</param>
        /// <param name="Right">Second value of the subtraction</param>
        /// <returns>The result saturated into the 16-bits range</returns>
        private uint SubtractUnsignedSaturate16(ushort Left, ushort Right)
        {
            uint Result = (uint)Left - Right;
            if (Result > 0xffff)
            {
                Result = 0;
                Registers.SetFlag(ARMFlag.Saturation, true);
            }

            return Result;
        }

        /// <summary>
        ///     Subtracts two 8-bits values, and saturates the result to the unsigned 8-bits range.
        ///     It also automatically sets the Saturation (Q) flag if a saturation occurs.
        /// </summary>
        /// <param name="Left">First value of the subtraction</param>
        /// <param name="Right">Second value of the subtraction</param>
        /// <returns>The result saturated into the 8-bits range</returns>
        private uint SubtractUnsignedSaturate8(byte Left, byte Right)
        {
            uint Result = (uint)Left - Right;
            if (Result > 0xff)
            {
                Result = 0;
                Registers.SetFlag(ARMFlag.Saturation, true);
            }

            return Result;
        }
    }
}
