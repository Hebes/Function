using System;
using System.Collections.Generic;

namespace Framework.Core
{
    public static class ExpansionEnum
    {
        public static int ToInt(this System.Enum e) => e.GetHashCode();
        
    }
}
