using LemonLime.ARM;
using System.Threading;

namespace LemonLime.CTR
{
    public class Core
    {
        private Interpreter CPU;

        private Memory Memory;

        private bool IsExecuting;

        public Core()
        {
            Memory = new Memory();

            CPU = new Interpreter(Memory, true);
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
