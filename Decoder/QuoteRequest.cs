using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode
{
    /// <summary>
    /// Provides properties for storing and retrieving CME FIX quote request fields.
    /// </summary>
    public sealed class QuoteRequest : Update, IParsable
    {
        private const int QUOTE_REQ_ID_FIELD_LENGTH = 23;

        public FieldInfo<char> QuoteReqID { get; set; }
        public int NoRelatedSym { get; set; }
        public FieldInfo<char> Symbol { get; set; }
        public int OrderQty { get; set; }
        public char Side { get; set; }
        public DateTime TransactTime { get; set; }
        public int QuoteType { get; set; }
        public FieldInfo<char> SecurityID { get; set; }
        public char SecurityIDSource { get; set; }

        /// <summary>
        /// Provides properties for storing and retrieving CME FIX quote request fields.
        /// </summary>
        public QuoteRequest(FieldInfo<byte> tagInfo, FieldInfo<byte> valueInfo, Trailer trailer)
            : base(tagInfo, valueInfo, trailer)
        {
            QuoteReqID = new FieldInfo<char>(QUOTE_REQ_ID_FIELD_LENGTH);
            Symbol = new FieldInfo<char>(SYMBOL_FIELD_LENGTH);
            SecurityID = new FieldInfo<char>(SECURITY_ID_FIELD_LENGTH);

            //throw new NotImplementedException("QuoteRequest class not yet implemented.");
        }

        /// <summary>
        /// Parses each field of a QuoteRequest update.
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
                else if (IsTag[tagLength](Tag.QuoteRequest.Symbol))
                {
                    ParseCharArrayField(Symbol);
                }
                else if (IsTag[tagLength](Tag.QuoteRequest.OrderQty))
                {
                    OrderQty = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.QuoteRequest.Side))
                {
                    Side = ParseCharField();
                }
                else if (IsTag[tagLength](Tag.QuoteRequest.SecurityID))
                {
                    ParseCharArrayField(SecurityID);
                }
                else if (IsTag[tagLength](Tag.QuoteRequest.SecurityIDSource))
                {
                    SecurityIDSource = ParseCharField();
                }
            }
            else if (tagLength == 3)
            {
                if (IsTag[tagLength](Tag.QuoteRequest.QuoteReqID))
                {
                    ParseCharArrayField(QuoteReqID);
                }
                else if (IsTag[tagLength](Tag.QuoteRequest.NoRelatedSym))
                {
                    NoRelatedSym = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.QuoteRequest.QuoteType))
                {
                    QuoteType = ParseIntField();
                }
            }

            throw new NotImplementedException("QuoteRequest_Parse method not yet implemented.");
        }
    }
}
