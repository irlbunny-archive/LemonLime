using LemonLime.ARM;
using System.Threading;

namespace LemonLime.CTR
{
    public class Core
    {
        public Interpreter CPU;
        public Memory Memory;

        private bool IsExecuting;

        public Core()
        {
            Memory = new Memory();

            CPU = new Interpreter(Memory, true);

            Memory.SetCPU(CPU);
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
                CPU.Execute();
            }
        }

        public void Stop()
        {
            IsExecuting = false;
        }
    }
}
