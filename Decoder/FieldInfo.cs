using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode
{
    /// <summary>
    /// Represents properties for storing information about the 'value' component of each tag/value pair.
    /// </summary>
    public sealed class FieldInfo<T> where T : struct
    {
        public T[] Contents { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// Represents properties for storing information about the 'value' component of each tag/value pair.
        /// </summary>
        /// <param name="maxContentsLength">Needs to be greater than the largest max value length as specified in the 'SDKFFMessageSpecs2012.pdf' document (v2.20, 05/01/2012).</param>
        public FieldInfo(int maxContentsLength)
        {
            Contents = new T[maxContentsLength];
        }
    }
}
