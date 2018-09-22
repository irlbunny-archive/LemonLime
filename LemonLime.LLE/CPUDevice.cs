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
        void Update();
    }

    class CPURAM : CPUDevice
    {
    	private uint[] RAM;
    	public CPURAM(uint Size)
    	{
    		this.RAM = new uint[Size / 4];
    	}

    	public uint ReadUInt32(uint Offset)
    	{
    		return this.RAM[Offset / 4];
    	}

    	public void WriteUInt32(uint Offset, uint Word)
    	{
    		this.RAM[Offset / 4] = Word;
    	}

    	public void Update() {}
    }

    class CPUROM : CPUDevice
    {
    	private byte[] ROM;
    	public CPUROM(String Path)
    	{
    		this.ROM = File.ReadAllBytes(Path);
    	}

    	public uint ReadUInt32(uint Offset)
    	{
    		uint Word = 0;
    		Word |= (uint)this.ROM[Offset + 0];
    		Word |= (uint)this.ROM[Offset + 1] << 8;
    		Word |= (uint)this.ROM[Offset + 2] << 16;
    		Word |= (uint)this.ROM[Offset + 3] << 24;
    		return Word;
    	}

    	public void WriteUInt32(uint Offset, uint Word) {}
    	public void Update() {}
    }
}
