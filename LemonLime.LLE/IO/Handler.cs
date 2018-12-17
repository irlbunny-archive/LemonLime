using LemonLime.Common;
using LemonLime.LLE.IO.Registers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LemonLime.LLE.IO
{
    class Handler
    {
        private List<Entry> Entries;

        public Handler()
        {
            Entries = new List<Entry>
            {
                // CFG9
                new Entry(CFG9.CFG9_RST11,    0x10000002),
                new Entry(CFG9.CFG9_UNITINFO, 0x10010010),

                // IRQ
                new Entry(IRQ.IRQ_IE, 0x10001000),
                new Entry(IRQ.IRQ_IF, 0x10001004),

                // NDMA
                new Entry(NDMA.NDMA_GLOBAL_CNT, 0x10002000),

                new Entry(NDMA.NDMA_CNT, 0x1000201C + (0 * 0x1C)), // Channel 1
                new Entry(NDMA.NDMA_CNT, 0x1000201C + (1 * 0x1C)), // Channel 2
                new Entry(NDMA.NDMA_CNT, 0x1000201C + (2 * 0x1C)), // Channel 3
                new Entry(NDMA.NDMA_CNT, 0x1000201C + (3 * 0x1C)), // Channel 4
                new Entry(NDMA.NDMA_CNT, 0x1000201C + (4 * 0x1C)), // Channel 5
                new Entry(NDMA.NDMA_CNT, 0x1000201C + (5 * 0x1C)), // Channel 6
                new Entry(NDMA.NDMA_CNT, 0x1000201C + (6 * 0x1C)), // Channel 7
                new Entry(NDMA.NDMA_CNT, 0x1000201C + (7 * 0x1C)), // Channel 8

                // PXI
                new Entry(PXI.PXI_SYNC, 0x10008000),
                new Entry(PXI.PXI_CNT,  0x10008004),

                // CDMA
                new Entry(CDMA.CDMA_UNKNOWN, 0x1000CD00 + (0 * 4)),
                new Entry(CDMA.CDMA_UNKNOWN, 0x1000CD00 + (1 * 4)),
                new Entry(CDMA.CDMA_UNKNOWN, 0x1000CD00 + (2 * 4)),
                new Entry(CDMA.CDMA_UNKNOWN, 0x1000CD00 + (3 * 4)),
                new Entry(CDMA.CDMA_UNKNOWN, 0x1000CD00 + (4 * 4)),

                // XDMA
                new Entry(XDMA.XDMA_UNKNOWN, 0x1000C020 + (0 * 4)),
                new Entry(XDMA.XDMA_UNKNOWN, 0x1000C020 + (1 * 4)),
                new Entry(XDMA.XDMA_UNKNOWN, 0x1000C020 + (2 * 4)),
                new Entry(XDMA.XDMA_UNKNOWN, 0x1000C020 + (3 * 4)),
                new Entry(XDMA.XDMA_UNKNOWN, 0x1000C020 + (4 * 4)),
                new Entry(XDMA.XDMA_UNKNOWN, 0x1000C100),
                
                // TIMER
                new Entry(TIMER.TIMER_VAL, 0x10003000 + 4 * 0), // Timer 1
                new Entry(TIMER.TIMER_VAL, 0x10003000 + 4 * 1), // Timer 2
                new Entry(TIMER.TIMER_VAL, 0x10003000 + 4 * 2), // Timer 3
                new Entry(TIMER.TIMER_VAL, 0x10003000 + 4 * 3), // Timer 4

                new Entry(TIMER.TIMER_CNT, 0x10003002 + 4 * 0), // Timer 1
                new Entry(TIMER.TIMER_CNT, 0x10003002 + 4 * 1), // Timer 2
                new Entry(TIMER.TIMER_CNT, 0x10003002 + 4 * 2), // Timer 3
                new Entry(TIMER.TIMER_CNT, 0x10003002 + 4 * 3), // Timer 4

                // HID
                new Entry(HID.HID_PAD, 0x10146000),
            };
        }

        public uint Call(Context Ctx)
        {
            switch (Ctx.Type)
            {
                case Type.Read:
                    switch (Ctx.Width)
                    {
                        case Width.Width1:
                            Logger.WriteInfo($"IO [{Ctx.Address.ToString("X")}]");
                            break;

                        case Width.Width2:
                            Logger.WriteInfo($"IO [{Ctx.Address.ToString("X")}]");
                            break;

                        case Width.Width4:
                            Logger.WriteInfo($"IO [{Ctx.Address.ToString("X")}]");
                            break;
                    }
                    break;

                case Type.Write:
                    switch (Ctx.Width)
                    {
                        case Width.Width1:
                            Logger.WriteInfo($"IO [{Ctx.Address.ToString("X")}], Data = {Ctx.GetInputUInt8().ToString("X2")}");
                            break;

                        case Width.Width2:
                            Logger.WriteInfo($"IO [{Ctx.Address.ToString("X")}], Data = {Ctx.GetInputUInt16().ToString("X4")}");
                            break;

                        case Width.Width4:
                            Logger.WriteInfo($"IO [{Ctx.Address.ToString("X")}], Data = {Ctx.GetInputUInt32().ToString("X8")}");
                            break;
                    }
                    break;
            }

            Entry RegisterEntry = Entries.Where(Entry => Entry.Address == Ctx.Address).FirstOrDefault();
            if (RegisterEntry != null)
            {
                RegisterEntry.Register(Ctx);
                return Ctx.GetOutputUInt32();
            }

            throw new Exception($"Unhandled {Ctx.Type} ({Ctx.Width}) @ {Ctx.Address.ToString($"X")}");
        }
    }
}
