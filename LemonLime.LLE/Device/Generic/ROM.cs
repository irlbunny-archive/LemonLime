using System;
using System.IO;

namespace LemonLime.LLE.Device.Generic
{
    public class ROM : LemonLime.LLE.Device.CPUDevice
    {
        private byte[] Buffer;
        private String DevName;

        public ROM(String Path, String Name)
        {
            this.Buffer = File.ReadAllBytes(Path);
            this.DevName = Name;
        }

        public uint ReadUInt32(uint Offset)
        {
            uint Word = 0;
            Word |= (uint)this.Buffer[Offset + 0];
            Word |= (uint)this.Buffer[Offset + 1] << 8;
            Word |= (uint)this.Buffer[Offset + 2] << 16;
            Word |= (uint)this.Buffer[Offset + 3] << 24;
            return Word;
        }

        public void WriteUInt32(uint Offset, uint Word) {}

        public uint Size()
        {
            return (uint)this.Buffer.Length * sizeof(byte);
        }

        public String Name()
        {
            return this.DevName;
        }
    }
}
