using LemonLime.ARM;
using System.Threading;

namespace LemonLime.CTR
{
    public class Core
    {
        private CPUHandler CPU;

        private Memory Memory;

        public Core()
        {
            Memory = new Memory();

            CPU = new CPUHandler(Memory);

            CPU.EnableCPU(CPUType.ARM9, true); // Enable ARM9 CPU
        }

        public void Start()
        {
            CPU.Start();
        }
    }
}
