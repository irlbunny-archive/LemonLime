using LemonLime.LLE.Memory;

namespace LemonLime.LLE.IO
{
    class Entry
    {
        public delegate void RegFunc(Context Ctx);

        public RegFunc Register;
        public uint    Address;

        public Entry(RegFunc Register, uint Address)
        {
            this.Register = Register;
            this.Address  = Address;
        }
    }
}
