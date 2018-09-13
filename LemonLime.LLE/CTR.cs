using LemonLime.ARM;
using System.Threading;

namespace LemonLime.LLE
{
    public class CTR
    {
        private CPUHandler CPU;

        public Memory Memory;

        public CTR()
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
