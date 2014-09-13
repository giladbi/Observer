using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Observer.Rebuild
{
    /// <summary>
    /// Provides methods and properties used for connecting the Decoder to the raw data set.
    /// </summary>
    public abstract class FileHelper
    {
        /// <summary>
        /// Provides methods and properties used for connecting the Decoder to the raw data set.
        /// </summary>
        public FileHelper()
        {
        }

        /// <summary>
        /// Used for parsing file paths from a directory, which can then be used as arguments for the Rebuilder.Rebuild method.
        /// </summary>
        /// <param name="dataDirectory"></param>
        /// <returns></returns>
        public abstract List<string> GetFiles(string dataDirectory);
    }
}
