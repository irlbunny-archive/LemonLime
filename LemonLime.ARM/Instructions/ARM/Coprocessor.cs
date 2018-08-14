namespace LemonLime.ARM
{
    public partial class Interpreter
    {
        /*
         * Note: Most of those instructions are just stubs, since coprocessors aren't implemented,
         * except for a few stuff of CP15. It have just the enough to make basic stuff work.
         */

        /// <summary>
        ///     Coprocessor Data Processing.
        /// </summary>
        private void ARM_CDP()
        {
            int CRm = (int)(Opcode & 0xf);
            uint Opcode2 = (Opcode >> 5) & 7;
            uint CoprocessorIndex = (Opcode >> 8) & 0xf;
            int CRd = (int)((Opcode >> 12) & 0xf);
            int CRn = (int)((Opcode >> 16) & 0xf);
            uint Opcode1 = (Opcode >> 20) & 0xf;
        }

        /// <summary>
        ///     Load Coprocessor.
        /// </summary>
        private void ARM_LDC()
        {
            uint CoprocessorIndex = (Opcode >> 8) & 0xf;
            int CRd = (int)((Opcode >> 12) & 0xf);
            bool N = (Opcode & 0x400000) != 0;
            uint Address = ARM_GetLoadAndStoreCPAddress();
        }

        /// <summary>
        ///     Move to Coprocessor from Register.
        /// </summary>
        private void ARM_MCR()
        {
            int CRm = (int)(Opcode & 0xf);
            uint Opcode2 = (Opcode >> 5) & 7;
            uint CoprocessorIndex = (Opcode >> 8) & 0xf;
            int Rd = (int)((Opcode >> 12) & 0xf);
            int CRn = (int)((Opcode >> 16) & 0xf);
            uint Opcode1 = (Opcode >> 21) & 7;

            if (CoprocessorIndex == 15) CP15.Write(CRn, Opcode1, CRm, Opcode2, Registers[Rd]);
        }

        /// <summary>
        ///     Move to Coprocessor from two Registers.
        /// </summary>
        private void ARM_MCRR()
        {
            int CRm = (int)(Opcode & 0xf);
            uint CoprocessorOpcode = (Opcode >> 4) & 0xf;
            uint CoprocessorIndex = (Opcode >> 8) & 0xf;
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);
        }

        /// <summary>
        ///     Move to Register from Coprocessor.
        /// </summary>
        private void ARM_MRC()
        {
            int CRm = (int)(Opcode & 0xf);
            uint Opcode2 = (Opcode >> 5) & 7;
            uint CoprocessorIndex = (Opcode >> 8) & 0xf;
            int Rd = (int)((Opcode >> 12) & 0xf);
            int CRn = (int)((Opcode >> 16) & 0xf);
            uint Opcode1 = (Opcode >> 21) & 7;

            if (CoprocessorIndex == 15)
            {
                uint Value = CP15.Read(CRn, Opcode1, CRm, Opcode2);

                if (Rd == 15)
                    Registers.CPSR = (Registers.CPSR & 0xfffffff) | (Value & 0xf0000000);
                else
                    Registers[Rd] = Value;
            }
        }

        /// <summary>
        ///     Move to two Registers from Coprocessor.
        /// </summary>
        private void ARM_MRRC()
        {
            int CRm = (int)(Opcode & 0xf);
            uint CoprocessorOpcode = (Opcode >> 4) & 0xf;
            uint CoprocessorIndex = (Opcode >> 8) & 0xf;
            int Rd = (int)((Opcode >> 12) & 0xf);
            int Rn = (int)((Opcode >> 16) & 0xf);
        }

        /// <summary>
        ///     Store Coprocessor.
        /// </summary>
        private void ARM_STC()
        {
            uint CoprocessorIndex = (Opcode >> 8) & 0xf;
            int CRd = (int)((Opcode >> 12) & 0xf);
            bool N = IsOpcodeBitSet(22);
            uint Address = ARM_GetLoadAndStoreCPAddress();
        }
    }
}
