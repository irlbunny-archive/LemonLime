namespace LemonLime.LLE.Device.ARM9
{
    // TODO
    class XDMA : CPU.Device
    {
        public uint   ReadWord (uint Offset) { return 0; }
        public ushort ReadShort(uint Offset) { return 0; }
        public byte   ReadByte (uint Offset) { return 0; }

        public void WriteWord (uint Offset, uint   Value) { }
        public void WriteShort(uint Offset, ushort Value) { }
        public void WriteByte (uint Offset, byte   Value) { }

        public uint   Size() { return 512;    } // TODO: Is this wrong?
        public string Name() { return "XDMA"; }
    }
}
