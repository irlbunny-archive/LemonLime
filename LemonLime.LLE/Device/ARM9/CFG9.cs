namespace LemonLime.LLE.Device.ARM9
{
    // TODO
    class CFG9 : CPU.Device
    {
        public uint   ReadWord (uint Offset) { return 0; }
        public ushort ReadShort(uint Offset) { return 0; }
        public byte   ReadByte (uint Offset) { return 0; }

        public void WriteWord (uint Offset, uint   Value) { }
        public void WriteShort(uint Offset, ushort Value) { }

        public void WriteByte(uint Offset, byte Value)
        {
            switch (Offset)
            {
                case 2: // CFG9_RST11
                    bool Reset = (Value << 31 != 1);
                    if (Reset != false) CTR.SetCPU(CPU.Type.ARM11, Reset);
                    break;
            }
        }

        public uint   Size() { return 30;     } // TODO: Possibly wrong?
        public string Name() { return "CFG9"; }
    }
}
