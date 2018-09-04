namespace LemonLime.LLE.IO
{
    class IOEntry
    {
        public delegate void IOFunc(IOData Data);

        public IOFunc Register;
        public uint   Address;

        public IOEntry(IOFunc Register, uint Address)
        {
            this.Register = Register;
            this.Address  = Address;
        }
    }
}
