namespace LemonLime.LLE
{
    class Context
    {
        public  uint  Address;
        public  Type  Type;
        public  Width Width;
        private uint  Input;
        private uint  Output;

        public Context(uint Address, Type Type,
            Width Width, uint Input = 0)
        {
            this.Address = Address;
            this.Type    = Type;
            this.Width   = Width;
            this.Input   = Input;
            this.Output  = 0;
        }

        public byte   GetInputUInt8()  { return (byte)   Input; }
        public ushort GetInputUInt16() { return (ushort) Input; }
        public uint   GetInputUInt32() { return          Input; }

        public byte   GetOutputUInt8()  { return (byte)   Output; }
        public ushort GetOutputUInt16() { return (ushort) Output; }
        public uint   GetOutputUInt32() { return          Output; }

        public void SetOutput(uint Value) { Output = Value; }
    }
}
