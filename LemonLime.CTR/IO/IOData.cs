namespace LemonLime.CTR.IO
{
    class IOData
    {
        public CPUHandler CPU;
        public uint       Address;
        public IOType     Type;
        public int        Width;
        public byte       Write8;
        public ushort     Write16;
        public uint       Write32;
        public byte       Read8;
        public ushort     Read16;
        public uint       Read32;

        public IOData(CPUHandler CPU, uint Address,
            IOType Type, int Width,
            byte Write8 = 0, ushort Write16 = 0,
            uint Write32 = 0)
        {
            this.CPU     = CPU;
            this.Address = Address;
            this.Type    = Type;
            this.Width   = Width;
            this.Write8  = Write8;
            this.Write16 = Write16;
            this.Write32 = Write32;
            this.Read8   = 0;
            this.Read16  = 0;
            this.Read32  = 0;
        }
    }
}
