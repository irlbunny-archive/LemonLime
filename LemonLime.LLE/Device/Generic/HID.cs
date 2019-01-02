using LemonLime.Common;

namespace LemonLime.LLE.Device.Generic
{
    class HID : CPU.Device
    {
        public uint ReadWord(uint Offset) { return 0; }

        public ushort ReadShort(uint Offset)
        {
            switch (Offset)
            {
                case 0: // HID_PAD
                    Logger.WriteInfo("HID_PAD, stubbed.");
                    break;

                case 2: // HID_UNKNOWN0
                    Logger.WriteInfo("HID_UNKNOWN0, stubbed.");
                    break;
            }

            return 0;
        }

        public byte ReadByte(uint Offset) { return 0; }

        public void WriteWord(uint Offset, uint Value) { }

        public void WriteShort(uint Offset, ushort Value)
        {
            switch (Offset)
            {
                case 2: // HID_UNKNOWN0
                    Logger.WriteInfo("HID_UNKNOWN0, stubbed.");
                    break;
            }
        }

        public void WriteByte(uint Offset, byte Value) { }

        public uint   Size() { return 4;     }
        public string Name() { return "HID"; }
    }
}
