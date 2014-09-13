using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode
{
    internal static class ExtenstionMethods
    {
        internal static bool IsDummy(this byte[] input)
        {
            throw new Exception("This is a dummy function, and the fact that you've used it indicates that you've done something wrong!");
        }

        internal static bool IsUninitialized(this DateTime input)
        {
            return input == DateTime.MinValue;
        }
    }
}
