using System;
using System.Collections.Generic;
using System.Text;

namespace CTREmulator.CTR.Memory
{
    public abstract class IO32
    {
        private uint ReadValue;
        private uint WriteValue;

        public byte Read(uint Address)
        {
            switch (Address & 3)
            {
                case 0:
                    ReadValue = ProcessRead(Address);
                    return (byte)ReadValue;
                case 1: return (byte)(ReadValue >> 8);
                case 2: return (byte)(ReadValue >> 16);
                case 3: return (byte)(ReadValue >> 24);
            }

            return 0;
        }

        public void Write(uint Address, byte Value)
        {
            switch (Address & 3)
            {
                case 0: WriteValue = Value; break;
                case 1: WriteValue |= (uint)(Value << 8); break;
                case 2: WriteValue |= (uint)(Value << 16); break;
                case 3:
                    WriteValue |= (uint)(Value << 24);
                    ProcessWrite(Address & 0xfffffffc, WriteValue);
                    break;
            }
        }

        public abstract uint ProcessRead(uint Address);

        public abstract void ProcessWrite(uint Address, uint Value);
    }
}
