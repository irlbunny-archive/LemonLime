using LemonLime.ARM;
using System.Threading;

namespace LemonLime.LLE
{
    public class CTR
    {
        private CPU.Handler Handler;

        private Memory Memory;

        public CTR()
        {
            Memory = new Memory();

            Handler = new CPU.Handler(Memory);

            Handler.EnableCpu(CPU.Type.Arm9, true); // Enable ARM9 CPU
        }

        public void Run()
        {
            Handler.Run();
        }
    }
}
