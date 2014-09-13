using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode
{
    /// <summary>
    /// Provides properties for storing and retrieving CME FIX logout fields.
    /// </summary>
    public sealed class Logout : Update, IParsable
    {
        private const int TEXT_FIELD_LENGTH = 180;

        public FieldInfo<char> ApplID { get; set; }
        public FieldInfo<char> Text { get; set; }

        /// <summary>
        /// Provides properties for storing and retrieving CME FIX logout fields.
        /// </summary>
        public Logout(FieldInfo<byte> tagInfo, FieldInfo<byte> valueInfo, Trailer trailer)
            : base(tagInfo, valueInfo, trailer)
        {
            ApplID = new FieldInfo<char>(APPL_ID_FIELD_LENGTH);
            Text = new FieldInfo<char>(TEXT_FIELD_LENGTH);
        }

        /// <summary>
        /// Parses each field of a Logout update.
        /// </summary>
        public void Parse()
        {
            int tagLength = tag.Length;

            if (tagLength == 2)
            {
                if (IsTag[tagLength](Tag.Logout.Text))
                {
                    ParseCharArrayField(Text);
                }
                else if (IsTag[tagLength](Tag.Trailer.CheckSum))
                {
                    trailer.CheckSum = ParseIntField();
                }
            }
            else if (tagLength == 4)
            {
                if (IsTag[tagLength](Tag.Logout.ApplID))
                {
                    ParseCharArrayField(ApplID);
                }
            }
        }

    }
}
