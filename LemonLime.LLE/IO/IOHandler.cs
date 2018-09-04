using LemonLime.Common;
using LemonLime.LLE.IO.Registers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LemonLime.LLE.IO
{
    class IOHandler
    {
        private List<IOEntry> Entries;

        public IOHandler()
        {
            Entries = new List<IOEntry>
            {
                // CFG9
                new IOEntry(CFG9.CFG9_RST11,    0x10000002),
                new IOEntry(CFG9.CFG9_UNITINFO, 0x10010010),

                // IRQ
                new IOEntry(IRQ.IRQ_IE, 0x10001000),
                new IOEntry(IRQ.IRQ_IF, 0x10001004),

                // NDMA
                new IOEntry(NDMA.NDMA_GLOBAL_CNT, 0x10002000),

                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (0 * 0x1C)), // Channel 1
                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (1 * 0x1C)), // Channel 2
                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (2 * 0x1C)), // Channel 3
                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (3 * 0x1C)), // Channel 4
                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (4 * 0x1C)), // Channel 5
                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (5 * 0x1C)), // Channel 6
                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (6 * 0x1C)), // Channel 7
                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (7 * 0x1C)), // Channel 8

                // PXI
                new IOEntry(PXI.PXI_SYNC, 0x10008000),
                new IOEntry(PXI.PXI_CNT,  0x10008004),

                // CDMA
                new IOEntry(CDMA.CDMA_UNKNOWN, 0x1000CD00 + (0 * 4)),
                new IOEntry(CDMA.CDMA_UNKNOWN, 0x1000CD00 + (1 * 4)),
                new IOEntry(CDMA.CDMA_UNKNOWN, 0x1000CD00 + (2 * 4)),
                new IOEntry(CDMA.CDMA_UNKNOWN, 0x1000CD00 + (3 * 4)),
                new IOEntry(CDMA.CDMA_UNKNOWN, 0x1000CD00 + (4 * 4)),

                // XDMA
                new IOEntry(XDMA.XDMA_UNKNOWN, 0x1000C020 + (0 * 4)),
                new IOEntry(XDMA.XDMA_UNKNOWN, 0x1000C020 + (1 * 4)),
                new IOEntry(XDMA.XDMA_UNKNOWN, 0x1000C020 + (2 * 4)),
                new IOEntry(XDMA.XDMA_UNKNOWN, 0x1000C020 + (3 * 4)),
                new IOEntry(XDMA.XDMA_UNKNOWN, 0x1000C020 + (4 * 4)),
                new IOEntry(XDMA.XDMA_UNKNOWN, 0x1000C100),
                
                // TIMER
                new IOEntry(TIMER.TIMER_VAL, 0x10003000 + 4 * 0), // Timer 1
                new IOEntry(TIMER.TIMER_VAL, 0x10003000 + 4 * 1), // Timer 2
                new IOEntry(TIMER.TIMER_VAL, 0x10003000 + 4 * 2), // Timer 3
                new IOEntry(TIMER.TIMER_VAL, 0x10003000 + 4 * 3), // Timer 4

                new IOEntry(TIMER.TIMER_CNT, 0x10003002 + 4 * 0), // Timer 1
                new IOEntry(TIMER.TIMER_CNT, 0x10003002 + 4 * 1), // Timer 2
                new IOEntry(TIMER.TIMER_CNT, 0x10003002 + 4 * 2), // Timer 3
                new IOEntry(TIMER.TIMER_CNT, 0x10003002 + 4 * 3), // Timer 4

                // HID
                new IOEntry(HID.HID_PAD, 0x10146000),
            };
        }

        public void Call(IOData Data)
        {
            switch (Data.Type)
            {
                case IOType.Read:
                    switch (Data.Width)
                    {
                        case IOWidth.Width1:
                            Logger.WriteInfo($"IO [{Data.Address.ToString("X")}]");
                            break;
                        case IOWidth.Width2:
                            Logger.WriteInfo($"IO [{Data.Address.ToString("X")}]");
                            break;
                        case IOWidth.Width4:
                            Logger.WriteInfo($"IO [{Data.Address.ToString("X")}]");
                            break;
                    }
                    break;

                case IOType.Write:
                    switch (Data.Width)
                    {
                        case IOWidth.Width1:
                            Logger.WriteInfo($"IO [{Data.Address.ToString("X")}], Data = {Data.Write8.ToString("X2")}");
                            break;
                        case IOWidth.Width2:
                            Logger.WriteInfo($"IO [{Data.Address.ToString("X")}], Data = {Data.Write16.ToString("X4")}");
                            break;
                        case IOWidth.Width4:
                            Logger.WriteInfo($"IO [{Data.Address.ToString("X")}], Data = {Data.Write32.ToString("X8")}");
                            break;
                    }
                    break;
            }

            IOEntry EntryForAddr = Entries.Where(Entry => Entry.Address == Data.Address).FirstOrDefault();

            if (EntryForAddr == null) throw new Exception($"Unhandled {Data.Type} ({Data.Width}) @ {Data.Address.ToString($"X")}");

            EntryForAddr.Register(Data);
        }
    }
}
