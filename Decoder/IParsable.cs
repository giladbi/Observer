using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode
{
    /// <summary>
    /// Provides methods for parsing FIX fields.
    /// </summary>
    public interface IParsable
    {
        void Parse();
    }
}
