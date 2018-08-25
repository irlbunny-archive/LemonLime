using System;

namespace LemonLime.ARM
{
    /// <summary>
    ///     ARM CPU execution core.
    /// </summary>
    public partial class Interpreter
    {
        private IBus Bus;
        private Coprocessor15 CP15;
        public ARMRegisters Registers;

        private bool HighVectors;
        private uint Opcode;
        private uint NextOpcode;

        public bool Debug = false;

        /// <summary>
        ///     Creates a new ARM CPU emulator.
        /// </summary>
        /// <param name="Bus">Bus used to read and write data</param>
        /// <param name="HighVectors">True if the Vector Table starts at 0xFFFF0000 instead of 0x0, false otherwise</param>
        public Interpreter(IBus Bus, bool HighVectors = false)
        {
            this.Bus = Bus;
            this.HighVectors = HighVectors;
            Reset();
        }

        /// <summary>
        ///     Occurs when a Breakpoint is triggered on the Program.
        /// </summary>
        public event EventHandler<BreakpointEventArgs> OnBreakpoint;

        /// <summary>
        ///     Occurs when a Software Interrupt is triggered on the Program.
        /// </summary>
        public event EventHandler<SoftwareInterruptEventArgs> OnSoftwareInterrupt;

        /// <summary>
        ///     Whether to do a Hardware Interrupt or not
        /// </summary>
        public bool IRQ = false;

        /// <summary>
        ///     Resets the CPU, and sets the Registers to the initial state.
        /// </summary>
        public void Reset()
        {
            CP15 = new Coprocessor15();
            Registers = new ARMRegisters();
            Registers.SetFlag(ARMFlag.FIQDisable, true);
            Registers.SetFlag(ARMFlag.IRQDisable, true);
            Registers.SetFlag(ARMFlag.AbortDisable, true);
            Registers.SetFlag(ARMFlag.Zero, true);
            Registers.Mode = ARMMode.Supervisor;
            Registers[15] = HighVectors ? 0xffff0000 : 0;

            Opcode = 0;
            ReloadPipeline();
        }

        /// <summary>
        ///     Reloads the cached Opcode on the 3-stage pipeline.
        ///     Call this if you change the value of PC or the contents of the RAM where PC is pointing.
        /// </summary>
        public void ReloadPipeline()
        {
            if (Registers.IsFlagSet(ARMFlag.Thumb))
            {
                NextOpcode = ReadUInt16(Registers[15]);
                Registers[15] += 2;
            }
            else
            {
                NextOpcode = ReadUInt32(Registers[15]);
                Registers[15] += 4;
            }
        }

        /// <summary>
        ///     Executes a single instruction.
        /// </summary>
        public void Execute()
        {
            if (IRQ)
            {
                uint SPSR = Registers.CPSR;
                Registers.Mode = ARMMode.IRQ;
                Registers[14] = Registers[15] - (uint)(Registers.IsFlagSet(ARMFlag.Thumb) ? 0 : 4);
                Registers.SPSR = SPSR;
                Registers.SetFlag(ARMFlag.Thumb, false);
                Registers.SetFlag(ARMFlag.IRQDisable, true);
                Registers.SetFlag(ARMFlag.Endianness, false);

                Registers[15] = HighVectors ? 0xffff0000 + 24 : 24;

                IRQ = false;
            }

            uint TruePC = Registers[15] - 4;

            if (Registers.IsFlagSet(ARMFlag.Thumb))
            {
                Opcode = NextOpcode;
                NextOpcode = ReadUInt16(Registers[15]);
                Registers[15] += 2;
                Registers.PCChanged = false;

                switch ((Opcode >> 13) & 7)
                {
                    case 0:
                        switch ((Opcode >> 11) & 3)
                        {
                            case 0: Thumb_LSL(); break;
                            case 1: Thumb_LSR(); break;
                            case 2: Thumb_ASR(); break;
                            case 3:
                                if (IsOpcodeBitSet(9))
                                {
                                    if (IsOpcodeBitSet(10))
                                        Thumb_SUB();
                                    else
                                        Thumb_SUB_3();
                                }
                                else
                                {
                                    if (IsOpcodeBitSet(10))
                                        Thumb_ADD();
                                    else
                                        Thumb_ADD_3();
                                }
                                break;
                        }
                        break;

                    case 1:
                        switch ((Opcode >> 11) & 3)
                        {
                            case 0: Thumb_MOV(); break;
                            case 1: Thumb_CMP(); break;
                            case 2: Thumb_ADD_2(); break;
                            case 3: Thumb_SUB_2(); break;
                        }
                        break;

                    case 2:
                        if (IsOpcodeBitSet(12))
                        {
                            switch ((Opcode >> 9) & 7)
                            {
                                case 0: Thumb_STR_2(); break;
                                case 1: Thumb_STRH_2(); break;
                                case 2: Thumb_STRB_2(); break;
                                case 3: Thumb_LDRSB(); break;
                                case 4: Thumb_LDR_2(); break;
                                case 5: Thumb_LDRH_2(); break;
                                case 6: Thumb_LDRB_2(); break;
                                case 7: Thumb_LDRSH(); break;
                            }
                        }
                        else
                        {
                            if (IsOpcodeBitSet(11))
                                Thumb_LDR_3();
                            else
                            {
                                if (IsOpcodeBitSet(10))
                                {
                                    switch ((Opcode >> 8) & 3)
                                    {
                                        case 0: Thumb_ADD_4(); break;
                                        case 1: Thumb_CMP_3(); break;
                                        case 2: Thumb_CPY(); break;
                                        case 3:
                                            if (IsOpcodeBitSet(7))
                                                Thumb_BLX_2();
                                            else
                                                Thumb_BX();
                                            break;
                                    }
                                }
                                else //Data Processing
                                {
                                    switch ((Opcode >> 6) & 0xf)
                                    {
                                        case 0: Thumb_AND(); break;
                                        case 1: Thumb_EOR(); break;
                                        case 2: Thumb_LSL_2(); break;
                                        case 3: Thumb_LSR_2(); break;
                                        case 4: Thumb_ASR_2(); break;
                                        case 5: Thumb_ADC(); break;
                                        case 6: Thumb_SBC(); break;
                                        case 7: Thumb_ROR(); break;
                                        case 8: Thumb_TST(); break;
                                        case 9: Thumb_NEG(); break;
                                        case 0xa: Thumb_CMP_2(); break;
                                        case 0xb: Thumb_CMN(); break;
                                        case 0xc: Thumb_ORR(); break;
                                        case 0xd: Thumb_MUL(); break;
                                        case 0xe: Thumb_BIC(); break;
                                        case 0xf: Thumb_MVN(); break;
                                    }
                                }
                            }
                        }
                        break;

                    case 3:
                        if (IsOpcodeBitSet(11))
                        {
                            if (IsOpcodeBitSet(12))
                                Thumb_LDRB();
                            else
                                Thumb_LDR();
                        }
                        else
                        {
                            if (IsOpcodeBitSet(12))
                                Thumb_STRB();
                            else
                                Thumb_STR();
                        }
                        break;

                    case 4:
                        if (IsOpcodeBitSet(12))
                        {
                            if (IsOpcodeBitSet(11))
                                Thumb_LDR_4();
                            else
                                Thumb_STR_3();
                        }
                        else
                        {
                            if (IsOpcodeBitSet(11))
                                Thumb_LDRH();
                            else
                                Thumb_STRH();
                        }
                        break;

                    case 5:
                        if (IsOpcodeBitSet(12))
                        {
                            switch ((Opcode >> 9) & 3)
                            {
                                case 0:
                                    if (IsOpcodeBitSet(7))
                                        Thumb_SUB_4();
                                    else
                                        Thumb_ADD_7();
                                    break;
                                case 1:
                                    if (IsOpcodeBitSet(11))
                                    {
                                        switch((Opcode >> 6) & 3)
                                        {
                                            case 0: Thumb_REV(); break;
                                            case 1: Thumb_REV16(); break;
                                            case 3: Thumb_REVSH(); break;
                                        }
                                    }
                                    else
                                    {
                                        switch ((Opcode >> 6) & 3)
                                        {
                                            case 0: Thumb_SXTH(); break;
                                            case 1: Thumb_SXTB(); break;
                                            case 2: Thumb_UXTH(); break;
                                            case 3: Thumb_UXTB(); break;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (IsOpcodeBitSet(11))
                                        Thumb_POP();
                                    else
                                        Thumb_PUSH();
                                    break;
                                case 3:
                                    if (IsOpcodeBitSet(11))
                                        Thumb_BKPT();
                                    else
                                    {
                                        if (IsOpcodeBitSet(5))
                                            Thumb_CPS();
                                        else
                                            Thumb_SETEND();
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            if (IsOpcodeBitSet(11))
                                Thumb_ADD_6();
                            else
                                Thumb_ADD_5();
                        }
                        break;

                    case 6:
                        if (IsOpcodeBitSet(12))
                        {
                            if (((Opcode >> 8) & 0xf) == 0xf)
                                Thumb_SWI();
                            else
                                Thumb_B();
                        }
                        else
                        {
                            if (IsOpcodeBitSet(11))
                                Thumb_LDMIA();
                            else
                                Thumb_STMIA();
                        }
                        break;

                    case 7:
                        if (((Opcode >> 11) & 3) == 0)
                            Thumb_B_2();
                        else
                            Thumb_BLX();
                        break;
                }
            }
            else
            {
                Opcode = NextOpcode;
                NextOpcode = ReadUInt32(Registers[15]);
                Registers[15] += 4;
                Registers.PCChanged = false;

                ARMCondition Condition = (ARMCondition)(Opcode >> 28);
                if (IsConditionMet(Condition))
                {
                    switch ((Opcode >> 25) & 7)
                    {
                        case 0:
                        case 1:
                            if (IsOpcodeBitSet(4) && IsOpcodeBitSet(7) && !IsOpcodeBitSet(25))
                            {
                                if (((Opcode >> 5) & 3) > 0 || IsOpcodeBitSet(24))
                                    ARM_DecodeLoadAndStoreExtension();
                                else
                                    ARM_DecodeMultiplyExtension();
                            }
                            else
                            {
                                if (((Opcode >> 23) & 3) == 2 && !IsOpcodeBitSet(20))
                                    ARM_DecodeDSPExtension();
                                else //Data Processing
                                {
                                    switch ((Opcode >> 21) & 0xf)
                                    {
                                        case 0: ARM_AND(); break;
                                        case 1: ARM_EOR(); break;
                                        case 2: ARM_SUB(); break;
                                        case 3: ARM_RSB(); break;
                                        case 4: ARM_ADD(); break;
                                        case 5: ARM_ADC(); break;
                                        case 6: ARM_SBC(); break;
                                        case 7: ARM_RSC(); break;
                                        case 8: ARM_TST(); break;
                                        case 9: ARM_TEQ(); break;
                                        case 0xa: ARM_CMP(); break;
                                        case 0xb: ARM_CMN(); break;
                                        case 0xc: ARM_ORR(); break;
                                        case 0xd: ARM_MOV(); break;
                                        case 0xe: ARM_BIC(); break;
                                        case 0xf: ARM_MVN(); break;
                                    }
                                }
                            }
                            break;

                        case 2:
                        case 3:
                            if (IsOpcodeBitSet(4) && IsOpcodeBitSet(25))
                                ARM_DecodeMediaExtension();
                            else //Load/Store
                            {
                                if (IsOpcodeBitSet(20))
                                {
                                    if (IsOpcodeBitSet(22))
                                        ARM_LDRB();
                                    else
                                        ARM_LDR();
                                }
                                else
                                {
                                    if (IsOpcodeBitSet(22))
                                        ARM_STRB();
                                    else
                                        ARM_STR();
                                }
                            }
                            break;

                        case 4:
                            //Load/Store Multiple
                            if (IsOpcodeBitSet(20))
                                ARM_LDM();
                            else
                                ARM_STM();
                            break;

                        case 5: ARM_B(); break;

                        case 6:
                        case 7:
                            //Coprocessor
                            if (IsOpcodeBitSet(25))
                            {
                                if (IsOpcodeBitSet(24))
                                    ARM_SWI();
                                else
                                {
                                    if (IsOpcodeBitSet(4))
                                    {
                                        if (IsOpcodeBitSet(20))
                                            ARM_MRC();
                                        else
                                            ARM_MCR();
                                    }
                                    else
                                        ARM_CDP();
                                }
                            }
                            else
                            {
                                if (IsOpcodeBitSet(20))
                                    ARM_MRRC();
                                else
                                    ARM_MCRR();
                            }
                            break;
                    }
                }
                else if (Condition == ARMCondition.Unconditional)
                {
                    switch ((Opcode >> 26) & 3)
                    {
                        case 0:
                            if (IsOpcodeBitSet(16))
                                ARM_SETEND();
                            else
                                ARM_CPS();
                            break;
                        case 1: ARM_PLD(); break;
                        case 2:
                            if (IsOpcodeBitSet(25))
                                ARM_BLX();
                            else
                            {
                                if (IsOpcodeBitSet(22))
                                    ARM_SRS();
                                else
                                    ARM_RFE();
                            }
                            break;
                        case 3:
                            if (IsOpcodeBitSet(25))
                            {
                                if (IsOpcodeBitSet(20))
                                    ARM_MRC();
                                else
                                    ARM_MCR();
                            }
                            else
                            {
                                if (IsOpcodeBitSet(20))
                                    ARM_MRRC();
                                else
                                    ARM_MCRR();
                            }
                            break;
                    }
                }
            }

            if (Registers.PCChanged) ReloadPipeline();

            if (Debug)
            {
                string RVals = null;
                for (int i = 0; i < 15; i++) RVals += "R" + i + " = " + Registers[i].ToString("X8") + " ";
                System.Diagnostics.Debug.WriteLine(Opcode.ToString("X8") + " - " + TruePC.ToString("X8") + ": " + RVals + " CPSR: " + Registers.CPSR.ToString("X8"));
            }
        }

        /// <summary>
        ///     Decodes and executes a Digital Signal Processing related instruction.
        /// </summary>
        private void ARM_DecodeDSPExtension()
        {
            if (IsOpcodeBitSet(25))
                ARM_MSR();
            else
            {
                if (IsOpcodeBitSet(7)) //Signed Multiplies
                {
                    switch ((Opcode >> 21) & 3)
                    {
                        case 0: ARM_SMLA(); break;
                        case 1:
                            if (IsOpcodeBitSet(5))
                                ARM_SMULW();
                            else
                                ARM_SMLAW();
                            break;
                        case 2: ARM_SMLAL(); break;
                        case 3: ARM_SMUL(); break;
                    }
                }
                else
                {
                    switch ((Opcode >> 4) & 7)
                    {
                        case 0:
                            if (IsOpcodeBitSet(21))
                                ARM_MSR();
                            else
                                ARM_MRS();
                            break;
                        case 1:
                            if (IsOpcodeBitSet(22))
                                ARM_CLZ();
                            else
                                ARM_BX();
                            break;
                        case 2: ARM_BXJ(); break;
                        case 3: ARM_BLX_2(); break;
                        case 5:
                            switch ((Opcode >> 21) & 3) //Saturation
                            {
                                case 0: ARM_QADD(); break;
                                case 1: ARM_QSUB(); break;
                                case 2: ARM_QDADD(); break;
                                case 3: ARM_QDSUB(); break;
                            }
                            break;
                        case 7: ARM_BKPT(); break;
                    }
                }
            }
        }

        /// <summary>
        ///     Decodes and executes a Load or Store instruction.
        /// </summary>
        private void ARM_DecodeLoadAndStoreExtension()
        {
            if (IsOpcodeBitSet(6))
            {
                if (IsOpcodeBitSet(20))
                {
                    if (IsOpcodeBitSet(5))
                        ARM_LDRSH();
                    else
                        ARM_LDRSB();
                }
                else
                {
                    if (IsOpcodeBitSet(5))
                        ARM_STRD();
                    else
                        ARM_LDRD();
                }
            }
            else
            {
                if (IsOpcodeBitSet(5))
                {
                    if (IsOpcodeBitSet(20))
                        ARM_LDRH();
                    else
                        ARM_STRH();
                }
                else
                {
                    if (IsOpcodeBitSet(23))
                    {
                        if (IsOpcodeBitSet(20))
                            ARM_LDREX();
                        else
                            ARM_STREX();
                    }
                    else
                    {
                        if (IsOpcodeBitSet(22))
                            ARM_SWPB();
                        else
                            ARM_SWP();
                    }
                }
            }
        }

        /// <summary>
        ///     Decodes and executes a Media related instruction.
        /// </summary>
        private void ARM_DecodeMediaExtension()
        {
            switch ((Opcode >> 23) & 3)
            {
                case 0: //Parallel Add/Subtract
                    switch ((Opcode >> 20) & 7)
                    {
                        case 1: //Signed
                            switch ((Opcode >> 5) & 7)
                            {
                                case 0: ARM_SADD16(); break;
                                case 1: ARM_SADDSUBX(); break;
                                case 2: ARM_SSUBADDX(); break;
                                case 3: ARM_SSUB16(); break;
                                case 4: ARM_SADD8(); break;
                                case 7: ARM_SSUB8(); break;
                            }
                            break;
                        case 2: //Saturating
                            switch ((Opcode >> 5) & 7)
                            {
                                case 0: ARM_QADD16(); break;
                                case 1: ARM_QADDSUBX(); break;
                                case 2: ARM_QSUBADDX(); break;
                                case 3: ARM_QSUB16(); break;
                                case 4: ARM_QADD8(); break;
                                case 7: ARM_QSUB8(); break;
                            }
                            break;
                        case 3: //Signed Halving
                            switch ((Opcode >> 5) & 7)
                            {
                                case 0: ARM_SHADD16(); break;
                                case 1: ARM_SHADDSUBX(); break;
                                case 2: ARM_SHSUBADDX(); break;
                                case 3: ARM_SHSUB16(); break;
                                case 4: ARM_SHADD8(); break;
                                case 7: ARM_SHSUB8(); break;
                            }
                            break;
                        case 5: //Unsigned
                            switch ((Opcode >> 5) & 7)
                            {
                                case 0: ARM_UADD16(); break;
                                case 1: ARM_UADDSUBX(); break;
                                case 2: ARM_USUBADDX(); break;
                                case 3: ARM_USUB16(); break;
                                case 4: ARM_UADD8(); break;
                                case 7: ARM_USUB8(); break;
                            }
                            break;
                        case 6: //Unsigned Saturating
                            switch ((Opcode >> 5) & 7)
                            {
                                case 0: ARM_UQADD16(); break;
                                case 1: ARM_UQADDSUBX(); break;
                                case 2: ARM_UQSUBADDX(); break;
                                case 3: ARM_UQSUB16(); break;
                                case 4: ARM_UQADD8(); break;
                                case 7: ARM_UQSUB8(); break;
                            }
                            break;
                        case 7: //Unsigned Halving
                            switch ((Opcode >> 5) & 7)
                            {
                                case 0: ARM_UHADD16(); break;
                                case 1: ARM_UHADDSUBX(); break;
                                case 4: ARM_UHADD8(); break;
                            }
                            break;
                    }
                    break;
                case 1:
                    if (IsOpcodeBitSet(5))
                    {
                        switch ((Opcode >> 6) & 3)
                        {
                            case 0:
                                switch ((Opcode >> 20) & 3)
                                {
                                    case 2:
                                        if (IsOpcodeBitSet(22))
                                            ARM_USAT16();
                                        else
                                            ARM_SSAT16();
                                        break;
                                    case 3: ARM_REV(); break;
                                }
                                break;
                            case 1:
                                bool R15 = ((Opcode >> 16) & 0xf) == 0xf;

                                switch ((Opcode >> 20) & 7)
                                {
                                    case 0:
                                        if (R15)
                                            ARM_SXTB16();
                                        else
                                            ARM_SXTAB16();
                                        break;
                                    case 2:
                                        if (R15)
                                            ARM_SXTB();
                                        else
                                            ARM_SXTAB();
                                        break;
                                    case 3:
                                        if (R15)
                                            ARM_SXTH();
                                        else
                                            ARM_SXTAH();
                                        break;
                                    case 4:
                                        if (R15)
                                            ARM_UXTB16();
                                        else
                                            ARM_UXTAB16();
                                        break;
                                    case 6:
                                        if (R15)
                                            ARM_UXTB();
                                        else
                                            ARM_UXTAB();
                                        break;
                                    case 7:
                                        if (R15)
                                            ARM_UXTH();
                                        else
                                            ARM_UXTAH();
                                        break;
                                }
                                break;
                            case 2:
                                switch ((Opcode >> 20) & 7)
                                {
                                    case 0: ARM_SEL(); break;
                                    case 3: ARM_REV16(); break;
                                    case 7: ARM_REVSH(); break;
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (IsOpcodeBitSet(21))
                        {
                            if (IsOpcodeBitSet(22))
                                ARM_USAT();
                            else
                                ARM_SSAT();
                        }
                        else
                        {
                            if (IsOpcodeBitSet(6))
                                ARM_PKHTB();
                            else
                                ARM_PKHBT();
                        }
                    }
                    break;
                case 2: //Multiplies
                    switch ((Opcode >> 20) & 7)
                    {
                        case 0:
                            switch ((Opcode >> 6) & 3)
                            {
                                case 0:
                                    if (((Opcode >> 12) & 0xf) == 0xf)
                                        ARM_SMUAD();
                                    else
                                        ARM_SMLAD();
                                    break;
                                case 1:
                                    if (((Opcode >> 12) & 0xf) == 0xf)
                                        ARM_SMUSD();
                                    else
                                        ARM_SMLSD();
                                    break;
                            }
                            break;
                        case 4:
                            switch ((Opcode >> 6) & 3)
                            {
                                case 0: ARM_SMLALD(); break;
                                case 1: ARM_SMLSLD(); break;
                            }
                            break;
                        case 5:
                            switch ((Opcode >> 6) & 3)
                            {
                                case 0:
                                    if (((Opcode >> 12) & 0xf) == 0xf)
                                        ARM_SMMUL();
                                    else
                                        ARM_SMMLA();
                                    break;
                                case 3: ARM_SMMLS(); break;
                            }
                             break;

                    }
                    break;
                case 3:
                    if (((Opcode >> 12) & 0xf) == 0xf)
                        ARM_USADA8();
                    else
                        ARM_USAD8();
                    break;
            }
        }

        /// <summary>
        ///     Decodes and executes a Multiply instruction.
        /// </summary>
        private void ARM_DecodeMultiplyExtension()
        {
            switch ((Opcode >> 21) & 7)
            {
                case 0: ARM_MUL(); break;
                case 1: ARM_MLA(); break;
                case 2: ARM_UMAAL(); break;
                case 4: ARM_UMULL(); break;
                case 5: ARM_UMLAL(); break;
                case 6: ARM_SMULL(); break;
                case 7: ARM_SMLAL(); break;
            }
        }
    }
}
