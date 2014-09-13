using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode
{
    /// <summary>
    /// Provides properties for storing and retrieving CME FIX security definition fields.
    /// </summary>
    public sealed class SecurityDefinition : Update, IParsable
    {
        private const int SECURITY_GROUP_FIELD_LENGTH = 6;
        private const int CFI_CODE_FIELD_LENGTH = 6;
        private const int SECURITY_EXCHANGE_FIELD_LENGTH = 4;
        private const int STRIKE_CURRENCY_FIELD_LENGTH = 3;
        private const int CURRENCY_FIELD_LENGTH = 3;
        private const int SETTL_CURRENCY_FIELD_LENGTH = 3;

        /// <summary>
        /// Contains fields found in the repeating group (for fields relating to the 'EventType' tag) block of each update.
        /// </summary>
        public RepeatingGroupEvent[] DataBlockEvent { get; set; }
        public int DataBlockEventCount { get; set; }

        /// <summary>
        /// Contains fields found in the repeating group (for fields relating to the 'MDFeedType' tag) block of each update.
        /// </summary>
        public RepeatingGroupMDFeedType[] DataBlockMDFeedType { get; set; }
        public int DataBlockMDFeedTypeCount { get; set; }

        public int TotNumReports { get; set; }
        public int NoEvents { get; set; }
        public int TradingReferencePrice { get; set; }
        public DateTime TradingReferenceDate { get; set; } // TODO: HAS NOT BEEN INCLUDED IN THE SECDEFPARSE METHOD, YET!!!!!!!!!!!!!!!!!!!!!
        public int SettlPriceType { get; set; }
        public int HighLimitPrice { get; set; }
        public int LowLimitPrice { get; set; }
        public FieldInfo<char> SecurityGroup { get; set; }
        public FieldInfo<char> Symbol { get; set; }
        public FieldInfo<char> SecurityDesc { get; set; }
        public FieldInfo<char> SecurityID { get; set; }
        public char SecurityIDSource { get; set; }
        public FieldInfo<char> CFICode { get; set; }
        public int UnderlyingProduct { get; set; }
        public FieldInfo<char> SecurityExchange { get; set; }
        public char PricingModel { get; set; }
        public int MinCabPrice { get; set; }
        //public int NoSecurityAltID; // Under development @ CME, should have its own RepeatingGroup class
        //public char[] SecurityAltID = new char[arraySize]; // Under development @ CME, should go in the above RepeatingGroup class
        //public int SecurityAltIDLength; // Under development @ CME, should go in the above RepeatingGroup class
        //public char SecurityAltIDSource; // Under development @ CME, should go in the above RepeatingGroup class
        public int ExpirationCycle { get; set; }
        public int UnitOfMeasureQty { get; set; }
        public int StrikePrice { get; set; }
        public FieldInfo<char> StrikeCurrency { get; set; }
        public int MinTradeVol { get; set; }
        public int MaxTradeVol { get; set; }
        public FieldInfo<char> Currency { get; set; }
        public FieldInfo<char> SettlCurrency { get; set; }
        public int NoMDFeedTypes { get; set; }

        /// <summary>
        /// Provides properties for storing and retrieving CME FIX security definition fields.
        /// </summary>
        public SecurityDefinition(FieldInfo<byte> tagInfo, FieldInfo<byte> valueInfo, Trailer trailer) 
            : base(tagInfo, valueInfo, trailer)
        {
            SecurityGroup = new FieldInfo<char>(SECURITY_GROUP_FIELD_LENGTH);
            Symbol = new FieldInfo<char>(SYMBOL_FIELD_LENGTH);
            SecurityDesc = new FieldInfo<char>(SECURITY_DESC_FIELD_LENGTH);
            SecurityID = new FieldInfo<char>(SECURITY_ID_FIELD_LENGTH);
            CFICode = new FieldInfo<char>(CFI_CODE_FIELD_LENGTH);
            SecurityExchange = new FieldInfo<char>(SECURITY_EXCHANGE_FIELD_LENGTH);
            StrikeCurrency = new FieldInfo<char>(STRIKE_CURRENCY_FIELD_LENGTH);
            Currency = new FieldInfo<char>(CURRENCY_FIELD_LENGTH);
            SettlCurrency = new FieldInfo<char>(SETTL_CURRENCY_FIELD_LENGTH);

            DataBlockEvent = new RepeatingGroupEvent[REPEATING_GROUP_ARRAY_LENGTH];
            for (int i = 0; i < REPEATING_GROUP_ARRAY_LENGTH; i++)
            {
                DataBlockEvent[i] = new RepeatingGroupEvent();
            }

            DataBlockMDFeedType = new RepeatingGroupMDFeedType[REPEATING_GROUP_ARRAY_LENGTH];
            for (int i = 0; i < REPEATING_GROUP_ARRAY_LENGTH; i++)
            {
                DataBlockMDFeedType[i] = new RepeatingGroupMDFeedType();
            }

            //throw new NotImplementedException("SecurityDefinition class not yet implemented.");
        }

        /// <summary>
        /// Represents the security definition repeating group message block for fields relating to the 'EventType' tag.
        /// </summary>
        public class RepeatingGroupEvent
        {
            public int EventType { get; set; }
            public DateTime EventDate { get; set; }
            public TimeSpan EventTime { get; set; }

            /// <summary>
            /// Represents the security definition repeating group message block for fields relating to the 'EventType' tag.
            /// </summary>
            public RepeatingGroupEvent()
            {
            }
        }

        /// <summary>
        /// Represents the security definition repeating group message block for fields relating to the 'MDFeedType' tag.
        /// </summary>
        public class RepeatingGroupMDFeedType
        {
            private const int MD_FEED_TYPE_FIELD_LENGTH = 3;

            public FieldInfo<char> MDFeedType { get; set; }
            public int MarketDepth { get; set; }

            /// <summary>
            /// Represents the security definition repeating group message block for fields relating to the 'MDFeedType' tag.
            /// </summary>
            public RepeatingGroupMDFeedType()
            {
                MDFeedType = new FieldInfo<char>(MD_FEED_TYPE_FIELD_LENGTH);
            }
        }

        /// <summary>
        /// Parses each field of a SecurityDefinition update.
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
                else if (IsTag[tagLength](Tag.SecurityDefinition.Symbol))
                {
                    ParseCharArrayField(Symbol);
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.SecurityID))
                {
                    ParseCharArrayField(SecurityID);
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.SecurityIDSource))
                {
                    SecurityIDSource = ParseCharField();
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.Currency))
                {
                    ParseCharArrayField(Currency);
                }
            }
            else if (tagLength == 3)
            {
                if (IsTag[tagLength](Tag.SecurityDefinition.TotNumReports))
                {
                    TotNumReports = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.NoEvents))
                {
                    NoEvents = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.SettlPriceType))
                {
                    SettlPriceType = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.SecurityDesc))
                {
                    ParseCharArrayField(SecurityDesc);
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.CFICode))
                {
                    ParseCharArrayField(CFICode);
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.UnderlyingProduct))
                {
                    UnderlyingProduct = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.SecurityExchange))
                {
                    ParseCharArrayField(SecurityExchange);
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.ExpirationCycle))
                {
                    ExpirationCycle = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.StrikePrice))
                {
                    StrikePrice = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.StrikeCurrency))
                {
                    ParseCharArrayField(StrikeCurrency);
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.MinTradeVol))
                {
                    MinTradeVol = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.SettlCurrency))
                {
                    ParseCharArrayField(SettlCurrency);
                }
            }
            else if (tagLength == 4)
            {
                if (IsTag[tagLength](Tag.SecurityDefinition.TradingReferencePrice))
                {
                    TradingReferencePrice = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.HighLimitPrice))
                {
                    HighLimitPrice = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.LowLimitPrice))
                {
                    LowLimitPrice = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.SecurityGroup))
                {
                    ParseCharArrayField(SecurityGroup);
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.PricingModel))
                {
                    PricingModel = ParseCharField();
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.MinCabPrice))
                {
                    MinCabPrice = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.UnitOfMeasureQty))
                {
                    UnitOfMeasureQty = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.MaxTradeVol))
                {
                    MaxTradeVol = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.SecurityDefinition.NoMDFeedTypes))
                {
                    NoMDFeedTypes = ParseIntField();
                }
            }

            throw new NotImplementedException("SecurityDefinition_Parse method not yet implemented.");
        }
    }
}
