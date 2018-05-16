namespace CTREmulator.ARM
{
    public partial class ARMInterpreter
    {
        /// <summary>
        ///     Decodes the Operand value from the Shifter Operand.
        /// </summary>
        /// <param name="S">True if the Carry flag should be updated with the shifting operations, false otherwise</param>
        /// <returns>The decoded Operand value</returns>
        private uint ARM_GetShifterOperand(bool S)
        {
            bool I = IsOpcodeBitSet(25);
            uint ShifterOperand = Opcode & 0xfff;

            if (I) //Immediate
            {
                int Rotate = (int)((ShifterOperand >> 8) << 1);
                uint Operand = ROR(ShifterOperand & 0xff, Rotate);
                if (S && Rotate > 0) Registers.SetFlag(ARMFlag.Carry, (Operand & 0x80000000) != 0);
                return Operand;
            }
            else //Register
            {
                uint Operand = 0;
                bool Carry = false;
                int Rm = (int)(ShifterOperand & 0xf);
                uint Shifter = (Opcode >> 5) & 3;

                int Shift;
                bool Register = IsOpcodeBitSet(4);
                if (Register)
                {
                    int Rs = (int)((ShifterOperand >> 8) & 0xf);
                    Shift = (int)(Registers[Rs] & 0xff);
                }
                else
                {
                    Shift = (int)((ShifterOperand >> 7) & 0x1f);
                    if (Shift == 0 && (Shifter == 1 || Shifter == 2)) Shift = 32;
                }

                if (Shift > 0 && Shift <= 32)
                {
                    switch (Shifter)
                    {
                        case 0: //LSL
                            Operand = Shift < 32 ? Registers[Rm] << Shift : 0;
                            Carry = (Registers[Rm] & (1 << (32 - Shift))) != 0;
                            break;

                        case 1: //LSR
                            Operand = Shift < 32 ? Registers[Rm] >> Shift : 0;
                            Carry = (Registers[Rm] & (1 << (Shift - 1))) != 0;
                            break;

                        case 2: //ASR
                            int RmValue = (int)Registers[Rm];
                            Operand = (uint)((long)RmValue >> Shift);
                            Carry = (RmValue & (1 << (Shift - 1))) != 0;
                            break;

                        case 3: //ROR
                            if (Register) Shift &= 0x1f;
                            Operand = ROR(Registers[Rm], Shift);
                            if (Shift > 0)
                                Carry = (Registers[Rm] & (1 << (Shift - 1))) != 0;
                            else
                                Carry = (Registers[Rm] & 0x80000000) != 0;
                            break;
                    }
                }
                else
                {
                    if (Shift == 0)
                    {
                        Operand = Registers[Rm];
                        Carry = Registers.IsFlagSet(ARMFlag.Carry);

                        if (!Register && Shifter == 3) //RRX
                        {
                            Operand = (Operand >> 1) | (Carry ? 0x80000000 : 0);
                            Carry = (Registers[Rm] & 1) != 0;
                        }
                    }
                    else if (Shifter > 1)
                    {
                        Carry = (Registers[Rm] & 0x80000000) != 0;
                        Operand = Carry ? 0xffffffff : 0;
                    }
                }

                if (S) Registers.SetFlag(ARMFlag.Carry, Carry);
                return Operand;
            }
        }
    }
}
