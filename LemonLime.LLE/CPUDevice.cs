using LemonLime.ARM;
using LemonLime.Common;
using System;
using System.IO;

namespace LemonLime.LLE.Device
{
    public interface CPUDevice
    {
        uint ReadUInt32(uint Offset);
        void WriteUInt32(uint Offset, uint Word);

        uint Size();
        String Name();
    }
}
