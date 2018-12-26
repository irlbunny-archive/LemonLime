using System;
using System.IO;

using LemonLime.ARM;
using LemonLime.Common;

namespace LemonLime.LLE.Device.ARM9
{
    public class OTP : CPUDevice
    {
        private FastMemoryBuffer OTP_Registers;

        public OTP()
        {
            this.OTP_Registers = new FastMemoryBuffer(0x108);
        }

        public void LockOut()
        {
            for (uint i = 0; i < 256; i += 4)
                this.OTP_Registers.WriteWord(i, 0xFFFFFFFF);
        }

        public void SetOTP(byte[] OTP)
        {
            if (OTP.Length != 256)
                throw new Exception($"OTP size is invalid (should be 256 bytes, not {OTP.Length})");

            for (uint i = 0; i < OTP.Length; i++)
                this.OTP_Registers.WriteByte(i, OTP[i]);
        }

        public void SetOTP(String Path)
        {
            try {
                this.SetOTP(File.ReadAllBytes(Path));
            } catch(Exception e) {
                Logger.WriteError($"Failed to load data from {Path} to the OTP buffer");
                Logger.WriteError(e.ToString());
            }
        }

        public uint ReadWord(uint Offset)
        {
            return this.OTP_Registers.ReadWord(Offset);
        }

        public ushort ReadShort(uint Offset)
        {
            return this.OTP_Registers.ReadShort(Offset);
        }

        public byte ReadByte(uint Offset)
        {
            return this.OTP_Registers.ReadByte(Offset);
        }

        public void WriteWord(uint Offset, uint Value) {}
        public void WriteShort(uint Offset, ushort Value) {}
        public void WriteByte(uint Offset, byte Value) {}

        public uint Size()
        {
            return 264;
        }

        public String Name()
        {
            return "OTP";
        }
    }
}
