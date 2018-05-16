using System;
using System.Collections.Generic;
using System.Text;

namespace CTREmulator
{
    /// <summary>
    /// Minimal ARM interpreter
    /// NOTE: Doesn't work
    /// </summary>
    class ARMInterpreter
    {
        [Flags]
        public enum CPSR : uint
        {
            Negative  = ((uint)1 << 31),
            Zero      = ((uint)1 << 30),
            Carry     = ((uint)1 << 29),
            Underflow = ((uint)1 << 28),
            IRQDis    = ((uint)1 << 7),
            FIQDis    = ((uint)1 << 6),
            Thumb     = ((uint)1 << 5),
            Mode      = 0x1F,
        }

        public uint[] _R;
        public CPSR _CPSR;

        public Queue<uint> _INST_BUF;

        public ARMInterpreter()
        {
            _R = new uint[16];
            _CPSR = 0;
            _INST_BUF = new Queue<uint>();
        }

        public void ProcessInstruction(uint _INST)
        {
            byte cond = (byte)(_INST >> 28);
            if ((cond & 8) == 0)
                switch (cond >> 1)
                {
                    case 0: if (_CPSR.HasFlag(CPSR.Zero) == ((cond & 1) != 0)) return; break;
                    case 1: if (_CPSR.HasFlag(CPSR.Carry) == ((cond & 1) != 0)) return; break;
                    case 2: if (_CPSR.HasFlag(CPSR.Negative) == ((cond & 1) != 0)) return; break;
                    case 3: if (_CPSR.HasFlag(CPSR.Underflow) == ((cond & 1) != 0)) return; break;
                }
            else
            {
                switch (cond & 7)
                {
                    case 0: if (_CPSR.HasFlag(CPSR.Carry) && !_CPSR.HasFlag(CPSR.Zero)) break; return;
                    case 1: if (!_CPSR.HasFlag(CPSR.Carry) && _CPSR.HasFlag(CPSR.Zero)) break; return;
                    case 2: if (_CPSR.HasFlag(CPSR.Negative) == _CPSR.HasFlag(CPSR.Underflow)) break; return;
                    case 3: if (_CPSR.HasFlag(CPSR.Negative) != _CPSR.HasFlag(CPSR.Underflow)) break; return;
                    case 4: if (_CPSR.HasFlag(CPSR.Negative) == _CPSR.HasFlag(CPSR.Underflow) && !_CPSR.HasFlag(CPSR.Zero)) break; return;
                    case 5: if (_CPSR.HasFlag(CPSR.Negative) != _CPSR.HasFlag(CPSR.Underflow) && _CPSR.HasFlag(CPSR.Zero)) break; return;
                    case 6: break;
                    case 7: return;
                }
            }
        }
    }
}
