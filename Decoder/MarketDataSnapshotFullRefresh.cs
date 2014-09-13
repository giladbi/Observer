using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode
{
    /// <summary>
    /// Provides properties for storing and retrieving CME FIX market data snapshot full refresh fields.
    /// </summary>
    public sealed class MarketDataSnapshotFullRefresh : Update, IParsable
    {
        /// <summary>
        /// Contains fields found in the repeating group block of each update.
        /// </summary>
        public RepeatingGroup[] DataBlock { get; set; }
        public int DataBlockCount { get; set; }

        public int TotNumReports { get; set; }
        public int RptSeq { get; set; }
        public int MDBookType { get; set; }
        public FieldInfo<char> SecurityID { get; set; }

        public char SecurityIDSource { get; set; }
        public int NoMDEntries { get; set; }

        /// <summary>
        /// Provides properties for storing and retrieving CME FIX market data snapshot full refresh fields.
        /// </summary>
        public MarketDataSnapshotFullRefresh(FieldInfo<byte> tagInfo, FieldInfo<byte> valueInfo, Trailer trailer)
            : base(tagInfo, valueInfo, trailer)
        {
            SecurityID = new FieldInfo<char>(SECURITY_ID_FIELD_LENGTH);

            DataBlock = new RepeatingGroup[REPEATING_GROUP_ARRAY_LENGTH];
            for (int i = 0; i < REPEATING_GROUP_ARRAY_LENGTH; i++)
            {
                DataBlock[i] = new RepeatingGroup();
            }

            //throw new NotImplementedException("MarketDataSnapshotFullRefresh class not yet implemented.");
        }

        /// <summary>
        /// Represents the market data snapshot full refresh repeating group message block.
        /// </summary>
        public class RepeatingGroup
        {
            public char MDEntryType { get; set; }
            public int MDEntryPx { get; set; }
            public int MDEntrySize { get; set; }
            public char QuoteCondition { get; set; }
            public int MDPriceLevel { get; set; }
            public int NumberOfOrders { get; set; }
            public int TradeVolume { get; set; }
            public char TickDirection { get; set; }
            public int NetChgPrevDay { get; set; }

            /// <summary>
            /// Represents the market data snapshot full refresh repeating group message block.
            /// </summary>
            public RepeatingGroup()
            {
            }
        }

        /// <summary>
        /// Parses each field of a MarketDataSnapshotFullRefresh update.
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
                else if (IsTag[tagLength](Tag.MarketDataSnapshotFullRefresh.RptSeq))
                {
                    RptSeq = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataSnapshotFullRefresh.SecurityID))
                {
                    ParseCharArrayField(SecurityID);
                }
                else if (IsTag[tagLength](Tag.MarketDataSnapshotFullRefresh.SecurityIDSource))
                {
                    SecurityIDSource = ParseCharField();
                }
            }
            else if (tagLength == 3)
            {
                if (IsTag[tagLength](Tag.MarketDataSnapshotFullRefresh.TotNumReports))
                {
                    TotNumReports = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataSnapshotFullRefresh.NoMDEntries))
                {
                    NoMDEntries = ParseIntField();
                }
            }
            else if (tagLength == 4)
            {
                if (IsTag[tagLength](Tag.MarketDataSnapshotFullRefresh.MDBookType))
                {
                    MDBookType = ParseIntField();
                }
            }

            throw new NotImplementedException("MarketDataSnapshotFullRefresh_Parse method not yet implemented.");
        }
    }
}
