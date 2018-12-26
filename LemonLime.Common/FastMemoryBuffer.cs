using System;
using System.Runtime.InteropServices;

using LemonLime.Common;

namespace LemonLime.Common
{
    public class FastMemoryBuffer
    {
        private IntPtr Memory;
        private uint MemorySize;

        public FastMemoryBuffer(uint Size)
        {
            if ((int)Size < 0)
                throw new Exception($"Marshal.AllocHGlobal can't allocate over 2GiB of memory (tried {Size} bytes)");

            this.Memory = Marshal.AllocHGlobal((int)Size);
            this.MemorySize = Size;
        }

        ~FastMemoryBuffer()
        {
            Marshal.FreeHGlobal(this.Memory);
        }

        public uint ByteSize()
        {
            return this.MemorySize;
        }

        public byte ReadByte(uint Offset)
        {
            return Marshal.ReadByte(this.Memory, (int)Offset);
        }

        public void WriteByte(uint Offset, byte Value)
        {
            Marshal.WriteByte(this.Memory, (int)Offset, Value);
        }

        public ushort ReadShort(uint Offset)
        {
            return (ushort)Marshal.ReadInt16(this.Memory, (int)Offset);
        }

        public void WriteShort(uint Offset, ushort Value)
        {
            Marshal.WriteInt16(this.Memory, (int)Offset, (short)Value);
        }

        public uint ReadWord(uint Offset)
        {
            return (uint)Marshal.ReadInt32(this.Memory, (int)Offset);
        }

        public void WriteWord(uint Offset, uint Value)
        {
            Marshal.WriteInt32(this.Memory, (int)Offset, (int)Value);
        }

        public ulong ReadLong(uint Offset)
        {
            return (ulong)Marshal.ReadInt64(this.Memory, (int)Offset);
        }

        public void WriteLong(uint Offset, ulong Value)
        {
            Marshal.WriteInt64(this.Memory, (int)Offset, (long)Value);
        }
    }
}
