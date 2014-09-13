using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Observer.Decode;

namespace Observer.Rebuild
{
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Returns 'true' if the input char[] matches the calling char[].
        /// </summary>
        /// <param name="array"></param>
        /// <param name="otherArray"></param>
        /// <returns></returns>
        public static bool Matches(this char[] array, char[] otherArray)
        {
            for (int i = 0; i < otherArray.Length; i++)
            {
                if (otherArray[i] != array[i])
                    return false;
            }

            return true;
        }

        private static int Concatenate(int a, int b)
        {
            int pow = 1;

            while (pow < b)
            {
                pow = ((pow << 2) + pow) << 1;
                a = ((a << 2) + a) << 1;
            }

            return a + b;
        }

        /// <summary>
        /// Computes an instrument ID from a FieldInfo input.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static int GetId(this FieldInfo<char> field)
        {
            int fieldLength = field.Length;
            int cumulative = 0;

            for (int i = 0; i < fieldLength; i++)
            {
                char fieldContents = field.Contents[i];

                if (i == 0)
                {
                    cumulative = fieldContents - 64;
                    continue;
                }

                GeneratePartialId(fieldContents, ref cumulative);
            }
            
            return cumulative;
        }

        /// <summary>
        /// Computes an instrument ID from a char[] input.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static int GetId(this char[] field)
        {
            int fieldLength = field.Length;
            int cumulative = 0;

            for (int i = 0; i < fieldLength; i++)
            {
                char fieldContents = field[i];

                if (i == 0)
                {
                    cumulative = fieldContents - 64;
                    continue;
                }

                GeneratePartialId(fieldContents, ref cumulative);
            }

            return cumulative;
        }

        private static void GeneratePartialId(char input, ref int hash)
        {
            if (input >= 'A' && input <= 'Z')
            {
                hash = Concatenate(hash, input - 64);
            }
            else if (input >= '0' && input <= '9')
            {
                hash = Concatenate(hash, input - 48);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Field contains unrecognized characters!");
            }
        }

        public static string GetNameWithoutExtension(this System.IO.FileInfo fileInfo)
        {
            string extension = fileInfo.Extension;

            return fileInfo.Name.Replace(extension, "");
        }

        /// <summary>
        /// Returns 'true' if the input DateTime object has the same Date property as the calling DateTime object.
        /// </summary>
        /// <param name="inputTime"></param>
        /// <param name="otherTime"></param>
        /// <returns></returns>
        public static bool IsWithinCacheRangeOf(this DateTime inputTime, DateTime otherTime)
        {
            return inputTime.Date == otherTime.Date;
        }
    }
}
