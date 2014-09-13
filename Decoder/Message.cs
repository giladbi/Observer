using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode
{
    /// <summary>
    /// Represents a single CME FIX message.
    /// </summary>
    public sealed class Message
    {
        public Header Header { get; set; }
        public Trailer Trailer { get; set; }
        public Update Body { get; set; }

        /// <summary>
        /// Represents a single CME FIX message.
        /// </summary>
        public Message()
        {
        }
    }
}
