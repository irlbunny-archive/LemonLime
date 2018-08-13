namespace CTREmulator.ARM
{
    public partial class Interpreter
    {
        uint ExclusiveTag;
        bool ExclusiveEnabled;

        const uint ERGMask = 0xfffffff8;

        /// <summary>
        ///     Preload Data.
        /// </summary>
        private void ARM_PLD()
        {
            uint Address = ARM_GetLoadAndStoreAddress();

            //Nothing to do here...
            //But if you know how memory access may be optimized with PLD, go ahead and implement it! :)
        }

        /// <summary>
        ///     Set Endian.
        /// </summary>
        private void ARM_SETEND()
        {
            Registers.SetFlag(ARMFlag.Endianness, IsOpcodeBitSet(9));
        }
    }
}
