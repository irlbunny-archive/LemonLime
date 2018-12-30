using System;
using System.IO;

using LemonLime.Common;

namespace LemonLime.LLE.Device.ARM9
{
    class OTP : CPU.Device
    {
        private FMemBuffer OTPRegisters;

        public OTP()
        {
            this.OTPRegisters = new FMemBuffer(0x108);
        }

        public void LockOut()
        {
            for (uint Index = 0; Index < 256; Index += 4)
                this.OTPRegisters.WriteWord(Index, 0xFFFFFFFF);
        }

        public void SetOTP(byte[] OTP)
        {
            if (OTP.Length != 256)
                throw new Exception($"OTP size is invalid (should be 256 bytes, not {OTP.Length})");

            for (uint Index = 0; Index < OTP.Length; Index++)
                this.OTPRegisters.WriteByte(Index, OTP[Index]);
        }

        public void SetOTP(String Path)
        {
            try
            {
                this.SetOTP(File.ReadAllBytes(Path));
            }
            catch (Exception e)
            {
                Logger.WriteError($"Failed to load data from {Path} to the OTP buffer");
                Logger.WriteError(e.ToString());
            }
        }

        public uint ReadWord(uint Offset)
        {
            return this.OTPRegisters.ReadWord(Offset);
        }

        public ushort ReadShort(uint Offset)
        {
            return this.OTPRegisters.ReadShort(Offset);
        }

        public byte ReadByte(uint Offset)
        {
            return this.OTPRegisters.ReadByte(Offset);
        }

        public void WriteWord (uint Offset, uint   Value) { }
        public void WriteShort(uint Offset, ushort Value) { }
        public void WriteByte (uint Offset, byte   Value) { }

        public uint   Size() { return 264;   }
        public String Name() { return "OTP"; }
    }
}
