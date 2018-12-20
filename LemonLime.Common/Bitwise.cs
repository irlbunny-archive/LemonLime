namespace LemonLime.Common
{
    public class Bitwise
    {
        public static bool GetBit(byte   Input, int Position) { return (Input & (1 << Position - 1)) != 0; }
        public static bool GetBit(ushort Input, int Position) { return (Input & (1 << Position - 1)) != 0; }
        public static bool GetBit(uint   Input, int Position) { return (Input & (1 << Position - 1)) != 0; }
    }
}
