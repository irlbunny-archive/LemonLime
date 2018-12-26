using System;
using System.IO;
using System.Collections.Generic;

using LemonLime.ARM;
using LemonLime.Common;

namespace LemonLime.LLE.Device.Generic
{
    enum PXI_CNT_Masks {
        SEND_FIFO_EMPTY = 0x01,
        SEND_FIFO_FULL = 0x02,
        SEND_FIFO_EMPTY_IRQ = 0x04,
        SEND_FIFO_CLEAR = 0x08,

        RECV_FIFO_EMPTY = 0x100,
        RECV_FIFO_FULL = 0x200,
        RECV_FIFO_NOT_EMPTY_IRQ = 0x400,

        FIFO_ERROR = 0x4000,
        FIFO_ENABLE = 0x8000
    }

    public class PXI : CPUDevice
    {
        private Queue<uint> SendFIFO, RecvFIFO;
        private FastMemoryBuffer ConfigBuffer;
        private uint InterruptBitMask;

        private byte RemoteByte;
        private bool SyncIRQEnable, FIFOEnable, FIFOError, SendEmptyIRQ, RecvNotEmptyIRQ;

        private PXI Endpoint;

        public PXI(uint InterruptBitPosition)
        {
            this.SendFIFO = new Queue<uint> (16);
            this.RecvFIFO = new Queue<uint> (16);
            this.ConfigBuffer = new FastMemoryBuffer(8);
            this.InterruptBitMask = (uint)(1 << (int)InterruptBitPosition);

            this.RemoteByte = 0;
            this.SyncIRQEnable = false;
            this.FIFOEnable = false;
            this.FIFOError = false;
            this.SendEmptyIRQ = false;
            this.RecvNotEmptyIRQ = false;

            this.Endpoint = null;
        }

        public void AttachEndpoint(PXI Endpoint)
        {
            this.Endpoint = Endpoint;
        }

        public byte Remote()
        {
            return this.RemoteByte;
        }

        public void TriggerSync()
        {
            return;
        }

        public uint ReadWord(uint Offset)
        {
            Config_UpdateRead();
            switch(Offset) {
                case 0x0:
                case 0x4:
                    return this.ConfigBuffer.ReadWord(Offset);

                case 0xC:
                    try {
                        return this.Endpoint.SendWord();
                    } catch(InvalidOperationException e) {
                        this.FIFOError = true;
                        return 0xFFFFFFFF;
                    }

                case 0x8:
                default:
                    return 0xFFFFFFFF;
            }
        }

        public ushort ReadShort(uint Offset)
        {
            Config_UpdateRead();
            if (Offset < 0x6)
                return this.ConfigBuffer.ReadShort(Offset);
            return 0xFFFF;
        }

        public byte ReadByte(uint Offset)
        {
            Config_UpdateRead();
            if (Offset < 0x6)
                return this.ConfigBuffer.ReadByte(Offset);
            return 0xFF;
        }

        public void WriteWord(uint Offset, uint Value)
        {
            if (Offset < 0x08) {
                Config_UpdateWrite(Offset, 4, new byte[]{(byte)(Value >> 24), (byte)(Value >> 16), (byte)(Value >> 8), (byte)Value});
            } else if (Offset == 0x0C) {
                this.SendFIFO.Enqueue(Value);
            }            
        }

        public void WriteShort(uint Offset, ushort Value)
        {
            Config_UpdateWrite(Offset, 2, new byte[]{(byte)(Value >> 8), (byte)Value});
        }

        public void WriteByte(uint Offset, byte Value)
        {
            Config_UpdateWrite(Offset, 1, new byte[]{Value});
        }

        public uint Size()
        {
            return 16;
        }

        public String Name()
        {
            return "PXI";
        }

        public uint SendWord()
        {
            if (this.SendFIFO.Count > 0) {
                return this.SendFIFO.Dequeue();
            } else {
                throw new InvalidOperationException("Send FIFO is already empty");
            }
        }

        private void Config_UpdateRead()
        {
            uint SyncReg = 0;
            SyncReg |= (uint)this.Endpoint.Remote();
            SyncReg |= (uint)this.RemoteByte << 8;
            SyncReg |= (uint)(this.SyncIRQEnable ? 1 : 0) << 31;

            this.ConfigBuffer.WriteWord(0, SyncReg);


            ushort NewControlReg = 0;
            NewControlReg |= (ushort)((this.SendFIFO.Count == 0) ? PXI_CNT_Masks.SEND_FIFO_EMPTY : 0);
            NewControlReg |= (ushort)((this.SendFIFO.Count == 16) ? PXI_CNT_Masks.SEND_FIFO_FULL : 0);
            NewControlReg |= (ushort)(this.SendEmptyIRQ ? PXI_CNT_Masks.SEND_FIFO_EMPTY_IRQ : 0);

            NewControlReg |= (ushort)((this.RecvFIFO.Count == 0) ? PXI_CNT_Masks.RECV_FIFO_EMPTY : 0);
            NewControlReg |= (ushort)((this.RecvFIFO.Count == 16) ? PXI_CNT_Masks.RECV_FIFO_FULL : 0);
            NewControlReg |= (ushort)(this.RecvNotEmptyIRQ ? PXI_CNT_Masks.RECV_FIFO_NOT_EMPTY_IRQ : 0);

            NewControlReg |= (ushort)(this.FIFOError ? PXI_CNT_Masks.FIFO_ERROR : 0);
            NewControlReg |= (ushort)(this.FIFOEnable ? PXI_CNT_Masks.FIFO_ENABLE : 0);

            this.ConfigBuffer.WriteShort(4, NewControlReg);
        }

        private void Config_UpdateWrite(uint Offset, uint Size, byte[] DataBuffer)
        {
            for (uint i = 0; i < Size; i++) {
                uint Data = (uint)DataBuffer[i];

                switch(Offset + i) {
                    case 0: // Remote byte sent from the other device
                    case 2: // Unused / undocumented byte
                    default:
                        break;

                    case 1: // Remote byte sent to the other device
                        this.RemoteByte = (byte)Data;
                        break;

                    case 3: // Sync configuration
                        this.SyncIRQEnable = ((Data & (uint)0x80) != 0) ? true : false;
                        if ((Data & this.InterruptBitMask) != 0) this.Endpoint.TriggerSync();
                        break;

                    case 4: // Lower half Control Register
                        this.SendEmptyIRQ = ((Data & (uint)PXI_CNT_Masks.SEND_FIFO_EMPTY_IRQ) != 0) ? true : false;
                        if ((Data & 0x08) != 0)
                             this.SendFIFO.Clear();
                        break;

                    case 5: // Upper half Control Register
                        Data <<= 8;
                        this.RecvNotEmptyIRQ = ((Data & (uint)PXI_CNT_Masks.RECV_FIFO_NOT_EMPTY_IRQ) != 0) ? true : false;
                        this.FIFOEnable = ((Data & (uint)PXI_CNT_Masks.FIFO_ENABLE) != 0) ? true : false;
                        this.FIFOError &= ((Data & (uint)PXI_CNT_Masks.FIFO_ERROR) != 0) ? false : true;
                        break;
                }
            }
        }
    }
}
