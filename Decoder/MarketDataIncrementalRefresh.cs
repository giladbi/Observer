using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode
{
    /// <summary>
    /// Provides properties for storing and retrieving CME FIX market data incremental refresh fields.
    /// </summary>
    public sealed class MarketDataIncrementalRefresh : Update, IParsable
    {
        /// <summary>
        /// Contains fields found in the repeating group block of each update.
        /// </summary>
        public RepeatingGroup[] DataBlock { get; set; }
        public int DataBlockCount { get; set; }

        public DateTime TradeDate { get; set; }
        public int NoMDEntries { get; set; }

        private Decoder decoder;

        /// <summary>
        /// Provides properties for storing and retrieving CME FIX market data incremental refresh fields.
        /// </summary>
        public MarketDataIncrementalRefresh(FieldInfo<byte> tagInfo, FieldInfo<byte> valueInfo, Trailer trailer, Decoder decoder)
            : base(tagInfo, valueInfo, trailer)
        {
            this.decoder = decoder;

            DataBlock = new RepeatingGroup[REPEATING_GROUP_ARRAY_LENGTH];

            for (int i = 0; i < REPEATING_GROUP_ARRAY_LENGTH; i++)
            {
                DataBlock[i] = new RepeatingGroup();
            }
        }

        /// <summary>
        /// Represents the market data incremental refresh repeating group message block.
        /// </summary>
        public class RepeatingGroup
        {
            private const int FIXING_BRACKET_FIELD_LENGTH = 5;

            public int RptSeq { get; set; }
            public int TradeVolume { get; set; }
            public int MDPriceLevel { get; set; }
            public int MDQuoteType { get; set; }
            public char SecurityIDSource { get; set; }
            public FieldInfo<char> SecurityID { get; set; }
            public DateTime SettlDate { get; set; }
            public int AggressorSide { get; set; }
            public FieldInfo<char> FixingBracket { get; set; }
            public int MatchEventIndicator { get; set; }
            public char MDEntryType { get; set; }
            public int MDEntryPx { get; set; }
            public int MDEntrySize { get; set; }
            public char QuoteCondition { get; set; }
            public int NumberOfOrders { get; set; }
            public int TickDirection { get; set; }
            public int NetChgPrevDay { get; set; }
            public int MDUpdateAction { get; set; }
            public TimeSpan MDEntryTime { get; set; }
            public int TradingSessionID { get; set; }
            public int OpenCloseSettleFlag { get; set; }
            public char TradeCondition { get; set; }
            public FieldInfo<char> SecurityDesc { get; set; }

            /// <summary>
            /// Represents the market data incremental refresh repeating group message block.
            /// </summary>
            public RepeatingGroup()
            {
                SecurityID = new FieldInfo<char>(SECURITY_ID_FIELD_LENGTH);
                FixingBracket = new FieldInfo<char>(FIXING_BRACKET_FIELD_LENGTH);
                SecurityDesc = new FieldInfo<char>(SECURITY_DESC_FIELD_LENGTH);
            }
        }

        /// <summary>
        /// Parses each field of a MarketDataIncrementalRefresh update.
        /// </summary>
        public void Parse()
        {
            int tagLength = tag.Length;

            if (tagLength == 2)
            {
                if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.SecurityIDSource))
                {
                    DataBlock[DataBlockCount].SecurityIDSource = ParseCharField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.SecurityID))
                {
                    ParseCharArrayField(DataBlock[DataBlockCount].SecurityID);
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.RptSeq))
                {
                    DataBlock[DataBlockCount].RptSeq = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.SettleDate))
                {
                    DataBlock[DataBlockCount].SettlDate = ParseDateField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.TradeDate))
                {
                    TradeDate = ParseDateField();
                }
                else if (IsTag[tagLength](Tag.Trailer.CheckSum))
                {
                    if (DataBlock[DataBlockCount].MDEntryType == Value.MDEntryType.Trade)
                    {
                        decoder.OnTradeParsed(DataBlock[DataBlockCount]);
                    }
                    else
                    {
                        decoder.OnQuoteParsed(DataBlock[DataBlockCount]);
                    }

                    trailer.CheckSum = ParseIntField();
                }
            }
            else if (tagLength == 3)
            {
                if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.MDEntryType))
                {
                    DataBlock[DataBlockCount].MDEntryType = ParseCharField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.MDEntryPx))
                {
                    DataBlock[DataBlockCount].MDEntryPx = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.MDEntrySize))
                {
                    DataBlock[DataBlockCount].MDEntrySize = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.QuoteCondition))
                {
                    DataBlock[DataBlockCount].QuoteCondition = ParseCharField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.SecurityDesc))
                {
                    ParseCharArrayField(DataBlock[DataBlockCount].SecurityDesc);
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.MDUpdateAction))
                {
                    if (DataBlockCount > 0)
                    {
                        if (DataBlock[DataBlockCount].MDEntryType == Value.MDEntryType.Trade)
                        {
                            decoder.OnTradeParsed(DataBlock[DataBlockCount]);
                        }
                        else
                        {
                            decoder.OnQuoteParsed(DataBlock[DataBlockCount]);
                        }
                    }

                    DataBlockCount++;

                    // Set all values which don't always get read in to "null" here
                    DataBlock[DataBlockCount].MDEntrySize = Value.NullEnumerations.IntEnum;
                    DataBlock[DataBlockCount].TradingSessionID = Value.NullEnumerations.IntEnum;
                    DataBlock[DataBlockCount].MDPriceLevel = Value.NullEnumerations.IntEnum;
                    DataBlock[DataBlockCount].QuoteCondition = Value.NullEnumerations.Char;
                    DataBlock[DataBlockCount].NumberOfOrders = Value.NullEnumerations.IntEnum;
                    DataBlock[DataBlockCount].NetChgPrevDay = Value.NullEnumerations.IntValue;
                    DataBlock[DataBlockCount].TickDirection = Value.NullEnumerations.IntEnum;
                    DataBlock[DataBlockCount].OpenCloseSettleFlag = Value.NullEnumerations.IntEnum;
                    DataBlock[DataBlockCount].SettlDate = Value.NullEnumerations.DateTime;
                    DataBlock[DataBlockCount].TradeCondition = Value.NullEnumerations.Char;
                    DataBlock[DataBlockCount].TradeVolume = Value.NullEnumerations.IntEnum;
                    DataBlock[DataBlockCount].MDQuoteType = Value.NullEnumerations.IntEnum;
                    DataBlock[DataBlockCount].FixingBracket.Contents[0] = Value.NullEnumerations.Char;
                    DataBlock[DataBlockCount].AggressorSide = Value.NullEnumerations.IntEnum;
                    DataBlock[DataBlockCount].MatchEventIndicator = Value.NullEnumerations.IntEnum;

                    DataBlock[DataBlockCount].MDUpdateAction = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.MDEntryTime))
                {
                    DataBlock[DataBlockCount].MDEntryTime = ParseTimeWithMsField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.NumberOfOrders))
                {
                    DataBlock[DataBlockCount].NumberOfOrders = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.TradingSessionID))
                {
                    DataBlock[DataBlockCount].TradingSessionID = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.NoMDEntries))
                {
                    NoMDEntries = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.OpenCloseSettleFlag))
                {
                    DataBlock[DataBlockCount].OpenCloseSettleFlag = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.TradeCondition))
                {
                    DataBlock[DataBlockCount].TradeCondition = ParseCharField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.TickDirection))
                {
                    DataBlock[DataBlockCount].TickDirection = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.NetChgPrevDay))
                {
                    DataBlock[DataBlockCount].NetChgPrevDay = ParseNegativeIntField();
                }
            }
            else if (tagLength == 4)
            {
                if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.MDPriceLevel))
                {
                    DataBlock[DataBlockCount].MDPriceLevel = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.MDQuoteType))
                {
                    DataBlock[DataBlockCount].MDQuoteType = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.TradeVolume))
                {
                    DataBlock[DataBlockCount].TradeVolume = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.FixingBracket))
                {
                    // The documentation doesn't show what format the FixingBracket value comes in, so
                    // I'm not sure what to do here.  Below this Exception is what I *think* may be correct,
                    // but I'm not sure.
                    throw new NotImplementedException("Tag 'Fixing Bracket' not yet implemented.");

                    ParseCharArrayField(DataBlock[DataBlockCount].FixingBracket);
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.AggressorSide))
                {
                    DataBlock[DataBlockCount].AggressorSide = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.MarketDataIncrementalRefresh.MatchEventIndicator))
                {
                    DataBlock[DataBlockCount].MatchEventIndicator = ParseIntField();
                }
            }
        }
    }
}
