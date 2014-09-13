using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode
{
    /// <summary>
    /// Provides properties for storing and retrieving CME FIX market data request fields.
    /// </summary>
    public sealed class MarketDataRequest : Update, IParsable
    {
        private const int MD_REQ_ID_FIELD_LENGTH = 32;

        public FieldInfo<char> ApplID { get; set; }
        public FieldInfo<char> MDReqID { get; set; }
        public int ApplBeginSeqNo { get; set; }
        public int ApplEndSeqNo { get; set; }

        /// <summary>
        /// Provides properties for storing and retrieving CME FIX market data request fields.
        /// </summary>
        public MarketDataRequest(FieldInfo<byte> tagInfo, FieldInfo<byte> valueInfo, Trailer trailer)
            : base(tagInfo, valueInfo, trailer)
        {
            ApplID = new FieldInfo<char>(APPL_ID_FIELD_LENGTH);
            MDReqID = new FieldInfo<char>(MD_REQ_ID_FIELD_LENGTH);
        }

        /// <summary>
        /// Parses each field of a MarketDataRequest update.
        /// </summary>
        public void Parse()
        {
            int tagLength = tag.Length;

            if (tagLength == 2)
            {
                if (IsTag[tagLength](Tag.Trailer.CheckSum))
                {
                    trailer.CheckSum = ParseIntField();
                }
            }
            else if (tagLength == 3)
            {
                if (IsTag[tagLength](Tag.MarketDataRequest.MDReqID))
                {
                    ParseCharArrayField(MDReqID);
                }
            }
            else if (tagLength == 4)
            {
                if (IsTag[tagLength](Tag.MarketDataRequest.ApplID))
                {
                    ParseCharArrayField(ApplID);
                }
                else if (IsTag[tagLength](Tag.MarketDataRequest.ApplBeginSeqNo))
                {
                    ApplBeginSeqNo = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataRequest.ApplEndSeqNo))
                {
                    ApplEndSeqNo = ParseIntField();
                }
            }
        }
    }
}
