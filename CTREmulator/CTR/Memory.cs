using CTREmulator.ARM;
using System;
using System.Collections.Generic;

namespace CTREmulator.CTR
{
    class Memory : IBus
    {
        private struct MemoryEntry
        {
            public uint             Address;
            public uint             Size;
            public string           DebugName;
            public MemoryType       Type;
        }

        private enum MemoryType
        {
            NONE,
            DATA_TCM,
            BOOTROM_ARM9,
            IO_MEMORY
        }

        private BootROM.ARM9      BootROM9;
        private List<MemoryEntry> MemoryEntries;

        private byte[] DataTCM  = new byte[0x00004000];
        private byte[] IOMemory = new byte[0x08000000];

        public Memory()
        {
            BootROM9  = new BootROM.ARM9();
            InitMemoryEntrys();
        }

        private void InitMemoryEntrys()
        {
            MemoryEntries = new List<MemoryEntry>();

            MemoryEntries.Add(new MemoryEntry
            {
                Address   = 0x00000000,
                Size      = 0x08000000,
                DebugName = "Instruction TCM",
                Type      = MemoryType.NONE
            });

            MemoryEntries.Add(new MemoryEntry
            {
                Address   = 0x01FF8000,
                Size      = 0x00008000,
                DebugName = "Instruction TCM (Kernel & Process)",
                Type      = MemoryType.NONE
            });

            MemoryEntries.Add(new MemoryEntry
            {
                Address   = 0x07FF8000,
                Size      = 0x00008000,
                DebugName = "Instruction TCM (BootROM)",
                Type      = MemoryType.NONE
            });

            MemoryEntries.Add(new MemoryEntry
            {
                Address   = 0x08000000,
                Size      = 0x00100000,
                DebugName = "ARM9 Internal Memory",
                Type      = MemoryType.NONE
            });

            MemoryEntries.Add(new MemoryEntry
            {
                Address   = 0x10000000,
                Size      = 0x08000000,
                DebugName = "IO Memory",
                Type      = MemoryType.NONE
            });

            MemoryEntries.Add(new MemoryEntry
            {
                Address   = 0x18000000,
                Size      = 0x00600000,
                DebugName = "VRAM",
                Type      = MemoryType.NONE
            });

            MemoryEntries.Add(new MemoryEntry
            {
                Address   = 0x20000000,
                Size      = 0x08000000,
                DebugName = "FCRAM",
                Type      = MemoryType.NONE
            });

            MemoryEntries.Add(new MemoryEntry
            {
                Address   = 0xFFF00000,
                Size      = 0x00004000,
                DebugName = "Data TCM (BootROM Mapped)",
                Type    = MemoryType.DATA_TCM
            });

            MemoryEntries.Add(new MemoryEntry
            {
                Address   = 0xFFFF0000,
                Size      = 0x00010000,
                DebugName = "ARM9 BootROM",
                Type    = MemoryType.BOOTROM_ARM9
            });
        }
        
        public byte ReadUInt8(uint Address)
        {

            for (int i = 0; i < MemoryEntries.Count; ++i)
            {
                MemoryEntry entry = MemoryEntries[i];

                if (entry.Address + entry.Size < Address) break;

                if (Util.inRange(Address, entry.Address, (entry.Address + entry.Size) - 1))
                {
                    Logging.WriteInfo($"{entry.DebugName} @ 0x{Address:X}");

                    switch (entry.Type)
                    {
                        case MemoryType.NONE:
                            return 0;
                        case MemoryType.DATA_TCM:
                            return DataTCM[Address - entry.Address];
                        case MemoryType.BOOTROM_ARM9:
                            return BootROM9.ARM9_BootROM[Address - entry.Address];
                        case MemoryType.IO_MEMORY:
                            return IOMemory[Address - entry.Address];
                    }
                }
            }

            Logging.WriteInfo($"Read @ 0x{Address:X}");

            return 0;
        }

        public void WriteUInt8(uint Address, byte Value)
        {
            for (int i = 0; i < MemoryEntries.Count; ++i)
            {
                MemoryEntry entry = MemoryEntries[i];

                if (entry.Address + entry.Size < Address) break;

                if (Util.inRange(Address, entry.Address, (entry.Address + entry.Size) - 1))
                {
                    Logging.WriteInfo($"{entry.DebugName} @ 0x{Address:X}, Value = {Value:X}");

                    switch (entry.Type)
                    {
                        case MemoryType.NONE:
                            return;
                        case MemoryType.DATA_TCM:
                            DataTCM[Address - entry.Address] = Value;
                            return;
                        case MemoryType.BOOTROM_ARM9:
                            return;
                        case MemoryType.IO_MEMORY:
                            IOMemory[Address - entry.Address] = Value;
                            return;
                    }
                }
            }

            Logging.WriteInfo($"Write @ 0x{Address:X}, Value = {Value:X}");
        }
    }
}
