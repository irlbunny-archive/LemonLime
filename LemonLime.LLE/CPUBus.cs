using LemonLime.ARM;
using LemonLime.Common;
using LemonLime.LLE.Device;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LemonLime.LLE
{
    class CPUBus : IBus
    {
        class CPUMap
        {
            public CPUDevice Device;
            public uint Start, End;

            public CPUMap(CPUDevice Device, uint Start)
            {
                this.Device = Device;
                this.Start = Start;
                this.End = Start + Device.Size();
            }
        }

        private List<CPUMap> BusMap;

        public CPUBus()
        {
            BusMap = new List<CPUMap>();
        }

        public void Attach(CPUDevice Device, uint Start)
        {
            BusMap.Add(new CPUMap(Device, Start));
        }

        private CPUMap FindMap(uint Address)
        {
            CPUMap Map = BusMap.Where(map => Address >= map.Start && Address <= map.End).SingleOrDefault();
            if (Map == null) throw new Exception($"Unhandled read @ 0x{Address.ToString($"X8")}");
            return Map;
        }

        private CPUDevice FindDevice(uint Address)
        {
            return FindMap(Address).Device;
        }

        public uint ReadUInt32(uint Address)
        {
            CPUMap Map = FindMap(Address);
            return Map.Device.ReadUInt32(Address - Map.Start);
        }

        public ushort ReadUInt16(uint Address)
        {
            uint Mask = Address & 2;
            uint MaskAddr = Address - Mask;
            return (ushort)(ReadUInt32(MaskAddr) >> (int)((Address - MaskAddr)*8));
        }

        public byte ReadUInt8(uint Address)
        {
            uint Mask = Address & 3;
            uint MaskAddr = Address - Mask;
            return (byte)(ReadUInt32(MaskAddr) >> (int)((Address - MaskAddr)*8));
        }

        public void WriteUInt32(uint Address, uint Value)
        {
            CPUDevice dev = FindDevice(Address);
            dev.WriteUInt32(Address, Value);
        }

        public void WriteUInt16(uint Address, ushort Value)
        {
            int Shift = (int)Address & 2;
            int BitMask = 0xFFFF << (Shift * 8);

            uint WordAddress = Address - (uint)Shift;
            uint Word = this.ReadUInt32(WordAddress);

            Word &= (uint)(~BitMask);
            Word |= (uint)Value << Shift;

            this.WriteUInt32(WordAddress, Word);
        }

        public void WriteUInt8(uint Address, byte Value)
        {
            int Shift = (int)Address & 3;
            int BitMask = 0xFF << (Shift * 8);

            uint WordAddress = Address - (uint)Shift;
            uint Word = this.ReadUInt32(WordAddress);

            Word &= (uint)(~BitMask);
            Word |= (uint)Value << Shift;

            this.WriteUInt32(WordAddress, Word);
        }

        public string DumpMemoryMap()
        {
            string MapDump = "Memory map:\n";
            foreach(CPUMap MapEntry in BusMap)
            {
                CPUDevice Dev = MapEntry.Device;
                uint StartAddress = MapEntry.Start;

                MapDump += $"{Dev.Name()}@{StartAddress.ToString("X8")}\n";
            }
            return MapDump;
        }
    }
}
