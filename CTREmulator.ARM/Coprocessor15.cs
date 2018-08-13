namespace CTREmulator.ARM
{
    class Coprocessor15
    {
        //ARM1176 CP15 setup

        private const uint CP15_MainId = 0x410FB767;
        private const uint CP15_CacheType = 0x10152152;
        private const uint CP15_TCMStatus = 0x20002;
        private const uint CP15_TLBType = 0x800;

        private const uint CP15_ProcessorFeature0 = 0x111;
        private const uint CP15_ProcessorFeature1 = 0x11;
        private const uint CP15_DebugFeature0 = 0x33;
        private const uint CP15_AuxiliaryFeature0 = 0;
        private const uint CP15_MemoryModelFeature0 = 0x1130003;
        private const uint CP15_MemoryModelFeature1 = 0x10030302;
        private const uint CP15_MemoryModelFeature2 = 0x1222100;
        private const uint CP15_MemoryModelFeature3 = 0;

        private const uint CP15_InstructionSetFeatureAttribute0 = 0x140011;
        private const uint CP15_InstructionSetFeatureAttribute1 = 0x12002111;
        private const uint CP15_InstructionSetFeatureAttribute2 = 0x11231121;
        private const uint CP15_InstructionSetFeatureAttribute3 = 0x1102131;
        private const uint CP15_InstructionSetFeatureAttribute4 = 0x1141;
        private const uint CP15_InstructionSetFeatureAttribute5 = 0;

        private uint CP15_Control;
        private uint CP15_AuxiliaryControl;
        private uint CP15_CoprocessorAccessControl;

        /// <summary>
        ///     Reads a value from the "CP15" Coprocessor.
        /// </summary>
        /// <param name="CRn">The CRn register index</param>
        /// <param name="Op1">The Opcode 1 value</param>
        /// <param name="CRm">The CRm register index</param>
        /// <param name="Op2">The Opcode 2 value</param>
        /// <returns>The value at the given register</returns>
        public uint Read(int CRn, uint Op1, int CRm, uint Op2)
        {
            if (Op1 == 0)
            {
                switch (CRn)
                {
                    case 0:
                        switch (CRm)
                        {
                            case 0:
                                switch (Op2)
                                {
                                    case 0: return CP15_MainId;
                                    case 1: return CP15_CacheType;
                                    case 2: return CP15_TCMStatus;
                                    case 3: return CP15_TLBType;
                                }
                                break;
                            case 1:
                                switch (Op2)
                                {
                                    case 0: return CP15_ProcessorFeature0;
                                    case 1: return CP15_ProcessorFeature1;
                                    case 2: return CP15_DebugFeature0;
                                    case 3: return CP15_AuxiliaryFeature0;
                                    case 4: return CP15_MemoryModelFeature0;
                                    case 5: return CP15_MemoryModelFeature1;
                                    case 6: return CP15_MemoryModelFeature2;
                                    case 7: return CP15_MemoryModelFeature3;
                                }
                                break;
                            case 2:
                                switch (Op2)
                                {
                                    case 0: return CP15_InstructionSetFeatureAttribute0;
                                    case 1: return CP15_InstructionSetFeatureAttribute1;
                                    case 2: return CP15_InstructionSetFeatureAttribute2;
                                    case 3: return CP15_InstructionSetFeatureAttribute3;
                                    case 4: return CP15_InstructionSetFeatureAttribute4;
                                    case 5: return CP15_InstructionSetFeatureAttribute5;
                                }
                                break;
                        }
                        break;
                    case 2:
                        switch (CRm)
                        {
                            case 0:
                                switch (Op2)
                                {
                                    case 0: return CP15_Control;
                                    case 1: return CP15_AuxiliaryControl;
                                    case 2: return CP15_CoprocessorAccessControl;
                                }
                                break;
                        }
                        break;
                }
            }

            return 0;
        }

        /// <summary>
        ///     Writes a value to the "CP15" Coprocessor.
        /// </summary>
        /// <param name="CRn">The CRn register index</param>
        /// <param name="Op1">The Opcode 1 value</param>
        /// <param name="CRm">The CRm register index</param>
        /// <param name="Op2">The Opcode 2 value</param>
        /// <param name="Value">Value to be written</param>
        public void Write(int CRn, uint Op1, int CRm, uint Op2, uint Value)
        {
            if (Op1 == 0)
            {
                switch (CRn)
                {
                    case 2:
                        switch (CRm)
                        {
                            case 0:
                                switch (Op2)
                                {
                                    case 0: CP15_Control = Value; break;
                                    case 1: CP15_AuxiliaryControl = Value; break;
                                    case 2: CP15_CoprocessorAccessControl = Value; break;
                                }
                                break;
                        }
                        break;
                }
            }
        }
    }
}
