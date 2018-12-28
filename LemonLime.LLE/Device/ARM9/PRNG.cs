using System;

namespace LemonLime.LLE.Device.ARM9
{
    class PRNG : Device
    {
        private Random RNG;

        public PRNG()
        {
            this.RNG = new Random();
        }

        public uint ReadWord(uint Offset)
        {
            return (uint)this.RNG.Next();
        }

        public ushort ReadShort(uint Offset)
        {
            return (ushort)this.RNG.Next();
        }

        public byte ReadByte(uint Offset)
        {
            return (byte)this.RNG.Next();
        }

        public void WriteWord (uint Offset, uint   Value) { }
        public void WriteShort(uint Offset, ushort Value) { }
        public void WriteByte (uint Offset, byte   Value) { }

        public uint   Size() { return 4096;   }
        public String Name() { return "PRNG"; }
    }
}
