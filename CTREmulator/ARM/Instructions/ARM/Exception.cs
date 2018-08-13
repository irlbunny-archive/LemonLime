using System;

namespace CTREmulator.ARM
{
    /// <summary>
    ///     Arguments returned by the BKPT instruction.
    /// </summary>
    public class BreakpointEventArgs : EventArgs
    {
        /// <summary>
        ///     Code set by the Program with the Break exception.
        /// </summary>
        public ushort BreakCode;

        /// <summary>
        ///     Creates a new Breakpoint event argument with the given Break Code.
        /// </summary>
        /// <param name="BreakCode">16-bits BKPT code set by the Program</param>
        public BreakpointEventArgs(ushort BreakCode)
        {
            this.BreakCode = BreakCode;
        }
    }

    /// <summary>
    ///     Arguments returned by the SWI instruction.
    /// </summary>
    public class SoftwareInterruptEventArgs : EventArgs
    {
        /// <summary>
        ///     Code set by the Program with the Software Interrupt.
        /// </summary>
        public uint InterruptCode;

        /// <summary>
        ///     Creates a new Software Interrupt event argument with the given Break Code.
        /// </summary>
        /// <param name="InterruptCode">24-bits SWI code set by the Program</param>
        public SoftwareInterruptEventArgs(uint InterruptCode)
        {
            this.InterruptCode = InterruptCode;
        }
    }

    public partial class Interpreter
    {
        /// <summary>
        ///     Breakpoint.
        /// </summary>
        private void ARM_BKPT()
        {
            uint SPSR = Registers.CPSR;
            Registers.Mode = ARMMode.Abort;
            Registers[14] = Registers[15] - 4;
            Registers.SPSR = SPSR;
            Registers.SetFlag(ARMFlag.Thumb, false);
            Registers.SetFlag(ARMFlag.IRQDisable, true);
            Registers.SetFlag(ARMFlag.AbortDisable, true);
            Registers.SetFlag(ARMFlag.Endianness, false);

            Registers[15] = HighVectors ? 0xffff000c : 0xc;

            if (OnBreakpoint != null)
            {
                ushort Code = (ushort)(((Opcode >> 4) & 0xfff0) | (Opcode & 0xf));
                OnBreakpoint(this, new BreakpointEventArgs(Code));
            }
        }

        /// <summary>
        ///     Change Processor State.
        /// </summary>
        private void ARM_CPS()
        {
            ARMMode Mode = (ARMMode)(Opcode & 0x1f);
            bool mmod = IsOpcodeBitSet(17);
            uint imod = (Opcode >> 18) & 3;

            if ((imod & 2) != 0)
            {
                bool Value = (imod & 1) != 0;
                if (IsOpcodeBitSet(6)) Registers.SetFlag(ARMFlag.FIQDisable, Value);
                if (IsOpcodeBitSet(7)) Registers.SetFlag(ARMFlag.IRQDisable, Value);
                if (IsOpcodeBitSet(8)) Registers.SetFlag(ARMFlag.AbortDisable, Value);
            }
            else
                if (mmod) Registers.Mode = Mode;
        }

        /// <summary>
        ///     Return from Exception.
        /// </summary>
        private void ARM_RFE()
        {
            uint Address = ARM_GetLoadAndStoreMultipleAddress();

            Registers[15] = ReadUInt32(Address);
            if (Registers.Mode != ARMMode.User) Registers.CPSR = ReadUInt32(Address + 4);
        }

        /// <summary>
        ///     Software Interrupt.
        /// </summary>
        private void ARM_SWI()
        {
            uint SPSR = Registers.CPSR;
            Registers.Mode = ARMMode.Supervisor;
            Registers[14] = Registers[15] - 4;
            Registers.SPSR = SPSR;
            Registers.SetFlag(ARMFlag.Thumb, false);
            Registers.SetFlag(ARMFlag.IRQDisable, true);
            Registers.SetFlag(ARMFlag.Endianness, false);

            Registers[15] = HighVectors ? 0xffff0008 : 8;

            if (OnSoftwareInterrupt != null)
            {
                uint Code = Opcode & 0xffffff;
                OnSoftwareInterrupt(this, new SoftwareInterruptEventArgs(Code));
            }
        }
    }
}
