namespace CTREmulator.ARM
{
    public partial class Interpreter
    {
        /// <summary>
        ///     Store Return State.
        /// </summary>
        private void ARM_SRS()
        {
            uint Address = ARM_GetSRSAddress();

            ARMMode OldMode = Registers.Mode;
            Registers.Mode = (ARMMode)(Opcode & 0x1f);

            WriteUInt32E(Address, Registers[14]);
            WriteUInt32E(Address + 4, Registers.SPSR);

            Registers.Mode = OldMode;
        }

        /// <summary>
        ///     Store Multiple.
        /// </summary>
        private void ARM_STM()
        {
            ushort RegisterList = (ushort)(Opcode & 0xffff);
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool W = IsOpcodeBitSet(21);
            bool S = IsOpcodeBitSet(22);
            bool U = IsOpcodeBitSet(23);
            bool P = IsOpcodeBitSet(24);

            uint Address;
            if (U)
                Address = Registers[Rn] + (uint)(P ? 4 : 0);
            else
            {
                Address = Registers[Rn] + (uint)(P ? 0 : 4);
                for (int Index = 0; Index < 16; Index++)
                {
                    if ((RegisterList & (1 << Index)) != 0) Address -= 4;
                }
            }

            uint Count = 0;
            ARMMode OldMode = Registers.Mode;
            if (S) Registers.Mode = ARMMode.User;
            for (int Index = 0; Index < 16; Index++)
            {
                if ((RegisterList & (1 << Index)) != 0)
                {
                    WriteUInt32E(Address + Count, Registers[Index]);
                    Count += 4;
                }
            }
            Registers.Mode = OldMode;

            if (W)
            {
                if (U)
                    Registers[Rn] += Count;
                else
                    Registers[Rn] -= Count;
            }
        }

        /// <summary>
        ///     Store Register.
        /// </summary>
        private void ARM_STR()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);

            uint Address = ARM_GetLoadAndStoreAddress();
            uint Value = ReadUInt32E(Address);
            WriteUInt32E(Address, Registers[Rd]);
        }

        /// <summary>
        ///     Store Register Byte.
        /// </summary>
        private void ARM_STRB()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);

            uint Address = ARM_GetLoadAndStoreAddress();
            Bus.WriteUInt8(Address, (byte)Registers[Rd]);
        }

        /// <summary>
        ///     Store Register Doubleword.
        /// </summary>
        private void ARM_STRD()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);

            if (Rd != 15)
            {
                uint Address = ARM_GetLoadAndStoreNonWordAddress();
                WriteUInt32E(Address, Registers[Rd]);
                WriteUInt32E(Address + 4, Registers[Rd + 1]);
            }
        }

        /// <summary>
        ///     Store Register Exclusive.
        /// </summary>
        private void ARM_STREX()
        {
            int Rm = (int)(Opcode & 0xf);
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            uint Address = Registers[Rn];
            if (ExclusiveEnabled && ExclusiveTag == (Address & ERGMask))
            {
                WriteUInt32E(Address, Registers[Rm]);
                ExclusiveEnabled = false;
                Registers[Rd] = 0;
            }
            else
                Registers[Rd] = 1;
        }

        /// <summary>
        ///     Store Register Halfword.
        /// </summary>
        private void ARM_STRH()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);

            uint Address = ARM_GetLoadAndStoreNonWordAddress();
            WriteUInt16E(Address, (ushort)Registers[Rd]);
        }
    }
}
