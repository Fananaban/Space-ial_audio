using System.Collections;
using System.Collections.Generic;

namespace extraMath
{
    public static class mathExtras
    {
        public static float Map(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
