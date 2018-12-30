using System;

namespace LemonLime.LLE.CPU
{
    interface Device
    {
        uint   ReadWord (uint Offset);
        ushort ReadShort(uint Offset);
        byte   ReadByte (uint Offset);

        void WriteWord (uint Offset, uint   Value);
        void WriteShort(uint Offset, ushort Value);
        void WriteByte (uint Offset, byte   Value);

        uint   Size();
        String Name();
    }
}
