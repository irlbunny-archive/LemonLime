using CTREmulator.ARM;
using CTREmulator.CTR.Memory;

namespace CTREmulator.CTR
{
    class Core
    {
        public ARMInterpreter ARM11;
        public ARMInterpreter ARM9;

        public Core()
        {
            // we need to load the bootrom into arm11 memory?
            ARM11 = new ARMInterpreter(Memory);
        }
    }
}
