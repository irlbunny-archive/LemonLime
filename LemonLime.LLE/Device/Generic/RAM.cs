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

    	public uint ReadUInt32(uint Offset)
    	{
    		return this.Buffer.ReadWord(Offset);
    	}

    	public void WriteUInt32(uint Offset, uint Word)
    	{
            this.Buffer.WriteWord(Offset, Word);
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
