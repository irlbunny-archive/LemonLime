using LemonLime.Common;
using LemonLime.CTR.IO.Registers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LemonLime.CTR.IO
{
    class IOHandler
    {
        private List<IOEntry> Entries;

        public IOHandler()
        {
            // Our IO entries
            Entries = new List<IOEntry>
            {
                new IOEntry(CFG9.CFG9_RST11,      0x10000002, 1),
                new IOEntry(TIMER.TIMER_1000300E, 0x1000300E, 1)
            };
        }

        public byte Call(IOData Data)
        {
            // Log IO calls
            switch (Data.Type)
            {
                case IOType.Read:
                    switch (Data.Width)
                    {
                        case 1:
                            Logger.WriteInfo($"IO ({Data.Width}) [{Data.Address.ToString("X")}] (Called)");
                            break;
                        case 2:
                            Logger.WriteInfo($"IO ({Data.Width}) [{Data.Address.ToString("X")}] (Called)");
                            break;
                        case 4:
                            Logger.WriteInfo($"IO ({Data.Width}) [{Data.Address.ToString("X")}] (Called)");
                            break;
                    }
                    break;

                case IOType.Write:
                    switch (Data.Width)
                    {
                        case 1:
                            Logger.WriteInfo($"IO ({Data.Width}) [{Data.Address.ToString("X")}] (Called), Data = {Data.Data8.ToString("X2")}");
                            break;
                        case 2:
                            Logger.WriteInfo($"IO ({Data.Width}) [{Data.Address.ToString("X")}] (Called), Data = {Data.Data8.ToString("X4")}");
                            break;
                        case 4:
                            Logger.WriteInfo($"IO ({Data.Width}) [{Data.Address.ToString("X")}] (Called), Data = {Data.Data8.ToString("X8")}");
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
                    case 1:
                        throw new Exception(Data.Address.ToString("X"));
                    case 2:
                        throw new Exception(Data.Address.ToString("X"));
                    case 4:
                        throw new Exception(Data.Address.ToString("X"));
                }
            }

            // Check width
            if (Data.Width != EntryForAddr.Width) return 1;

            switch (Data.Type)
            {
                // Return data from register
                case IOType.Read:
                    return EntryForAddr.Register(Data);

                // Write doesn't return anything from register
                case IOType.Write:
                    EntryForAddr.Register(Data);
                    return 0;
            }

            // This shouldn't happen(?)
            return 0;
        }
    }
}
