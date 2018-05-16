using System;
using System.Collections.Generic;
using System.Text;

namespace CTREmulator.CTR.IODevices.GPU
{
    class PICA200
    {
        // Top Left
        private const uint PICA_FB0_TopLeft_StartAddr  = 0x1E6000;
        private const uint PICA_FB0_TopLeft_EndAddr    = 0x22C500;
        private const uint PICA_FB1_TopLeft_StartAddr  = 0x273000;
        private const uint PICA_FB1_TopLeft_EndAddr    = 0x273000;

        // Top Right
        private const uint PICA_FB0_TopRight_StartAddr = 0x22C800;
        private const uint PICA_FB0_TopRight_EndAddr   = 0x272D00;
        private const uint PICA_FB1_TopRight_StartAddr = 0x2B9800;
        private const uint PICA_FB1_TopRight_EndAddr   = 0x2FFD00;

        // Bottom
        private const uint PICA_FB0_Bottom_StartAddr   = 0x48F000;
        private const uint PICA_FB0_Bottom_EndAddr     = 0x4C7400;
        private const uint PICA_FB1_Bottom_StartAddr   = 0x4C7800;
        private const uint PICA_FB1_Bottom_EndAddr     = 0x4FF800;
    }
}
