using System;
using System.Collections.Generic;
using System.Linq;

using LemonLime.ARM;

namespace LemonLime.LLE.CPU
{
    class Bus : IBus
    {
        private class DeviceDescriptor
        {
            public CPU.Device Device;
            public uint Start, End;

            public DeviceDescriptor(CPU.Device Device, uint Start)
            {
                this.Device = Device;
                this.Start  = Start;
                this.End    = Start + Device.Size();
            }
        }

        private List<DeviceDescriptor> BusMap;

        public Bus()
        {
            this.BusMap = new List<DeviceDescriptor>();
        }

        public void Attach(CPU.Device Device, uint Start)
        {
            this.BusMap.Add(new DeviceDescriptor(Device, Start));
        }

        public uint ReadUInt32(uint Address)
        {
            DeviceDescriptor Map = GetMap(Address, 4);
            return Map.Device.ReadWord(Address - Map.Start);
        }

        public ushort ReadUInt16(uint Address)
        {
            DeviceDescriptor Map = GetMap(Address, 2);
            return Map.Device.ReadShort(Address - Map.Start);
        }

        public byte ReadUInt8(uint Address)
        {
            DeviceDescriptor Map = GetMap(Address, 1);
            return Map.Device.ReadByte(Address - Map.Start);
        }

        public void WriteUInt32(uint Address, uint Value)
        {
            DeviceDescriptor Map = GetMap(Address, 4);
            Map.Device.WriteWord(Address - Map.Start, Value);
        }

        public void WriteUInt16(uint Address, ushort Value)
        {
            DeviceDescriptor Map = GetMap(Address, 2);
            Map.Device.WriteShort(Address - Map.Start, Value);
        }

        public void WriteUInt8(uint Address, byte Value)
        {
            DeviceDescriptor Map = GetMap(Address, 1);
            Map.Device.WriteByte(Address - Map.Start, Value);
        }

        public string GetMemoryMaps()
        {
            string MapsString = "Memory Maps:\n";
            foreach (DeviceDescriptor MapEntry in this.BusMap)
            {
                CPU.Device Dev          = MapEntry.Device;
                uint       StartAddress = MapEntry.Start;

                MapsString += $"{Dev.Name()}@{StartAddress.ToString("X8")}\n";
            }

            return MapsString;
        }

        private DeviceDescriptor GetMap(uint Address, uint WordSize)
        {
            DeviceDescriptor Map = BusMap.Where(_Map => Address >= _Map.Start && (Address + WordSize) <= _Map.End).SingleOrDefault();
            if (Map != null) return Map;

            throw new Exception($"Unhandled address @ 0x{Address.ToString($"X8")}");
        }
    }
}
