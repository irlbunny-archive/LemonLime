using System;
using System.IO;

using LemonLime.ARM;
using LemonLime.Common;

namespace LemonLime.LLE.Device.Generic
{
    public class ROM : CPUDevice
    {
        // TODO: Potentially move to a memory mapped file?

        private FastMemoryBuffer Buffer;
        private String DevName;

        public ROM(String Path, String Name)
        {
            byte[] FileData = File.ReadAllBytes(Path);

            if ((FileData.Length % 16) != 0)
                throw new Exception("ROM file size should always be a multiple of 16");

            this.Buffer = new FastMemoryBuffer((uint)FileData.Length);
            for (uint i = 0; i < FileData.Length; i++)
                this.Buffer.WriteByte(i, FileData[i]);

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

        public void WriteWord(uint Offset, uint Value) {}
        public void WriteShort(uint Offset, ushort Value) {}
        public void WriteByte(uint Offset, byte Value) {}

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
