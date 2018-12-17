using LemonLime.ARM;
using System.Threading;

namespace LemonLime.LLE
{
    public class CTR
    {
        private Memory      Memory;
        private CPU.Handler Handler;

        public CTR()
        {
            Memory  = new Memory();
            Handler = new CPU.Handler(Memory, Memory);

            Handler.EnableCpu(CPU.Type.Arm9, true); // Enable ARM9 CPU
        }

        public void Run()
        {
            Handler.Start();
        }
    }
}
