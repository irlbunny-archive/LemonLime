using LemonLime.Common;

namespace LemonLime.LLE.Device.ARM9
{
    // TODO
    class IRQ : CPU.Device
    {
        public uint ReadWord(uint Offset)
        {
            Logger.WriteInfo($"Offset = {Offset}");
            return 0;
        }

        public ushort ReadShort(uint Offset) { return 0; }
        public byte   ReadByte (uint Offset) { return 0; }

        public void WriteWord(uint Offset, uint Value)
        {
            Logger.WriteInfo($"Offset = {Offset}");
        }

        public void WriteShort(uint Offset, ushort Value) { }
        public void WriteByte (uint Offset, byte   Value) { }

        public uint   Size() { return 8;     }
        public string Name() { return "IRQ"; }
    }
}
