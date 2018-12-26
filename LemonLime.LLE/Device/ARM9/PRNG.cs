using System;
using System.IO;

using LemonLime.ARM;
using LemonLime.Common;

namespace LemonLime.LLE.Device.ARM9
{
    public class PRNG : CPUDevice
    {
        private Random RandomGenerator;

        public PRNG()
        {
            this.RandomGenerator = new Random();
        }

        public uint ReadWord(uint Offset)
        {
            return (uint)this.RandomGenerator.Next();
        }

        public ushort ReadShort(uint Offset)
        {
            return (ushort)this.RandomGenerator.Next();
        }

        public byte ReadByte(uint Offset)
        {
            return (byte)this.RandomGenerator.Next();
        }

        public void WriteWord(uint Offset, uint Value) {}
        public void WriteShort(uint Offset, ushort Value) {}
        public void WriteByte(uint Offset, byte Value) {}

        public uint Size()
        {
            return 4096;
        }

        public String Name()
        {
            return "PRNG";
        }
    }
}
