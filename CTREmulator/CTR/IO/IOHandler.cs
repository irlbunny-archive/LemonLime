using System;
using System.Collections.Generic;
using CTREmulator.ARM;

namespace CTREmulator.CTR.IO
{
    partial class IOHandler
    {
        private delegate byte ReadIOFunc (Interpreter CPU);
        private delegate void WriteIOFunc(Interpreter CPU, byte Value);

        struct IOCallbacks
        {
            public ReadIOFunc  Read;
            public WriteIOFunc Write;
        }

        private Dictionary<uint, IOCallbacks> IOFuncs;

        private Interpreter CPU;

        public IOHandler(Interpreter CPU)
        {
            IOFuncs = new Dictionary<uint, IOCallbacks>()
            {
                { 0x10000002, new IOCallbacks { Read = CFG9_RST11_READ, Write = CFG9_RST11_WRITE } }
            };

            this.CPU = CPU;
        }

        public byte Read(uint Address)
        {
            if (IOFuncs.TryGetValue(Address, out IOCallbacks Funcs))
            {
                Funcs.Read(CPU);
            }
            else
            {
                throw new NotImplementedException(Address.ToString("X4"));
            }

            return 0;
        }

        public void Write(uint Address, byte Value)
        {
            if (IOFuncs.TryGetValue(Address, out IOCallbacks Funcs))
            {
                Funcs.Write(CPU, Value);
            }
            else
            {
                throw new NotImplementedException(Address.ToString("X4"));
            }
        }
    }
}
