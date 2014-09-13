using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode
{
    /// <summary>
    /// Provides properties for storing and retrieving CME FIX trailer fields.
    /// </summary>
    public sealed class Trailer
    {
        public int CheckSum { get; set; }

        public Trailer()
        {
        }
    }
}
