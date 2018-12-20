using System;

namespace LemonLime.LLE.Memory
{
    class Map
    {
        public delegate void PhysFunc(Context Ctx);

        public uint     Address;
        public uint     Size;
        public byte[]   Phys;
        public PhysFunc PhysCallback;

        public Map(uint Address, uint Size,
            byte[] Phys = null, PhysFunc PhysCallback = null)
        {
            this.Address = Address;
            this.Size    = Size;

            if (Phys != null)
            {
                if (Phys.Length > Size)
                    throw new Exception("Physical memory length is bigger than size variable.");

                this.Phys = Phys;
            }
            else
            {
                if (PhysCallback != null)
                {
                    this.PhysCallback = PhysCallback;
                    return;
                }

                this.Phys = new byte[Size];
            }
        }
    }
}
