using CTREmulator.ARM;
using CTREmulator.CTR.Memory;
using System.Threading;

namespace CTREmulator.CTR
{
    class Core
    {
        public Memory.Memory Memory;
        public ARMInterpreter ARM11;
        public ARMInterpreter ARM9;
        public MemoryLayout Layout11;
        public MemoryLayout Layout9;

        private bool IsExecuting;

        public Core()
        {
            // we need to load the bootrom into arm11/9 memory?
            Memory = new Memory.Memory();
            Layout11 = new MemoryLayout(Memory, LayoutTypes.ARM11);
            Layout9 = new MemoryLayout(Memory, LayoutTypes.ARM9);
            ARM11 = new ARMInterpreter(Layout11); // actually, no. we need to implement the MMU?
            ARM9 = new ARMInterpreter(Layout9);
            ARM11.Reset();
            ARM9.Reset();
        }

        public void Start()
        {
            Thread ExecutionThread = new Thread(Run);
            ExecutionThread.Start();
        }

        private void Run()
        {
            IsExecuting = true;
            while (IsExecuting)
            {
                ARM11.Execute();
                ARM9.Execute();
            }
        }

        public void Stop()
        {
            IsExecuting = false;
        }
    }
}
