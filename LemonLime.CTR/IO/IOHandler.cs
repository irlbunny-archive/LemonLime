using System;
using System.Collections.Generic;
using LemonLime.ARM;

namespace LemonLime.CTR.IO
{
    partial class IOHandler
    {
        private delegate byte IOFunc(Interpreter CPU);

        private Dictionary<uint, IOFunc> IOFuncs;

        private Interpreter CPU;

        public IOHandler(Interpreter CPU)
        {
            IOFuncs = new Dictionary<uint, IOFunc>()
            {
                { 0x10000002, CFG9_RST11 }
            };

            this.CPU = CPU;
        }

        public byte Call(uint Address)
        {
            if (IOFuncs.TryGetValue(Address, out IOFunc Func))
            {
                Func(CPU);
            }
            else
            {
                throw new NotImplementedException(Address.ToString("X4"));
            }

            return 0;
        }
    }
}
