using System;
using System.Collections.Generic;
using System.Text;

namespace CTREmulator.CTR
{
    public class IORegisters
    {
        // CONFIG9
        public static uint CFG9_SYSPROT9    = 0x0;
        public static uint CFG9_SYSPROT11   = 0x1;
        public static uint CFG9_RST11       = 0x2;
        public static uint CFG9_DEBUGCTL    = 0x4;
        public static uint CFG9_UNKNOWN5    = 0x8; // Unknown what this is, but it is set to 3 after writing to AES_CTL
        public static uint CFG9_CARDCTL     = 0xC;
        public static uint CFG9_CARDSTATUS  = 0x10;
        public static uint CFG9_CARDCYCLES0 = 0x12;
        public static uint CFG9_CARDCYCLES1 = 0x14;
        public static uint CFG9_SDMMCCTL    = 0x20;
        public static uint CFG9_UNKNOWN11   = 0x100;
        public static uint CFG9_EXTMEMCNT9  = 0x200;
        public static uint CFG9_MPCORECFG   = 0xFFC;
        public static uint CFG9_BOOTENV     = 0x10000;
        public static uint CFG9_UNITINFO    = 0x10010;
        public static uint CFG9_TWLUNITINFO = 0x10014;

        // IRQ
        public static uint IRQ_IE = 0x1000;
        public static uint IRQ_IF = 0x1004;

        // NDMA
        public static uint NDMA_GLOBAL_CNT   = 0x2000;
        public static uint NDMA_SRC_ADDR     = 0x2004;
        public static uint NDMA_DST_ADDR     = 0x2008;
        public static uint NDMA_TRANSFER_CNT = 0x200C;
        public static uint NDMA_WRITE_CNT    = 0x2010;
        public static uint NDMA_BLOCK_CNT    = 0x2014;
        public static uint NDMA_FILL_DATA    = 0x2018;
        public static uint NDMA_CNT          = 0x201C;

        // Timer
        public static uint TIMER_VAL = 0x3000;
        public static uint TIMER_CNT = 0x3002;

        // CTRCARD
        public static uint CTRCARD_CNT     = 0x4000;
        public static uint CTRCARD_BLKCNT  = 0x4004;
        public static uint CTRCARD_SECCNT  = 0x4008;
        public static uint CTRCARD_SECSEED = 0x4010;
        public static uint CTRCARD_CMD     = 0x4020;
        public static uint CTRCARD_FIFO    = 0x4030;
    }
}
