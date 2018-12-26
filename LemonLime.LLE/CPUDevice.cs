using LemonLime.ARM;
using LemonLime.Common;
using System;
using System.IO;

namespace LemonLime.LLE.Device
{
    public interface CPUDevice
    {
        uint ReadWord(uint Offset);
        ushort ReadShort(uint Offset);
        byte ReadByte(uint Offset);

        void WriteWord(uint Offset, uint Value);
        void WriteShort(uint Offset, ushort Value);
        void WriteByte(uint Offset, byte Value);

        uint Size();
        String Name();
    }
}
