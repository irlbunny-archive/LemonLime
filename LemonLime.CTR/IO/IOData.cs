using LemonLime.ARM;

namespace LemonLime.CTR.IO
{
    class IOData
    {
        public uint        Address;
        public IOType      Type;
        public int         Width;
        public byte        Data8;
        public ushort      Data16;
        public uint        Data32;
        public Interpreter CPU;

        public IOData(uint Address, IOType Type,
            int Width, byte Data8 = 0,
            ushort Data16 = 0, uint Data32 = 0)
        {
            this.Address = Address;
            this.Type    = Type;
            this.Width   = Width;
            this.Data8   = Data8;
            this.Data16  = Data16;
            this.Data32  = Data32;
        }
    }
}
