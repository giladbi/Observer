using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode
{
    /// <summary>
    /// Provides properties for storing and retrieving CME FIX security status fields.
    /// </summary>
    public sealed class SecurityStatus : Update, IParsable
    {
        public FieldInfo<char> SecurityID { get; set; }
        public char SecurityIDSource { get; set; }
        public DateTime TradeDate { get; set; }
        public int HighPx { get; set; }
        public int LowPx { get; set; }
        public FieldInfo<char> Symbol { get; set; }
        public int SecurityTradingStatus { get; set; }
        public int HaltReason { get; set; }
        public int SecurityTradingEvent { get; set; }

        /// <summary>
        /// Provides properties for storing and retrieving CME FIX security status fields.
        /// </summary>
        public SecurityStatus(FieldInfo<byte> tagInfo, FieldInfo<byte> valueInfo, Trailer trailer)
            : base(tagInfo, valueInfo, trailer)
        {
            SecurityID = new FieldInfo<char>(SECURITY_ID_FIELD_LENGTH);
            Symbol = new FieldInfo<char>(SYMBOL_FIELD_LENGTH);
        }

        /// <summary>
        /// Parses each field of a SecurityStatus update.
        /// </summary>
        public void Parse()
        {
            int tagLength = tag.Length;

            if (tagLength == 2)
            {
                if (IsTag[tagLength](Tag.SecurityStatus.SecurityIDSource))
                {
                    SecurityIDSource = ParseCharField();
                }
                else if (IsTag[tagLength](Tag.SecurityStatus.SecurityID))
                {
                    ParseCharArrayField(SecurityID);
                }
                else if (IsTag[tagLength](Tag.SecurityStatus.Symbol))
                {
                    ParseCharArrayField(Symbol);
                }
                else if (IsTag[tagLength](Tag.SecurityStatus.TradeDate))
                {
                    TradeDate = ParseDateField();
                }
                else if (IsTag[tagLength](Tag.Trailer.CheckSum))
                {
                    trailer.CheckSum = ParseIntField();
                }
            }
            else if (tagLength == 3)
            {
                if (IsTag[tagLength](Tag.SecurityStatus.HighPx))
                {
                    HighPx = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityStatus.LowPx))
                {
                    LowPx = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityStatus.SecurityTradingStatus))
                {
                    SecurityTradingStatus = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityStatus.HaltReason))
                {
                    HaltReason = ParseIntField();
                }
            }
            else if (tagLength == 4)
            {
                if (IsTag[tagLength](Tag.SecurityStatus.SecurityTradingEvent))
                {
                    SecurityTradingEvent = ParseIntField();
                }
            }
        }
    }
}
