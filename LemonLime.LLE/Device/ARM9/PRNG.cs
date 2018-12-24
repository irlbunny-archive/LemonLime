using System;
using System.IO;

using LemonLime.ARM;
using LemonLime.Common;

namespace LemonLime.LLE.Device.ARM9
{
    public class PRNG : CPUDevice
    {
        private Random RandomGenerator;

    	public PRNG()
    	{
            this.RandomGenerator = new Random();
    	}

    	public uint ReadUInt32(uint Offset)
    	{
            return (uint)this.RandomGenerator.Next();
    	}

    	public void WriteUInt32(uint Offset, uint Word) {}

        public uint Size()
        {
            return 4096;
        }

        public String Name()
        {
            return "PRNG";
        }
    }
}
