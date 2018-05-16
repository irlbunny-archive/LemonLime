namespace CTREmulator.ARM
{
    public partial class ARMInterpreter
    {
        /// <summary>
        ///     Breakpoint.
        /// </summary>
        private void Thumb_BKPT()
        {
            uint SPSR = Registers.CPSR;
            Registers.Mode = ARMMode.Abort;
            Registers[14] = Registers[15] - 2;
            Registers.SPSR = SPSR;
            Registers.SetFlag(ARMFlag.Thumb, false);
            Registers.SetFlag(ARMFlag.IRQDisable, true);
            Registers.SetFlag(ARMFlag.AbortDisable, true);
            Registers.SetFlag(ARMFlag.Endianness, false);

            Registers[15] = HighVectors ? 0xffff000c : 0xc;

            if (OnBreakpoint != null)
            {
                ushort Code = (ushort)(Opcode & 0xff);
                OnBreakpoint(this, new BreakpointEventArgs(Code));
            }
        }

        /// <summary>
        ///     Change Processor State.
        /// </summary>
        private void Thumb_CPS()
        {
            if (Registers.Mode != ARMMode.User)
            {
                bool Value = IsOpcodeBitSet(4);
                if (IsOpcodeBitSet(0)) Registers.SetFlag(ARMFlag.FIQDisable, Value);
                if (IsOpcodeBitSet(1)) Registers.SetFlag(ARMFlag.IRQDisable, Value);
                if (IsOpcodeBitSet(2)) Registers.SetFlag(ARMFlag.AbortDisable, Value);               
            }
        }

        /// <summary>
        ///     Software Interrupt.
        /// </summary>
        private void Thumb_SWI()
        {
            uint SPSR = Registers.CPSR;
            Registers.Mode = ARMMode.Supervisor;
            Registers[14] = Registers[15] - 2;
            Registers.SPSR = SPSR;
            Registers.SetFlag(ARMFlag.Thumb, false);
            Registers.SetFlag(ARMFlag.IRQDisable, true);
            Registers.SetFlag(ARMFlag.Endianness, false);

            Registers[15] = HighVectors ? 0xffff0008 : 8;

            if (OnSoftwareInterrupt != null)
            {
                uint Code = Opcode & 0xff;
                OnSoftwareInterrupt(this, new SoftwareInterruptEventArgs(Code));
            }
        }
    }
}
