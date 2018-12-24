using LemonLime.ARM;
using LemonLime.Common;
using System;
using System.IO;

namespace LemonLime.LLE.Device.Generic
{
    public class RAM : CPUDevice
    {
    	private uint[] Buffer;
        private String DevName;

    	public RAM(uint Size, String Name)
    	{
    		this.Buffer = new uint[Size / 4];
            this.DevName = Name;
    	}

    	public uint ReadUInt32(uint Offset)
    	{
    		return this.Buffer[Offset / 4];
    	}

    	public void WriteUInt32(uint Offset, uint Word)
    	{
    		this.Buffer[Offset / 4] = Word;
    	}

        public uint Size()
        {
            return (uint)this.Buffer.Length * sizeof(uint);
        }

        public String Name()
        {
            return this.DevName;
        }
    }
}
