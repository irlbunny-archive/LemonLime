using CTREmulator.ARM;

namespace CTREmulator.CTR.IO
{
    partial class IOHandler
    {
        private byte CFG9_RST11_READ(Interpreter CPU)
        {
            // TODO: Implement this

            Logging.WriteInfo("Reading.");

            // 
            // CPU.ReadUInt8(0x0);

            return 0;
        }

        private void CFG9_RST11_WRITE(Interpreter CPU, byte Value)
        {
            // TODO: Implement this

            Logging.WriteInfo("Writing.");

            // 
            // CPU.WriteUInt8(0x0, 0x0);
        }
    }
}
