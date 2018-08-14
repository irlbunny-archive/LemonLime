namespace LemonLime.ARM
{
    public partial class Interpreter
    {
        /// <summary>
        ///     ARM CPU conditions.
        /// </summary>
        private enum ARMCondition
        {
            Equal = 0,
            NotEqual = 1,
            CarrySet = 2,
            CarryClear = 3,
            Minus = 4,
            Plus = 5,
            Overflow = 6,
            NoOverflow = 7,
            UnsignedHigher = 8,
            UnsignedLowerOrSame = 9,
            SignedGreaterThanOrEqual = 0xa,
            SignedLessThan = 0xb,
            SignedGreaterThan = 0xc,
            SignedLessThanOrEqual = 0xd,
            Always = 0xe,
            Unconditional = 0xf
        }

        /// <summary>
        ///     Checks whenever the Condition of a Opcode matches the current Status Register, to see if the condition is true.
        /// </summary>
        /// <param name="Condition">The Condition of the Opcode</param>
        /// <returns>If the condition is true or not</returns>
        private bool IsConditionMet(ARMCondition Condition)
        {
            switch (Condition)
            {
                case ARMCondition.Equal: return Registers.IsFlagSet(ARMFlag.Zero);
                case ARMCondition.NotEqual: return Registers.IsFlagClear(ARMFlag.Zero);
                case ARMCondition.CarrySet: return Registers.IsFlagSet(ARMFlag.Carry);
                case ARMCondition.CarryClear: return Registers.IsFlagClear(ARMFlag.Carry);
                case ARMCondition.Minus: return Registers.IsFlagSet(ARMFlag.Negative);
                case ARMCondition.Plus: return Registers.IsFlagClear(ARMFlag.Negative);
                case ARMCondition.Overflow: return Registers.IsFlagSet(ARMFlag.Overflow);
                case ARMCondition.NoOverflow: return Registers.IsFlagClear(ARMFlag.Overflow);
                case ARMCondition.UnsignedHigher: return ConditionHI();
                case ARMCondition.UnsignedLowerOrSame: return ConditionLS();
                case ARMCondition.SignedGreaterThanOrEqual: return ConditionGE();
                case ARMCondition.SignedLessThan: return ConditionLT();
                case ARMCondition.SignedGreaterThan: return Registers.IsFlagClear(ARMFlag.Zero) && ConditionGE();
                case ARMCondition.SignedLessThanOrEqual: return Registers.IsFlagSet(ARMFlag.Zero) || ConditionLT();
                case ARMCondition.Always: return true;
            }

            return false;
        }

        /// <summary>
        ///     Checks if the Unsigned Higher condition is met, based on the values of the Status register.
        /// </summary>
        /// <returns>True if the condition is met, false otherwise</returns>
        private bool ConditionHI()
        {
            return Registers.IsFlagSet(ARMFlag.Carry) && Registers.IsFlagClear(ARMFlag.Zero);
        }

        /// <summary>
        ///     Checks if the Unsigned Lower or Same condition is met, based on the values of the Status register.
        /// </summary>
        /// <returns>True if the condition is met, false otherwise</returns>
        private bool ConditionLS()
        {
            return Registers.IsFlagClear(ARMFlag.Carry) || Registers.IsFlagSet(ARMFlag.Zero);
        }

        /// <summary>
        ///     Checks if the Greater Than or Equal condition is met, based on the values of the Status register.
        /// </summary>
        /// <returns>True if the condition is met, false otherwise</returns>
        private bool ConditionGE()
        {
            return Registers.IsFlagSet(ARMFlag.Negative) == Registers.IsFlagSet(ARMFlag.Overflow);
        }

        /// <summary>
        ///     Checks if the Less Than condition is met, based on the values of the Status register.
        /// </summary>
        /// <returns>True if the condition is met, false otherwise</returns>
        private bool ConditionLT()
        {
            return Registers.IsFlagSet(ARMFlag.Negative) != Registers.IsFlagSet(ARMFlag.Overflow);
        }
    }
}
