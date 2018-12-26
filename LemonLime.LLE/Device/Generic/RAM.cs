using System;
using System.IO;

using LemonLime.ARM;
using LemonLime.Common;

namespace LemonLime.LLE.Device.Generic
{
    public class RAM : CPUDevice
    {
        private FastMemoryBuffer Buffer;
        private String DevName;

        public RAM(uint Size, String Name)
        {
            this.Buffer = new FastMemoryBuffer(Size);
            this.DevName = Name;
        }

        public uint ReadWord(uint Offset)
        {
            return this.Buffer.ReadWord(Offset);
        }

        public ushort ReadShort(uint Offset)
        {
            return this.Buffer.ReadShort(Offset);
        }

        public byte ReadByte(uint Offset)
        {
            return this.Buffer.ReadByte(Offset);
        }

        public void WriteWord(uint Offset, uint Value)
        {
            this.Buffer.WriteWord(Offset, Value);
        }

        public void WriteShort(uint Offset, ushort Value)
        {
            this.Buffer.WriteShort(Offset, Value);
        }

        public void WriteByte(uint Offset, byte Value)
        {
            this.Buffer.WriteByte(Offset, Value);
        }

        public uint Size()
        {
            return this.Buffer.ByteSize();
        }

        public String Name()
        {
            return this.DevName;
        }
    }
}
