namespace LemonLime.CTR.IO
{
    class IOEntry
    {
        public delegate void IOFunc(IOData Data);

        public IOFunc Register;
        public uint   Address;
        public int    Width;
        public bool   Locked;

        public IOEntry(IOFunc Register, uint Address, int Width)
        {
            this.Register = Register;
            this.Address  = Address;
            this.Width    = Width;
        }
    }
}
