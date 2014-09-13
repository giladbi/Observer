using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode
{
    /// <summary>
    /// Provides properties for storing and retrieving CME FIX header fields.
    /// </summary>
    public sealed class Header : Update
    {
        private const int SENDER_COMP_ID_FIELD_LENGTH = 7;

        public char ApplVerID { get; set; }
        public int BodyLength { get; set; }
        public char MsgType { get; set; }
        public FieldInfo<char> SenderCompID { get; set; }
        public int MsgSeqNum { get; set; }
        public DateTime SendingTime { get; set; }
        public int LastMsgSeqNumProcessed { get; set; }
        public char PossDupFlag { get; set; }
        public int MDSecurityTradingStatus { get; set; }

        private bool gotTimestamp = false;
        private Decoder decoder;

        /// <summary>
        /// Provides properties for storing and retrieving CME FIX header fields.
        /// </summary>
        public Header(FieldInfo<byte> tagInfo, FieldInfo<byte> valueInfo, Trailer trailer, Decoder decoder)
            : base(tagInfo, valueInfo, trailer)
        {
            SenderCompID = new FieldInfo<char>(SENDER_COMP_ID_FIELD_LENGTH);
            this.decoder = decoder;
        }

        /// <summary>
        /// Parses the header of a message and checks to see if the end of the header has been reached.
        /// </summary>
        /// <returns></returns>
        public bool IsHeaderEnd()
        {
            if (!gotTimestamp)
            {
                gotTimestamp = HasParsedTimestamp();

                return false;
            }
            else
            {
                if (HasParsedHeader())
                {
                    decoder.OnHeaderParsed(this);

                    gotTimestamp = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool HasParsedTimestamp()
        {
            int tagLength = tag.Length;

            if (tagLength == 1)
            {
                if (IsTag[tagLength](Tag.Header.BodyLength))
                {
                    BodyLength = ParseIntField();
                }
            }
            else if (tagLength == 2)
            {
                if (IsTag[tagLength](Tag.Header.MsgType))
                {
                    MsgType = ParseCharField();
                }
                else if (IsTag[tagLength](Tag.Header.SenderCompID))
                {
                    ParseCharArrayField(SenderCompID);
                }
                else if (IsTag[tagLength](Tag.Header.MsgSeqNum))
                {
                    MsgSeqNum = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.Header.SendingTime))
                {
                    SendingTime = ParseDateTimeWithMsField();

                    return true; // Timestamp means we've reached the end of the header.
                }
                else if (IsTag[tagLength](Tag.Trailer.CheckSum))
                {
                    trailer.CheckSum = ParseIntField();
                }
            }
            else if (tagLength == 4)
            {
                if (IsTag[tagLength](Tag.Header.ApplVerID))
                {
                    ApplVerID = ParseCharField();
                }
            }

            return false;
        }

        private bool HasParsedHeader()
        {
            int tagLength = tag.Length;

            if (tagLength == 2)
            {
                if (IsTag[tagLength](Tag.Header.PossDupFlag))
                {
                    PossDupFlag = ParseCharField();

                    return true;
                }
                else if (IsTag[tagLength](Tag.Trailer.CheckSum))
                {
                    trailer.CheckSum = ParseIntField();

                    return false;
                }
                else
                {
                    decoder.ProcessUpdate(value);

                    return true;
                }
            }
            else if (tagLength == 3)
            {
                if (IsTag[tagLength](Tag.Header.LastMsgSeqNumProcessed))
                {
                    LastMsgSeqNumProcessed = ParseIntField();

                    return true;
                }
                else
                {
                    decoder.ProcessUpdate(value);

                    return true;
                }
            }
            else if (tagLength == 4)
            {
                if (IsTag[tagLength](Tag.Header.MDSecurityTradingStatus))
                {
                    MDSecurityTradingStatus = ParseIntField();

                    return true;
                }
                else
                {
                    decoder.ProcessUpdate(value);

                    return true;
                }
            }
            else
            {
                decoder.ProcessUpdate(value);

                return true;
            }
        }
    }
}
