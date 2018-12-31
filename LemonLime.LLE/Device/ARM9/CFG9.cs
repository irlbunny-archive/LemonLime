using System;

namespace LemonLime.LLE.Device.ARM9
{
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
                    Console.WriteLine("CFG9_RST11");
                    bool Reset = (Value << 31 != 1);
                    Console.WriteLine(Reset);
                    if (Reset && Value != 0) CTR.SetCPU(CPU.Type.ARM11, Reset);
                    break;
            }
        }

        public uint   Size() { return 30;     }
        public String Name() { return "CFG9"; }
    }
}
