using LemonLime.ARM;
using System.Threading;

namespace LemonLime.LLE
{
    public class CTR
    {
        private CPUHandler CPU;

        public CTR()
        {
            CPU = new CPUHandler();
            CPU.EnableCPU(CPUType.ARM9, true); // Enable ARM9 CPU
        }

        public void Start()
        {
            CPU.Start();
        }
    }
}
