namespace LemonLime.ARM
{
    public partial class Interpreter
    {
        /// <summary>
        ///     Gets the Address used for the general Load and Store instructions.
        /// </summary>
        /// <returns>The Base Address to Load or Store</returns>
        private uint ARM_GetLoadAndStoreAddress()
        {
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool W = IsOpcodeBitSet(21);
            bool U = IsOpcodeBitSet(23);
            bool P = IsOpcodeBitSet(24);
            bool I = IsOpcodeBitSet(25);

            ARMMode OldMode = Registers.Mode;
            if (!P && W) Registers.Mode = ARMMode.User;
            uint Offset = 0;
            if (I) //Register
            {
                int Rm = (int)(Opcode & 0xf);
                uint Shift = (Opcode >> 5) & 3;
                int ShiftImmediate = (int)((Opcode >> 7) & 0x1f);

                switch (Shift)
                {
                    case 0: Offset = Registers[Rm] << ShiftImmediate; break; //LSL
                    case 1: Offset = ShiftImmediate != 0 ? Registers[Rm] >> ShiftImmediate : 0; break; //LSR

                    case 2: //ASR
                        if (ShiftImmediate == 0)
                            Offset = (Registers[Rm] & 0x80000000) != 0 ? 0xffffffff : 0;
                        else
                            Offset = Registers[Rm] >> ShiftImmediate;
                        break;

                    case 3: //ROR
                        if (ShiftImmediate == 0)
                            Offset = (Registers[Rm] >> 1) | (Registers.IsFlagSet(ARMFlag.Carry) ? 0x80000000 : 0);
                        else
                            Offset = ROR(Registers[Rm], ShiftImmediate);
                        break;
                }
            }
            else //Immediate
                Offset = Opcode & 0xfff;

            uint Address = 0;
            if (P)
            {
                if (W) //Pre-Indexed
                {
                    if (U)
                        Registers[Rn] += Offset;
                    else
                        Registers[Rn] -= Offset;

                    Address = Registers[Rn];
                }
                else //Offset
                    Address = U ? Registers[Rn] + Offset : Registers[Rn] - Offset;
            }
            else
            {
                if (!W) //Post-Indexed
                {
                    Address = Registers[Rn];

                    if (U)
                        Registers[Rn] += Offset;
                    else
                        Registers[Rn] -= Offset;
                }
            }

            Registers.Mode = OldMode;
            return Address;
        }

        /// <summary>
        ///     Gets the Address used to Load or Store on the Coprocessor-related instructions.
        /// </summary>
        /// <returns>The Base Address to Load or Store</returns>
        private uint ARM_GetLoadAndStoreCPAddress()
        {
            uint Offset = (Opcode & 0xff) << 2;
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool W = IsOpcodeBitSet(21);
            bool U = IsOpcodeBitSet(23);
            bool P = IsOpcodeBitSet(24);

            uint Address;
            if (P)
            {
                if (W) //Pre-Indexed
                {
                    if (U)
                        Registers[Rn] += Offset;
                    else
                        Registers[Rn] -= Offset;

                    Address = Registers[Rn];
                }
                else //Offset
                    Address = U ? Registers[Rn] + Offset : Registers[Rn] - Offset;
            }
            else
            {
                if (W) //Post-Indexed
                {
                    Address = Registers[Rn];

                    if (U)
                        Registers[Rn] += Offset;
                    else
                        Registers[Rn] -= Offset;
                }
                else //Unindexed
                    Address = Registers[Rn];
            }

            return Address;
        }

        /// <summary>
        ///     Gets the Address used on Load or Store instruction that uses values bigger or smaller than 32-bits.
        /// </summary>
        /// <returns>The Base Address to Load or Store</returns>
        private uint ARM_GetLoadAndStoreNonWordAddress()
        {
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool W = IsOpcodeBitSet(21);
            bool I = IsOpcodeBitSet(22);
            bool U = IsOpcodeBitSet(23);
            bool P = IsOpcodeBitSet(24);

            uint Offset;
            if (I) //Immediate 
                Offset = (Opcode & 0xf) | ((Opcode >> 4) & 0xf0);
            else //Register
                Offset = Registers[(int)(Opcode & 0xf)];

            uint Address = 0;
            if (P)
            {
                if (W) //Pre-Indexed
                {
                    if (U)
                        Registers[Rn] += Offset;
                    else
                        Registers[Rn] -= Offset;

                    Address = Registers[Rn];
                }
                else //Offset
                    Address = U ? Registers[Rn] + Offset : Registers[Rn] - Offset;
            }
            else
            {
                if (!W) //Post-Indexed
                {
                    Address = Registers[Rn];

                    if (U)
                        Registers[Rn] += Offset;
                    else
                        Registers[Rn] -= Offset;
                }
            }

            return Address;
        }

        /// <summary>
        ///     Gets the Address used on Load or Store Multiple instructions.
        /// </summary>
        /// <returns>The Base Address to Load or Store</returns>
        private uint ARM_GetLoadAndStoreMultipleAddress()
        {
            ushort RegisterList = (ushort)(Opcode & 0xffff);
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool W = IsOpcodeBitSet(21);
            bool U = IsOpcodeBitSet(23);
            bool P = IsOpcodeBitSet(24);

            uint Count = 0;
            for (int Index = 0; Index < 16; Index++)
            {
                if ((RegisterList & (1 << Index)) != 0) Count += 4;
            }

            uint Address;
            if (U)
                Address = Registers[Rn] + (uint)(P ? 4 : 0);
            else
                Address = Registers[Rn] - Count + (uint)(P ? 0 : 4);

            if (W)
            {
                if (U)
                    Registers[Rn] += Count;
                else
                    Registers[Rn] -= Count;
            }

            return Address;
        }

        /// <summary>
        ///     Gets the Address used by the SRS (Store Return State) instruction.
        /// </summary>
        /// <returns>The Base Address to Store</returns>
        private uint ARM_GetSRSAddress()
        {
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool W = IsOpcodeBitSet(21);
            bool U = IsOpcodeBitSet(23);
            bool P = IsOpcodeBitSet(24);

            uint Address;
            if (U)
                Address = Registers[Rn] + (uint)(P ? 4 : 0);
            else
                Address = Registers[Rn] - 8 + (uint)(P ? 0 : 4);

            if (W)
            {
                if (U)
                    Registers[Rn] += 8;
                else
                    Registers[Rn] -= 8;
            }

            return Address;
        }
    }
}
