using LemonLime.Common;
using LemonLime.CTR.IO.Registers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LemonLime.CTR.IO
{
    class IOHandler
    {
        // IO Entries
        private List<IOEntry> Entries;

        public IOHandler()
        {
            // Our IO entries
            Entries = new List<IOEntry>
            {
                // CFG9
                new IOEntry(CFG9.CFG9_RST11, 0x10000002, 1),

                // IRQ
                new IOEntry(IRQ.IRQ_IE, 0x10001000, 4),
                new IOEntry(IRQ.IRQ_IF, 0x10001004, 4),

                // NDMA
                new IOEntry(NDMA.NDMA_GLOBAL_CNT, 0x10002000, 4),

                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (0 * 0x1C), 4), // Channel 1
                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (1 * 0x1C), 4), // Channel 2
                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (2 * 0x1C), 4), // Channel 3
                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (3 * 0x1C), 4), // Channel 4
                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (4 * 0x1C), 4), // Channel 5
                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (5 * 0x1C), 4), // Channel 6
                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (6 * 0x1C), 4), // Channel 7
                new IOEntry(NDMA.NDMA_CNT, 0x1000201C + (7 * 0x1C), 4), // Channel 8

                // PXI
                new IOEntry(PXI.PXI_SYNC, 0x10008000, 4),
                new IOEntry(PXI.PXI_CNT,  0x10008004, 2),

                // CDMA
                new IOEntry(CDMA.CDMA_UNKNOWN, 0x1000CD00 + (0 * 4), 4),
                new IOEntry(CDMA.CDMA_UNKNOWN, 0x1000CD00 + (1 * 4), 4),
                new IOEntry(CDMA.CDMA_UNKNOWN, 0x1000CD00 + (2 * 4), 4),
                new IOEntry(CDMA.CDMA_UNKNOWN, 0x1000CD00 + (3 * 4), 4),
                new IOEntry(CDMA.CDMA_UNKNOWN, 0x1000CD00 + (4 * 4), 4),

                // XDMA
                new IOEntry(XDMA.XDMA_UNKNOWN, 0x1000C020 + (0 * 4), 4),
                new IOEntry(XDMA.XDMA_UNKNOWN, 0x1000C020 + (1 * 4), 4),
                new IOEntry(XDMA.XDMA_UNKNOWN, 0x1000C020 + (2 * 4), 4),
                new IOEntry(XDMA.XDMA_UNKNOWN, 0x1000C020 + (3 * 4), 4),
                new IOEntry(XDMA.XDMA_UNKNOWN, 0x1000C020 + (4 * 4), 4),
                new IOEntry(XDMA.XDMA_UNKNOWN, 0x1000C100, 4),
                
                // TIMER
                new IOEntry(TIMER.TIMER_VAL, 0x10003000 + 4 * 0, 2), // Timer 1
                new IOEntry(TIMER.TIMER_VAL, 0x10003000 + 4 * 1, 2), // Timer 2
                new IOEntry(TIMER.TIMER_VAL, 0x10003000 + 4 * 2, 2), // Timer 3
                new IOEntry(TIMER.TIMER_VAL, 0x10003000 + 4 * 3, 2), // Timer 4

                new IOEntry(TIMER.TIMER_CNT, 0x10003002 + 4 * 0, 2), // Timer 1
                new IOEntry(TIMER.TIMER_CNT, 0x10003002 + 4 * 1, 2), // Timer 2
                new IOEntry(TIMER.TIMER_CNT, 0x10003002 + 4 * 2, 2), // Timer 3
                new IOEntry(TIMER.TIMER_CNT, 0x10003002 + 4 * 3, 2), // Timer 4

                // HID
                new IOEntry(HID.HID_PAD, 0x10146000, 2),
            };
        }

        public void Call(IOData Data)
        {
            // Log IO calls
            switch (Data.Type)
            {
                // Read log
                case IOType.Read:
                    switch (Data.Width)
                    {
                        // Width 1
                        case 1:
                            Logger.WriteInfo($"IO ({Data.Width}) [{Data.Address.ToString("X")}]");
                            break;

                        // Width 2
                        case 2:
                            Logger.WriteInfo($"IO ({Data.Width}) [{Data.Address.ToString("X")}]");
                            break;

                        // Width 4
                        case 4:
                            Logger.WriteInfo($"IO ({Data.Width}) [{Data.Address.ToString("X")}]");
                            break;
                    }
                    break;

                // Write log
                case IOType.Write:
                    switch (Data.Width)
                    {
                        // Width 1
                        case 1:
                            Logger.WriteInfo($"IO ({Data.Width}) [{Data.Address.ToString("X")}], Data = {Data.Write8.ToString("X2")}");
                            break;

                        // Width 2
                        case 2:
                            Logger.WriteInfo($"IO ({Data.Width}) [{Data.Address.ToString("X")}], Data = {Data.Write8.ToString("X4")}");
                            break;

                        // Width 4
                        case 4:
                            Logger.WriteInfo($"IO ({Data.Width}) [{Data.Address.ToString("X")}], Data = {Data.Write8.ToString("X8")}");
                            break;
                    }
                    break;
            }

            // Get IOEntry for address
            IOEntry EntryForAddr = Entries.Where(Entry => Entry.Address == Data.Address).FirstOrDefault();

            // Check if IOEntry is null
            if (EntryForAddr == null)
            {
                // Format address hex
                switch (Data.Width)
                {
                    // Width 1
                    case 1:
                        throw new Exception(Data.Address.ToString("X"));

                    // Width 2
                    case 2:
                        throw new Exception(Data.Address.ToString("X"));

                    // Width 4
                    case 4:
                        throw new Exception(Data.Address.ToString("X"));
                }
            }

            // Check width
            if (Data.Width != EntryForAddr.Width) return;

            // Execute register
            EntryForAddr.Register(Data);
        }
    }
}
