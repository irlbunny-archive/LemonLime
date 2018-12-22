using LemonLime.ARM;
using LemonLime.Common;
using System.Collections.Generic;

namespace LemonLime.LLE.Memory
{
    class Handler : IBus
    {
        public List<Map> Maps;

        public List<Map> Routes;

        public Handler()
        {
            Maps = new List<Map>();

            Routes = new List<Map>();
        }

        public byte ReadUInt8(uint Address)
        {
            Logger.WriteInfo($"Read @ 0x{Address.ToString("X8")}");
            return 0;
        }

        public ushort ReadUInt16(uint Address)
        {
            return (ushort)(ReadUInt8(Address) |
                (ReadUInt8(Address + 1) << 8));
        }

        public uint ReadUInt32(uint Address)
        {
            return (uint)(ReadUInt8(Address)   |
                (ReadUInt8(Address + 1) << 8)  |
                (ReadUInt8(Address + 2) << 16) |
                (ReadUInt8(Address + 3) << 24));
        }

        public void WriteUInt8(uint Address, byte Value)
        {
            Logger.WriteInfo($"Write @ 0x{Address.ToString("X8")}, Value = {Value.ToString("X")}");
        }

        public void WriteUInt16(uint Address, ushort Value)
        {
            WriteUInt8(Address,     (byte)Value);
            WriteUInt8(Address + 1, (byte)(Value >> 8));
        }

        public void WriteUInt32(uint Address, uint Value)
        {
            WriteUInt8(Address,     (byte)Value);
            WriteUInt8(Address + 1, (byte)(Value >> 8));
            WriteUInt8(Address + 2, (byte)(Value >> 16));
            WriteUInt8(Address + 3, (byte)(Value >> 24));
        }
    }
}
