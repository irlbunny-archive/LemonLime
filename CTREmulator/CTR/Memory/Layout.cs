using System;
using System.Collections.Generic;
using System.Text;

namespace CTREmulator.CTR.Memory
{
    class Layout
    {
        private LayoutTypes ProcessorId;

        public Layout(LayoutTypes ProcessorId = LayoutTypes.ARM9)
        {
            this.ProcessorId = ProcessorId;
        }
    }
}
