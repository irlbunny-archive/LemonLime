namespace CTREmulator.ARM
{
    public partial class Interpreter
    {
        /// <summary>
        ///     Load Multiple.
        /// </summary>
        private void ARM_LDM()
        {
            ushort RegisterList = (ushort)(Opcode & 0xffff);
            bool PC = (RegisterList & 0x8000) != 0;
            int Rn = (int)((Opcode >> 16) & 0xf);
            bool W = IsOpcodeBitSet(21);
            bool S = IsOpcodeBitSet(22);

            ARMMode OldMode = Registers.Mode;
            if (S && !PC) Registers.Mode = ARMMode.User;
            uint Address = ARM_GetLoadAndStoreMultipleAddress() & 0xfffffffc;
            for (int Index = 0; Index < 15; Index++)
            {
                if ((RegisterList & (1 << Index)) != 0)
                {
                    Registers[Index] = ReadUInt32E(Address);
                    Address += 4;
                }
            }
            Registers.Mode = OldMode;

            if (PC)
            {
                uint Value = ReadUInt32E(Address);
                Registers[15] = Value & 0xfffffffe;
                Registers.SetFlag(ARMFlag.Thumb, (Value & 1) != 0);
                if (S) Registers.CPSR = Registers.SPSR;
            }
        }

        /// <summary>
        ///     Load Register.
        /// </summary>
        private void ARM_LDR()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);

            uint Address = ARM_GetLoadAndStoreAddress();
            uint Value = ROR(ReadUInt32E(Address & 0xfffffffc), (int)(Address & 3) << 3);
            if (Rd == 15)
            {
                Registers[15] = Value & 0xfffffffe;
                Registers.SetFlag(ARMFlag.Thumb, (Value & 1) != 0);
            }
            else
                Registers[Rd] = Value;
        }

        /// <summary>
        ///     Load Register Byte.
        /// </summary>
        private void ARM_LDRB()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);

            uint Address = ARM_GetLoadAndStoreAddress();
            byte Value = Bus.ReadUInt8(Address);
            Registers[Rd] = Value;
        }

        /// <summary>
        ///     Load Register Doubleword.
        /// </summary>
        private void ARM_LDRD()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);

            if (Rd != 15)
            {
                uint Address = ARM_GetLoadAndStoreNonWordAddress();
                Registers[Rd] = ReadUInt32E(Address);
                Registers[Rd + 1] = ReadUInt32E(Address + 4);
            }
        }

        /// <summary>
        ///     Load Register Exclusive.
        /// </summary>
        private void ARM_LDREX()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);

            uint Address = Registers[Rn];
            uint Value = ReadUInt32E(Address);
            Registers[Rd] = Value;
            ExclusiveTag = Address & ERGMask;
            ExclusiveEnabled = true;
        }

        /// <summary>
        ///     Load Register Halfword.
        /// </summary>
        private void ARM_LDRH()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);

            uint Address = ARM_GetLoadAndStoreNonWordAddress();
            Registers[Rd] = ReadUInt16E(Address);
        }

        /// <summary>
        ///     Load Register Signed Byte.
        /// </summary>
        private void ARM_LDRSB()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);

            uint Address = ARM_GetLoadAndStoreNonWordAddress();
            sbyte Value = (sbyte)Bus.ReadUInt8(Address);
            Registers[Rd] = (uint)Value;
        }

        /// <summary>
        ///     Load Register Signed Halfword.
        /// </summary>
        private void ARM_LDRSH()
        {
            int Rd = (int)((Opcode >> 12) & 0xf);

            uint Address = ARM_GetLoadAndStoreNonWordAddress();
            short Value = (short)ReadUInt16E(Address);
            Registers[Rd] = (uint)Value;
        }
    }
}