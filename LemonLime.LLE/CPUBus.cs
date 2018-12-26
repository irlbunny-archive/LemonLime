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
        private class DeviceDescriptor
        {
            public CPUDevice Device;
            public uint Start, End;

            public DeviceDescriptor(CPUDevice Device, uint Start)
            {
                this.Device = Device;
                this.Start = Start;
                this.End = Start + Device.Size();
            }
        }

        private List<DeviceDescriptor> BusMap;

        public CPUBus()
        {
            this.BusMap = new List<DeviceDescriptor>();
        }

        public void Attach(CPUDevice Device, uint Start)
        {
            this.BusMap.Add(new DeviceDescriptor(Device, Start));
        }

        public uint ReadUInt32(uint Address)
        {
            DeviceDescriptor Map = FindMap(Address, 4);
            return Map.Device.ReadWord(Address - Map.Start);
        }

        public ushort ReadUInt16(uint Address)
        {
            DeviceDescriptor Map = FindMap(Address, 2);
            return Map.Device.ReadShort(Address - Map.Start);
        }

        public byte ReadUInt8(uint Address)
        {
            DeviceDescriptor Map = FindMap(Address, 1);
            return Map.Device.ReadByte(Address - Map.Start);
        }

        public void WriteUInt32(uint Address, uint Value)
        {
            DeviceDescriptor Map = FindMap(Address, 4);
            Map.Device.WriteWord(Address - Map.Start, Value);
        }

        public void WriteUInt16(uint Address, ushort Value)
        {
            DeviceDescriptor Map = FindMap(Address, 2);
            Map.Device.WriteShort(Address - Map.Start, Value);
        }

        public void WriteUInt8(uint Address, byte Value)
        {
            DeviceDescriptor Map = FindMap(Address, 1);
            Map.Device.WriteByte(Address - Map.Start, Value);
        }

        public string DumpMemoryMap()
        {
            string MapDump = "Memory map:\n";
            foreach(DeviceDescriptor MapEntry in this.BusMap)
            {
                CPUDevice Dev = MapEntry.Device;
                uint StartAddress = MapEntry.Start;

                MapDump += $"{Dev.Name()}@{StartAddress.ToString("X8")}\n";
            }
            return MapDump;
        }

        private DeviceDescriptor FindMap(uint Address, uint WordSize)
        {
            DeviceDescriptor Map = BusMap.Where(map => Address >= map.Start && (Address + WordSize) <= map.End).SingleOrDefault();
            if (Map == null) throw new Exception($"Unhandled read @ 0x{Address.ToString($"X8")}");
            return Map;
        }
    }
}
