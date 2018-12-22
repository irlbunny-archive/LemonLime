namespace LemonLime.LLE
{
    public class CTR
    {
        private Memory.Handler ARM9Memory;

        private Memory.Handler ARM11Memory;

        private IO.Handler IO;

        private CPU.Handler Handler;

        public CTR()
        {
            ARM9Memory = new Memory.Handler();

            ARM11Memory = new Memory.Handler();

            IO = new IO.Handler();

            ARM9Memory.Maps.Add(new Memory.Map(0x10000000, 0x08000000, null, IO.Call));

            ARM11Memory.Maps.Add(new Memory.Map(0x10000000, 0x08000000, null, IO.Call));

            Handler = new CPU.Handler(ARM9Memory, ARM11Memory);

            CPU.Handler.EnableCPU(CPU.Type.ARM9, true); // Enable ARM9 CPU
        }

        public void Run()
        {
            Handler.Start();
        }
    }
}
