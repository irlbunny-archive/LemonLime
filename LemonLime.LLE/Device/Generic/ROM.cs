using System;
using System.IO;

using LemonLime.ARM;
using LemonLime.Common;

namespace LemonLime.LLE.Device.Generic
{
    public class ROM : CPUDevice
    {
        private FastMemoryBuffer Buffer;
        private String DevName;

        public ROM(String Path, String Name)
        {
            byte[] FileData = File.ReadAllBytes(Path);

            this.Buffer = new FastMemoryBuffer((uint)FileData.Length);
            for (uint i = 0; i < FileData.Length; i++)
                this.Buffer.WriteByte(i, FileData[i]);

            this.DevName = Name;
        }

        public uint ReadUInt32(uint Offset)
        {
            return this.Buffer.ReadWord(Offset);
        }

        public void WriteUInt32(uint Offset, uint Word) {}

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
