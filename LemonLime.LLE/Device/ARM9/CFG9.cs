using LemonLime.ARM;
using LemonLime.Common;
using System;
using System.IO;

namespace LemonLime.LLE.Device.ARM9
{
    enum CFG9_Register {
        SYSPROT9 = 0,
        SYSPROT11 = 1,
        RST11 = 2
    }

    public class CFG9 : CPUDevice
    {
    	private uint[] Buffer;
        private String DevName;

    	public CFG9()
    	{
            this.Buffer = new uint[1024];
    	}

    	public uint ReadUInt32(uint Offset)
    	{
    		return this.Buffer[Offset];
    	}

    	public void WriteUInt32(uint Offset, uint Word)
    	{
    		this.Buffer[Offset / 4] = Word;
    	}

        public uint Size()
        {
            return 4096;
        }

        public String Name()
        {
            return "CONFIG9";
        }
    }
}
