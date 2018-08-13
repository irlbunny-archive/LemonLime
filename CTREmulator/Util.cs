using System;
using System.Collections.Generic;
using System.Text;

namespace CTREmulator
{
    class Util
    {
        public static bool inRange(uint Number, uint RangeMin, uint RangeMax)
        {
            return (Number >= RangeMin && Number <= RangeMax);
        }
    }
}
