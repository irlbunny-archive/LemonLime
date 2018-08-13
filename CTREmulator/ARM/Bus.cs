namespace CTREmulator.ARM
{
    /// <summary>
    ///     ARM Bus interface for Memory access.
    /// </summary>
    public interface IBus
    {
        /// <summary>
        ///     Reads a 8-bits value from the Memory.
        /// </summary>
        /// <param name="Address">Address to read the data from</param>
        /// <returns>Data on the address</returns>
        byte ReadUInt8(uint Address);

        /// <summary>
        ///     Writes a 8-bits value to the Memory.
        /// </summary>
        /// <param name="Address">Address to write the data on</param>
        /// <param name="Value">Value to be written</param>
        void WriteUInt8(uint Address, byte Value);
    }

    public partial class Interpreter
    {
        //Read

        /// <summary>
        ///     Reads a 16-bits Little Endian value from the Memory.
        /// </summary>
        /// <param name="Address">Address to read the data from</param>
        /// <returns>Data on the address</returns>
        private ushort ReadUInt16(uint Address)
        {
            return (ushort)(Bus.ReadUInt8(Address) |
                (Bus.ReadUInt8(Address + 1) << 8));
        }

        /// <summary>
        ///     Reads a 32-bits Little Endian value from the Memory.
        /// </summary>
        /// <param name="Address">Address to read the data from</param>
        /// <returns>Data on the address</returns>
        private uint ReadUInt32(uint Address)
        {
            return (uint)(Bus.ReadUInt8(Address) |
                (Bus.ReadUInt8(Address + 1) << 8) |
                (Bus.ReadUInt8(Address + 2) << 16) |
                (Bus.ReadUInt8(Address + 3) << 24));
        }

        /// <summary>
        ///     Reads a 16-bits value from the Memory on Little or Big Endian, depending on the E-bit setting.
        /// </summary>
        /// <param name="Address">Address to read the data from</param>
        /// <returns>Data on the address</returns>
        private ushort ReadUInt16E(uint Address)
        {
            if (Registers.IsFlagSet(ARMFlag.Endianness))
                return (ushort)((Bus.ReadUInt8(Address) << 8) |
                    Bus.ReadUInt8(Address + 1));
            else
                return ReadUInt16(Address);
        }

        /// <summary>
        ///     Reads a 32-bits value from the Memory on Little or Big Endian, depending on the E-bit setting.
        /// </summary>
        /// <param name="Address">Address to read the data from</param>
        /// <returns>Data on the address</returns>
        private uint ReadUInt32E(uint Address)
        {
            if (Registers.IsFlagSet(ARMFlag.Endianness))
                return (uint)((Bus.ReadUInt8(Address) << 24) |
                    (Bus.ReadUInt8(Address + 1) << 16) |
                    (Bus.ReadUInt8(Address + 2) << 8) |
                    Bus.ReadUInt8(Address + 3));
            else
                return ReadUInt32(Address);
        }

        //Write

        /// <summary>
        ///     Writes a 16-bits Little Endian value to the Memory.
        /// </summary>
        /// <param name="Address">Address to write the data on</param>
        /// <param name="Value">Value to be written</param>
        private void WriteUInt16(uint Address, ushort Value)
        {
            Bus.WriteUInt8(Address, (byte)Value);
            Bus.WriteUInt8(Address + 1, (byte)(Value >> 8));
        }

        /// <summary>
        ///     Writes a 32-bits Little Endian value to the Memory.
        /// </summary>
        /// <param name="Address">Address to write the data on</param>
        /// <param name="Value">Value to be written</param>
        public void WriteUInt32(uint Address, uint Value)
        {
            Bus.WriteUInt8(Address, (byte)Value);
            Bus.WriteUInt8(Address + 1, (byte)(Value >> 8));
            Bus.WriteUInt8(Address + 2, (byte)(Value >> 16));
            Bus.WriteUInt8(Address + 3, (byte)(Value >> 24));
        }

        /// <summary>
        ///     Writes a 16-bits value to the Memory on Little or Big Endian, depending on the E-bit setting.
        /// </summary>
        /// <param name="Address">Address to write the data on</param>
        /// <param name="Value">Value to be written</param>
        private void WriteUInt16E(uint Address, ushort Value)
        {
            if (Registers.IsFlagSet(ARMFlag.Endianness))
            {
                Bus.WriteUInt8(Address, (byte)(Value >> 8));
                Bus.WriteUInt8(Address + 1, (byte)Value);
            }
            else
                WriteUInt16(Address, Value);
        }

        /// <summary>
        ///     Writes a 32-bits value to the Memory on Little or Big Endian, depending on the E-bit setting.
        /// </summary>
        /// <param name="Address">Address to write the data on</param>
        /// <param name="Value">Value to be written</param>
        private void WriteUInt32E(uint Address, uint Value)
        {
            if (Registers.IsFlagSet(ARMFlag.Endianness))
            {
                Bus.WriteUInt8(Address, (byte)(Value >> 24));
                Bus.WriteUInt8(Address + 1, (byte)(Value >> 16));
                Bus.WriteUInt8(Address + 2, (byte)(Value >> 8));
                Bus.WriteUInt8(Address + 3, (byte)Value);
            }
            else
                WriteUInt32(Address, Value);
        }
    }
}
