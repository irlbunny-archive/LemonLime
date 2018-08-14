namespace LemonLime.ARM
{
    public partial class Interpreter
    {
        /// <summary>
        ///     Branch (1).
        /// </summary>
        private void Thumb_B()
        {
            sbyte Immediate = (sbyte)(Opcode & 0xff);

            ARMCondition Condition = (ARMCondition)((Opcode >> 8) & 0xf);
            if (IsConditionMet(Condition)) Registers[15] = (uint)(Registers[15] + (Immediate << 1));
        }

        /// <summary>
        ///     Branch (2).
        /// </summary>
        private void Thumb_B_2()
        {
            int Immediate = SignExtend12((Opcode & 0x7ff) << 1);

            Registers[15] = (uint)(Registers[15] + Immediate);
        }

        /// <summary>
        ///     Branch with Link and Exchange (1).
        /// </summary>
        private void Thumb_BLX()
        {
            uint Immediate = Opcode & 0x7ff;
            uint H = (Opcode >> 11) & 3;

            if (H > 0)
            {
                if (H != 2)
                {
                    uint LR = (Registers[15] - 2) | 1;
                    Registers[15] = Registers[14] + (Immediate << 1);
                    Registers[14] = LR;

                    if (H == 1)
                    {
                        Registers[15] &= 0xfffffffc;
                        Registers.SetFlag(ARMFlag.Thumb, false);
                    }
                }
                else
                    Registers[14] = (uint)(Registers[15] + (SignExtend11(Immediate) << 12));
            }
        }

        /// <summary>
        ///     Branch with Link and Exchange (2).
        /// </summary>
        private void Thumb_BLX_2()
        {
            int Rm = (int)((Opcode >> 3) & 7);

            Registers[14] = (Registers[15] - 2) | 1;
            Registers[15] = Registers[Rm] & 0xfffffffe;
            Registers.SetFlag(ARMFlag.Thumb, (Registers[Rm] & 1) != 0);
        }

        /// <summary>
        ///     Branch and Exchange.
        /// </summary>
        private void Thumb_BX()
        {
            int Rm = (int)((Opcode >> 3) & 0xf);

            Registers[15] = Registers[Rm] & 0xfffffffe;
            Registers.SetFlag(ARMFlag.Thumb, (Registers[Rm] & 1) != 0);
        }
    }
}
