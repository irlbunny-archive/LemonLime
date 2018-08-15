namespace LemonLime.CTR.IO
{
    class IOEntry
    {
        public delegate byte IOFunc(IOData Data);

        public IOFunc Register;
        public uint   Address;
        public int    Width;

        public IOEntry(IOFunc Register, uint Address, int Width)
        {
            this.Register = Register;
            this.Address  = Address;
            this.Width    = Width;
        }
    }
}
