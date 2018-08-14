using System;

namespace LemonLime.ARM
{
    public partial class Interpreter
    {
        /// <summary>
        ///     Flags used on the registers CPSR and SPSR.
        /// </summary>
        public enum ARMFlag : uint
        {
            Thumb = 0x20, //T
            FIQDisable = 0x40, //F
            IRQDisable = 0x80, //I
            AbortDisable = 0x100, //A
            Endianness = 0x200, //E
            Saturation = 0x8000000, //Q
            Overflow = 0x10000000, //V
            Carry = 0x20000000, //C
            Zero = 0x40000000, //Z
            Negative = 0x80000000 //N
        }

        /// <summary>
        ///     ARM CPU executions modes.
        /// </summary>
        public enum ARMMode
        {
            User = 0x10,
            FIQ = 0x11,
            IRQ = 0X12,
            Supervisor = 0x13,
            Abort = 0x17,
            Undefined = 0x1b,
            System = 0x1f
        }

        /// <summary>
        ///     Represents all the 37 ARM Registers.
        /// </summary>
        public class ARMRegisters
        {
            /*
             * General purpose registers
             * R13 = Stack pointer
             * R14 = Link register
             * R15 = Program counter
             */
            private uint[] R = new uint[16];

            private uint R8_FIQ;
            private uint R9_FIQ;
            private uint R10_FIQ;
            private uint R11_FIQ;
            private uint R12_FIQ;
            private uint R13_FIQ;
            private uint R14_FIQ;

            private uint R13_IRQ;
            private uint R14_IRQ;

            private uint R13_SVC;
            private uint R14_SVC;

            private uint R13_ABT;
            private uint R14_ABT;

            private uint R13_UNDEF;
            private uint R14_UNDEF;

            /*
             * Status registers
             */

            /// <summary>
            ///     Current Processor Status Register.
            /// </summary>
            public uint CPSR;
            private uint SPSR_FIQ;
            private uint SPSR_IRQ;
            private uint SPSR_SVC;
            private uint SPSR_ABT;
            private uint SPSR_UNDEF;

            /// <summary>
            ///     This flag is set to true every time the value of R15 is changed.
            ///     It needs to be set to false manually. Useful for handling the 3-stage pipeline.
            /// </summary>
            public bool PCChanged;

            /// <summary>
            ///     Gets or sets the value of the SPSR register of the current mode.
            /// </summary>
            public uint SPSR
            {
                get
                {
                    switch (Mode)
                    {
                        case ARMMode.FIQ: return SPSR_FIQ;
                        case ARMMode.IRQ: return SPSR_IRQ;
                        case ARMMode.Supervisor: return SPSR_SVC;
                        case ARMMode.Abort: return SPSR_ABT;
                        case ARMMode.Undefined: return SPSR_UNDEF;
                    }

                    return 0;
                }
                set
                {
                    switch (Mode)
                    {
                        case ARMMode.FIQ: SPSR_FIQ = value; break;
                        case ARMMode.IRQ: SPSR_IRQ = value; break;
                        case ARMMode.Supervisor: SPSR_SVC = value; break;
                        case ARMMode.Abort: SPSR_ABT = value; break;
                        case ARMMode.Undefined: SPSR_UNDEF = value; break;
                    }
                }
            }

            /// <summary>
            ///     Gets or sets the current CPU execution mode.
            /// </summary>
            public ARMMode Mode
            {
                get
                {
                    return (ARMMode)(CPSR & 0x1f);
                }
                set
                {
                    CPSR = (uint)(CPSR & ~0x1f) | (uint)value;
                }
            }


            /// <summary>
            ///     Gets or sets the Greater than or Equal bits on the CPSR Register.
            /// </summary>
            public byte GE
            {
                get
                {
                    return (byte)((CPSR >> 16) & 0xf);
                }
                set
                {
                    CPSR = (CPSR & 0xfff0ffff) | (uint)(value << 16);
                }
            }

            /// <summary>
            ///     ARM General purpose registers, accessible by Index number.
            /// </summary>
            /// <param name="RegisterIndex">The Index number of the Register to work with</param>
            /// <returns>The value of the Register with the specified Index</returns>
            public uint this[int RegisterIndex]
            {
                get
                {
                    switch (Mode)
                    {
                        case ARMMode.User:
                        case ARMMode.System:
                            return R[RegisterIndex];

                        case ARMMode.FIQ:
                            if (RegisterIndex < 8 || RegisterIndex == 15)
                                return R[RegisterIndex];
                            else
                            {
                                switch (RegisterIndex)
                                {
                                    case 8: return R8_FIQ;
                                    case 9: return R9_FIQ;
                                    case 10: return R10_FIQ;
                                    case 11: return R11_FIQ;
                                    case 12: return R12_FIQ;
                                    case 13: return R13_FIQ;
                                    case 14: return R14_FIQ;
                                }
                            }
                            break;

                        case ARMMode.IRQ:
                            if (RegisterIndex < 13 || RegisterIndex == 15)
                                return R[RegisterIndex];
                            else
                            {
                                switch (RegisterIndex)
                                {
                                    case 13: return R13_IRQ;
                                    case 14: return R14_IRQ;
                                }
                            }
                            break;

                        case ARMMode.Supervisor:
                            if (RegisterIndex < 13 || RegisterIndex == 15)
                                return R[RegisterIndex];
                            else
                            {
                                switch (RegisterIndex)
                                {
                                    case 13: return R13_SVC;
                                    case 14: return R14_SVC;
                                }
                            }
                            break;

                        case ARMMode.Abort:
                            if (RegisterIndex < 13 || RegisterIndex == 15)
                                return R[RegisterIndex];
                            else
                            {
                                switch (RegisterIndex)
                                {
                                    case 13: return R13_ABT;
                                    case 14: return R14_ABT;
                                }
                            }
                            break;

                        case ARMMode.Undefined:
                            if (RegisterIndex < 13 || RegisterIndex == 15)
                                return R[RegisterIndex];
                            else
                            {
                                switch (RegisterIndex)
                                {
                                    case 13: return R13_UNDEF;
                                    case 14: return R14_UNDEF;
                                }
                            }
                            break;

                        default: throw new Exception("SharpARM: Invalid CPSR ARM execution Mode!");
                    }

                    return 0;
                }
                set
                {
                    switch (Mode)
                    {
                        case ARMMode.User:
                        case ARMMode.System:
                            R[RegisterIndex] = value;
                            break;

                        case ARMMode.FIQ:
                            if (RegisterIndex < 8 || RegisterIndex == 15)
                                R[RegisterIndex] = value;
                            else
                            {
                                switch (RegisterIndex)
                                {
                                    case 8: R8_FIQ = value; break;
                                    case 9: R9_FIQ = value; break;
                                    case 10: R10_FIQ = value; break;
                                    case 11: R11_FIQ = value; break;
                                    case 12: R12_FIQ = value; break;
                                    case 13: R13_FIQ = value; break;
                                    case 14: R14_FIQ = value; break;
                                }
                            }
                            break;

                        case ARMMode.IRQ:
                            if (RegisterIndex < 13 || RegisterIndex == 15)
                                R[RegisterIndex] = value;
                            else
                            {
                                switch (RegisterIndex)
                                {
                                    case 13: R13_IRQ = value; break;
                                    case 14: R14_IRQ = value; break;
                                }
                            }
                            break;

                        case ARMMode.Supervisor:
                            if (RegisterIndex < 13 || RegisterIndex == 15)
                                R[RegisterIndex] = value;
                            else
                            {
                                switch (RegisterIndex)
                                {
                                    case 13: R13_SVC = value; break;
                                    case 14: R14_SVC = value; break;
                                }
                            }
                            break;

                        case ARMMode.Abort:
                            if (RegisterIndex < 13 || RegisterIndex == 15)
                                R[RegisterIndex] = value;
                            else
                            {
                                switch (RegisterIndex)
                                {
                                    case 13: R13_ABT = value; break;
                                    case 14: R14_ABT = value; break;
                                }
                            }
                            break;

                        case ARMMode.Undefined:
                            if (RegisterIndex < 13 || RegisterIndex == 15)
                                R[RegisterIndex] = value;
                            else
                            {
                                switch (RegisterIndex)
                                {
                                    case 13: R13_UNDEF = value; break;
                                    case 14: R14_UNDEF = value; break;
                                }
                            }
                            break;

                        default: throw new Exception("SharpARM: Invalid CPSR ARM execution Mode!");
                    }

                    if (RegisterIndex == 15) PCChanged = true;
                }
            }

            /// <summary>
            ///     Checks if a Flag is set on the Status register.
            /// </summary>
            /// <param name="Flag">Flag that should be checked</param>
            /// <returns>True if the flag is set, false otherwise</returns>
            public bool IsFlagSet(ARMFlag Flag)
            {
                return (CPSR & (uint)Flag) != 0;
            }

            /// <summary>
            ///     Check if a Flag is cleared on the Status register.
            /// </summary>
            /// <param name="Flag">Flag that should be checked</param>
            /// <returns>True if the flag is cleared, false otherwise</returns>
            public bool IsFlagClear(ARMFlag Flag)
            {
                return (CPSR & (uint)Flag) == 0;
            }

            /// <summary>
            ///     Set the value of a given ARM Flag.
            /// </summary>
            /// <param name="Flag">The affected Flag</param>
            /// <param name="Value">The bit value that should be set (True = Set or False = Cleared)</param>
            public void SetFlag(ARMFlag Flag, bool Value)
            {
                if (Value) CPSR |= (uint)Flag; else CPSR &= ~(uint)Flag;
            }
        }
    }
}
