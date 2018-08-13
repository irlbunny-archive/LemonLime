using System;

namespace CTREmulator.ARM
{
    public partial class Interpreter
    {
        /*
         * Instructions
         */

        //Add

        /// <summary>
        ///     Add with Carry.
        /// </summary>
        private void ARM_ADC()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);
            bool UpdateFlags = S && Rd != 15;
            uint ShifterOperand = ARM_GetShifterOperand(UpdateFlags);

            Registers[Rd] = Add(Registers[Rn], ShifterOperand, true, UpdateFlags);
            if (S && Rd == 15) Registers.CPSR = Registers.SPSR;
        }

        /// <summary>
        ///     Add.
        /// </summary>
        private void ARM_ADD()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool S = IsOpcodeBitSet(20);
            bool UpdateFlags = S && Rd != 15;
            uint ShifterOperand = ARM_GetShifterOperand(UpdateFlags);

            Registers[Rd] = Add(Registers[Rn], ShifterOperand, false, UpdateFlags);
            if (S && Rd == 15) Registers.CPSR = Registers.SPSR;
        }

        //Saturating Add

        /// <summary>
        ///     Saturating Add.
        /// </summary>
        private void ARM_QADD()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            Registers[Rd] = AddSignedSaturate(Registers[Rn], Registers[Rm]);
        }

        /// <summary>
        ///     Saturating Add 16-bits.
        /// </summary>
        private void ARM_QADD16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            ushort RmLow = (ushort)Registers[Rm];
            ushort RmHigh = (ushort)(Registers[Rm] >> 16);

            ushort RnLow = (ushort)Registers[Rn];
            ushort RnHigh = (ushort)(Registers[Rn] >> 16);

            Registers[Rd] = AddSignedSaturate16(RnLow, RmLow);
            Registers[Rd] |= AddSignedSaturate16(RnHigh, RmHigh) << 16;
        }

        /// <summary>
        ///     Saturating Add 8-bits.
        /// </summary>
        private void ARM_QADD8()
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

            Registers[Rd] = AddSignedSaturate8(Rn0, Rm0);
            Registers[Rd] |= AddSignedSaturate8(Rn1, Rm1) << 8;
            Registers[Rd] |= AddSignedSaturate8(Rn2, Rm2) << 16;
            Registers[Rd] |= AddSignedSaturate8(Rn3, Rm3) << 24;
        }

        /// <summary>
        ///     Saturating Add and Subtract with Exchange.
        /// </summary>
        private void ARM_QADDSUBX()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            ushort RmLow = (ushort)Registers[Rm];
            ushort RmHigh = (ushort)(Registers[Rm] >> 16);

            ushort RnLow = (ushort)Registers[Rn];
            ushort RnHigh = (ushort)(Registers[Rn] >> 16);

            Registers[Rd] = SubtractSignedSaturate16(RnLow, RmHigh);
            Registers[Rd] |= AddSignedSaturate16(RnHigh, RmLow) << 16;
        }

        /// <summary>
        ///     Saturating Double and Add.
        /// </summary>
        private void ARM_QDADD()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            uint Double = AddSignedSaturate(Registers[Rn], Registers[Rn]);
            Registers[Rd] = AddSignedSaturate(Registers[Rm], Double);
        }

        //Signed Add

        /// <summary>
        ///     Signed Add 16-bits.
        /// </summary>
        private void ARM_SADD16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            short RmLow = (short)Registers[Rm];
            short RmHigh = (short)(Registers[Rm] >> 16);

            short RnLow = (short)Registers[Rn];
            short RnHigh = (short)(Registers[Rn] >> 16);

            int ResultLow = RnLow + RmLow;
            int ResultHigh = RnHigh + RmHigh;

            Registers.GE = 0;
            if (ResultLow >= 0) Registers.GE = 3;
            if (ResultHigh >= 0) Registers.GE |= 0xc;

            Registers[Rd] = (uint)(ResultLow & 0xffff);
            Registers[Rd] |= (uint)((ResultHigh & 0xffff) << 16);
        }

        /// <summary>
        ///     Signed Add 8-bits.
        /// </summary>
        private void ARM_SADD8()
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

            int Result0 = Rn0 + Rm0;
            int Result1 = Rn1 + Rm1;
            int Result2 = Rn2 + Rm2;
            int Result3 = Rn3 + Rm3;

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
        ///     Signed Add and Subtract with Exchange.
        /// </summary>
        private void ARM_SADDSUBX()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            short RmLow = (short)Registers[Rm];
            short RmHigh = (short)(Registers[Rm] >> 16);

            short RnLow = (short)Registers[Rn];
            short RnHigh = (short)(Registers[Rn] >> 16);

            int ResultLow = RnLow - RmHigh;
            int ResultHigh = RnHigh + RmLow;

            Registers.GE = 0;
            if (ResultLow >= 0) Registers.GE = 3;
            if (ResultHigh >= 0) Registers.GE |= 0xc;

            Registers[Rd] = (uint)(ResultLow & 0xffff);
            Registers[Rd] |= (uint)((ResultHigh & 0xffff) << 16);
        }

        /// <summary>
        ///     Signed Halving Add 16-bits.
        /// </summary>
        private void ARM_SHADD16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            short RmLow = (short)Registers[Rm];
            short RmHigh = (short)(Registers[Rm] >> 16);

            short RnLow = (short)Registers[Rn];
            short RnHigh = (short)(Registers[Rn] >> 16);

            int ResultLow = (RnLow + RmLow) >> 1;
            int ResultHigh = (RnHigh + RmHigh) >> 1;

            Registers[Rd] = (uint)(ResultLow & 0xffff);
            Registers[Rd] |= (uint)((ResultHigh & 0xffff) << 16);
        }

        /// <summary>
        ///     Signed Halving Add 8-bits.
        /// </summary>
        private void ARM_SHADD8()
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

            int Result0 = (Rn0 + Rm0) >> 1;
            int Result1 = (Rn1 + Rm1) >> 1;
            int Result2 = (Rn2 + Rm2) >> 1;
            int Result3 = (Rn3 + Rm3) >> 1;

            Registers[Rd] = (uint)(Result0 & 0xff);
            Registers[Rd] |= (uint)((Result1 & 0xff) << 8);
            Registers[Rd] |= (uint)((Result2 & 0xff) << 16);
            Registers[Rd] |= (uint)((Result3 & 0xff) << 24);
        }

        /// <summary>
        ///     Signed Halving Add and Subtract with Exchange.
        /// </summary>
        private void ARM_SHADDSUBX()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            short RmLow = (short)Registers[Rm];
            short RmHigh = (short)(Registers[Rm] >> 16);

            short RnLow = (short)Registers[Rn];
            short RnHigh = (short)(Registers[Rn] >> 16);

            int ResultLow = (RnLow - RmHigh) >> 1;
            int ResultHigh = (RnHigh + RmLow) >> 1;

            Registers[Rd] = (uint)(ResultLow & 0xffff);
            Registers[Rd] |= (uint)((ResultHigh & 0xffff) << 16);
        }

        //Unsigned Add

        /// <summary>
        ///     Unsigned Add 16-bits.
        /// </summary>
        private void ARM_UADD16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            ushort RmLow = (ushort)Registers[Rm];
            ushort RmHigh = (ushort)(Registers[Rm] >> 16);

            ushort RnLow = (ushort)Registers[Rn];
            ushort RnHigh = (ushort)(Registers[Rn] >> 16);

            uint ResultLow = (uint)(RnLow + RmLow);
            uint ResultHigh = (uint)(RnHigh + RmHigh);

            Registers.GE = 0;
            if (ResultLow > 0xffff) Registers.GE = 3;
            if (ResultHigh > 0xffff) Registers.GE |= 0xc;

            Registers[Rd] = ResultLow & 0xffff;
            Registers[Rd] |= (ResultHigh & 0xffff) << 16;
        }

        /// <summary>
        ///     Unsigned Add 8-bits.
        /// </summary>
        private void ARM_UADD8()
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

            uint Result0 = (uint)(Rn0 + Rm0);
            uint Result1 = (uint)(Rn1 + Rm1);
            uint Result2 = (uint)(Rn2 + Rm2);
            uint Result3 = (uint)(Rn3 + Rm3);

            Registers.GE = 0;
            if (Result0 > 0xff) Registers.GE = 1;
            if (Result1 > 0xff) Registers.GE |= 2;
            if (Result2 > 0xff) Registers.GE |= 4;
            if (Result3 > 0xff) Registers.GE |= 8;

            Registers[Rd] = Result0 & 0xff;
            Registers[Rd] |= (Result1 & 0xff) << 8;
            Registers[Rd] |= (Result2 & 0xff) << 16;
            Registers[Rd] |= (Result3 & 0xff) << 24;
        }

        /// <summary>
        ///     Unsigned Add and Subtract with Exchange.
        /// </summary>
        private void ARM_UADDSUBX()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            ushort RmLow = (ushort)Registers[Rm];
            ushort RmHigh = (ushort)(Registers[Rm] >> 16);

            ushort RnLow = (ushort)Registers[Rn];
            ushort RnHigh = (ushort)(Registers[Rn] >> 16);

            uint ResultLow = (uint)(RnLow - RmHigh);
            uint ResultHigh = (uint)(RnHigh + RmLow);

            Registers.GE = 0;
            if (ResultLow > 0xffff) Registers.GE = 3;
            if (ResultHigh > 0xffff) Registers.GE |= 0xc;

            Registers[Rd] = ResultLow & 0xffff;
            Registers[Rd] |= (ResultHigh & 0xffff) << 16;
        }

        /// <summary>
        ///     Unsigned Halving Add 16-bits.
        /// </summary>
        private void ARM_UHADD16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            ushort RmLow = (ushort)Registers[Rm];
            ushort RmHigh = (ushort)(Registers[Rm] >> 16);

            ushort RnLow = (ushort)Registers[Rn];
            ushort RnHigh = (ushort)(Registers[Rn] >> 16);

            uint ResultLow = (uint)(RnLow + RmLow) >> 1;
            uint ResultHigh = (uint)(RnHigh + RmHigh) >> 1;

            Registers[Rd] = ResultLow & 0xffff;
            Registers[Rd] |= (ResultHigh & 0xffff) << 16;
        }

        /// <summary>
        ///     Unsigned Halving Add 8-bits.
        /// </summary>
        private void ARM_UHADD8()
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

            uint Result0 = (uint)(Rn0 + Rm0) >> 1;
            uint Result1 = (uint)(Rn1 + Rm1) >> 1;
            uint Result2 = (uint)(Rn2 + Rm2) >> 1;
            uint Result3 = (uint)(Rn3 + Rm3) >> 1;

            Registers[Rd] = Result0 & 0xff;
            Registers[Rd] |= (Result1 & 0xff) << 8;
            Registers[Rd] |= (Result2 & 0xff) << 16;
            Registers[Rd] |= (Result3 & 0xff) << 24;
        }

        /// <summary>
        ///     Unsigned Halving Add and Subtract with Exchange.
        /// </summary>
        private void ARM_UHADDSUBX()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            ushort RmLow = (ushort)Registers[Rm];
            ushort RmHigh = (ushort)(Registers[Rm] >> 16);

            ushort RnLow = (ushort)Registers[Rn];
            ushort RnHigh = (ushort)(Registers[Rn] >> 16);

            uint ResultLow = (uint)(RnLow - RmHigh) >> 1;
            uint ResultHigh = (uint)(RnHigh + RmLow) >> 1;

            Registers[Rd] = ResultLow & 0xffff;
            Registers[Rd] |= (ResultHigh & 0xffff) << 16;
        }

        /// <summary>
        ///     Unsigned Saturating Add 16-bits.
        /// </summary>
        private void ARM_UQADD16()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            ushort RmLow = (ushort)Registers[Rm];
            ushort RmHigh = (ushort)(Registers[Rm] >> 16);

            ushort RnLow = (ushort)Registers[Rn];
            ushort RnHigh = (ushort)(Registers[Rn] >> 16);

            Registers[Rd] = AddUnsignedSaturate16(RnLow, RmLow);
            Registers[Rd] |= AddUnsignedSaturate16(RnHigh, RmHigh) << 16;
        }

        /// <summary>
        ///     Unsigned Saturating Add 8-bits.
        /// </summary>
        private void ARM_UQADD8()
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

            Registers[Rd] = AddUnsignedSaturate8(Rn0, Rm0);
            Registers[Rd] |= AddUnsignedSaturate8(Rn1, Rm1) << 8;
            Registers[Rd] |= AddUnsignedSaturate8(Rn2, Rm2) << 16;
            Registers[Rd] |= AddUnsignedSaturate8(Rn3, Rm3) << 24;
        }

        /// <summary>
        ///     Unsigned Saturating Add and Subtract with Exchange.
        /// </summary>
        private void ARM_UQADDSUBX()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            ushort RmLow = (ushort)Registers[Rm];
            ushort RmHigh = (ushort)(Registers[Rm] >> 16);

            ushort RnLow = (ushort)Registers[Rn];
            ushort RnHigh = (ushort)(Registers[Rn] >> 16);

            Registers[Rd] = SubtractUnsignedSaturate16(RnLow, RmHigh);
            Registers[Rd] |= AddUnsignedSaturate16(RnHigh, RmLow) << 16;
        }

        /// <summary>
        ///     Unsigned Sum of Absolute Differences 8-bits.
        /// </summary>
        private void ARM_USAD8()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int Rd = (int)((Opcode >> 16) & 0xf);

            byte Rm0 = (byte)Registers[Rm];
            byte Rm1 = (byte)(Registers[Rm] >> 8);
            byte Rm2 = (byte)(Registers[Rm] >> 16);
            byte Rm3 = (byte)(Registers[Rm] >> 24);

            byte Rs0 = (byte)Registers[Rs];
            byte Rs1 = (byte)(Registers[Rs] >> 8);
            byte Rs2 = (byte)(Registers[Rs] >> 16);
            byte Rs3 = (byte)(Registers[Rs] >> 24);

            int Result0 = Math.Abs(Rm0 - Rs0);
            int Result1 = Math.Abs(Rm1 - Rs1);
            int Result2 = Math.Abs(Rm2 - Rs2);
            int Result3 = Math.Abs(Rm3 - Rs3);

            Registers[Rd] = (uint)Result0;
            Registers[Rd] += (uint)Result1;
            Registers[Rd] += (uint)Result2;
            Registers[Rd] += (uint)Result3;
        }

        /// <summary>
        ///     Unsigned Sum of Absolute Differences and Accumulate 8-bits.
        /// </summary>
        private void ARM_USADA8()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rs = (int)((Opcode >> 8) & 0xf);
            int Rn = (int)((Opcode >> 12) & 0xf);
            int Rd = (int)((Opcode >> 16) & 0xf);

            byte Rm0 = (byte)Registers[Rm];
            byte Rm1 = (byte)(Registers[Rm] >> 8);
            byte Rm2 = (byte)(Registers[Rm] >> 16);
            byte Rm3 = (byte)(Registers[Rm] >> 24);

            byte Rs0 = (byte)Registers[Rs];
            byte Rs1 = (byte)(Registers[Rs] >> 8);
            byte Rs2 = (byte)(Registers[Rs] >> 16);
            byte Rs3 = (byte)(Registers[Rs] >> 24);

            int Result0 = Math.Abs(Rm0 - Rs0);
            int Result1 = Math.Abs(Rm1 - Rs1);
            int Result2 = Math.Abs(Rm2 - Rs2);
            int Result3 = Math.Abs(Rm3 - Rs3);

            Registers[Rd] = (uint)Result0;
            Registers[Rd] += (uint)Result1;
            Registers[Rd] += (uint)Result2;
            Registers[Rd] += (uint)Result3;
            Registers[Rd] += Registers[Rn];
        }

        /*
         * Utils
         */

        /// <summary>
        ///     Adds two 32-bits values together (Left and Right), and then adds the Carry value.
        ///     It automatically set the Overflow, Carry, Zero and Negative flags (if S is true).
        /// </summary>
        /// <param name="Left">The first value to be added</param>
        /// <param name="Right">The second value to be added</param>
        /// <param name="C">True if the Carry flag should be used, false otherwise</param>
        /// <param name="S">True if the flags should be updated, false otherwise</param>
        /// <returns>The result of the addition</returns>
        private uint Add(uint Left, uint Right, bool C, bool S)
        {
            uint Carry = C ? (Registers.CPSR >> 29) & 1 : 0;
            ulong Result64 = (ulong)Left + Right + Carry;
            uint Result = (uint)Result64;

            if (S)
            {
                Registers.SetFlag(ARMFlag.Carry, Result64 > 0xffffffff);
                Registers.SetFlag(ARMFlag.Overflow, AddOverflow(Left, Right, Result));
                SetZNFlags(Result);
            }

            return Result;
        }

        //Overflow checks

        /// <summary>
        ///     Checks whenever a 32-bits addition overflowed.
        /// </summary>
        /// <param name="Left">First value of the addition</param>
        /// <param name="Right">Second value of the addition</param>
        /// <param name="Result">Result of the addition</param>
        /// <returns>True if the result overflowed, false otherwise</returns>
        private bool AddOverflow(uint Left, uint Right, uint Result)
        {
            return (~(Left ^ Right) & (Left ^ Result) & 0x80000000) != 0;
        }

        /// <summary>
        ///     Checks whenever a 16-bits addition overflowed.
        /// </summary>
        /// <param name="Left">First value of the addition</param>
        /// <param name="Right">Second value of the addition</param>
        /// <param name="Result">Result of the addition</param>
        /// <returns>True if the result overflowed, false otherwise</returns>
        private bool AddOverflow16(ushort Left, ushort Right, uint Result)
        {
            return (~(Left ^ Right) & (Left ^ Result) & 0x8000) != 0;
        }

        /// <summary>
        ///     Checks whenever a 8-bits addition overflowed.
        /// </summary>
        /// <param name="Left">First value of the addition</param>
        /// <param name="Right">Second value of the addition</param>
        /// <param name="Result">Result of the addition</param>
        /// <returns>True if the result overflowed, false otherwise</returns>
        private bool AddOverflow8(byte Left, byte Right, uint Result)
        {
            return (~(Left ^ Right) & (Left ^ Result) & 0x80) != 0;
        }

        //Signed

        /// <summary>
        ///     Adds two 32-bits values, and saturates the result to the signed 32-bits range.
        ///     It also automatically sets the Saturation (Q) flag if a saturation occurs.
        /// </summary>
        /// <param name="Left">First value of the addition</param>
        /// <param name="Right">Second value of the addition</param>
        /// <returns>The result saturated into the 32-bits range</returns>
        private uint AddSignedSaturate(uint Left, uint Right)
        {
            uint Result = Left + Right;
            if (AddOverflow(Left, Right, Result))
            {
                Result = (Result & 0x80000000) != 0 ? 0x7fffffff : 0x80000000;
                Registers.SetFlag(ARMFlag.Saturation, true);
            }

            return Result;
        }

        /// <summary>
        ///     Adds two 16-bits values, and saturates the result to the signed 16-bits range.
        ///     It also automatically sets the Saturation (Q) flag if a saturation occurs.
        /// </summary>
        /// <param name="Left">First value of the addition</param>
        /// <param name="Right">Second value of the addition</param>
        /// <returns>The result saturated into the 16-bits range</returns>
        private uint AddSignedSaturate16(ushort Left, ushort Right)
        {
            uint Result = (uint)(Left + Right);
            if (AddOverflow16(Left, Right, Result))
            {
                Result = (uint)((Result & 0x8000) != 0 ? 0x7fff : 0x8000);
                Registers.SetFlag(ARMFlag.Saturation, true);
            }

            return Result;
        }

        /// <summary>
        ///     Adds two 8-bits values, and saturates the result to the signed 8-bits range.
        ///     It also automatically sets the Saturation (Q) flag if a saturation occurs.
        /// </summary>
        /// <param name="Left">First value of the addition</param>
        /// <param name="Right">Second value of the addition</param>
        /// <returns>The result saturated into the 8-bits range</returns>
        private uint AddSignedSaturate8(byte Left, byte Right)
        {
            uint Result = (uint)(Left + Right);
            if (AddOverflow8(Left, Right, Result))
            {
                Result = (uint)((Result & 0x80) != 0 ? 0x7f : 0x80);
                Registers.SetFlag(ARMFlag.Saturation, true);
            }

            return Result;
        }

        //Unsigned

        /// <summary>
        ///     Adds two 32-bits values, and saturates the result to the unsigned 32-bits range.
        ///     It also automatically sets the Saturation (Q) flag if a saturation occurs.
        /// </summary>
        /// <param name="Left">First value of the addition</param>
        /// <param name="Right">Second value of the addition</param>
        /// <returns>The result saturated into the 32-bits range</returns>
        private uint AddUnsignedSaturate(uint Left, uint Right)
        {
            ulong Result = (ulong)Left + Right;
            if (Result > 0xffffffff)
            {
                Result = 0xffffffff;
                Registers.SetFlag(ARMFlag.Saturation, true);
            }

            return (uint)Result;
        }

        /// <summary>
        ///     Adds two 16-bits values, and saturates the result to the unsigned 16-bits range.
        ///     It also automatically sets the Saturation (Q) flag if a saturation occurs.
        /// </summary>
        /// <param name="Left">First value of the addition</param>
        /// <param name="Right">Second value of the addition</param>
        /// <returns>The result saturated into the 16-bits range</returns>
        private uint AddUnsignedSaturate16(ushort Left, ushort Right)
        {
            uint Result = (uint)Left + Right;
            if (Result > 0xffff)
            {
                Result = 0xffff;
                Registers.SetFlag(ARMFlag.Saturation, true);
            }

            return Result;
        }

        /// <summary>
        ///     Adds two 8-bits values, and saturates the result to the unsigned 8-bits range.
        ///     It also automatically sets the Saturation (Q) flag if a saturation occurs.
        /// </summary>
        /// <param name="Left">First value of the addition</param>
        /// <param name="Right">Second value of the addition</param>
        /// <returns>The result saturated into the 8-bits range</returns>
        private uint AddUnsignedSaturate8(byte Left, byte Right)
        {
            uint Result = (uint)Left + Right;
            if (Result > 0xff)
            {
                Result = 0xff;
                Registers.SetFlag(ARMFlag.Saturation, true);
            }

            return Result;
        }
    }
}
