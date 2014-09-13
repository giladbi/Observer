using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode.Tag
{
    /// <summary>
    /// Byte representations of CME FIX header tags.
    /// </summary>
    internal static class Header
    {
        public static readonly byte[] ApplVerID = new byte[] { 49, 49, 50, 56 }; //"1128";
        public static readonly byte[] BodyLength = new byte[] { 57 }; //"9";
        public static readonly byte[] MsgType = new byte[] { 51, 53 }; //"35";
        public static readonly byte[] SenderCompID = new byte[] { 52, 57 }; //"49";
        public static readonly byte[] MsgSeqNum = new byte[] { 51, 52 }; //"34";
        public static readonly byte[] SendingTime = new byte[] { 53, 50 }; //"52";
        public static readonly byte[] LastMsgSeqNumProcessed = new byte[] { 51, 54, 57 }; //"369"; // Snapshot only.
        public static readonly byte[] PossDupFlag = new byte[] { 52, 51 }; //"43"; // Usually not sent.
        public static readonly byte[] MDSecurityTradingStatus = new byte[] { 49, 54, 56, 50 }; //"1682";  // Snapshot only.  
    }

    /// <summary>
    /// Byte representations of CME FIX trailer tags.
    /// </summary>
    internal static class Trailer
    {
        public static readonly byte[] CheckSum = new byte[] { 49, 48 }; //"10";
    }

    /// <summary>
    /// Byte representations of CME FIX logout tags. Tag 35=5.
    /// </summary>
    internal static class Logout
    {
        public static readonly byte[] ApplID = new byte[] { 49, 49, 56, 48 }; //"1180";
        public static readonly byte[] Text = new byte[] { 53, 56 }; //"58";
    }

    /// <summary>
    /// Byte representations of CME FIX logon tags.  Tag 35=A.
    /// </summary>
    internal static class Logon
    {
        // Customer to CME
        public static readonly byte[] Username = new byte[] { 53, 53, 51 }; //"553";
        public static readonly byte[] Password = new byte[] { 53, 53, 52 }; //"554";
        public static readonly byte[] DefaultApplVerID = new byte[] { 49, 49, 51, 55 }; //"1137";
        // CME to customer
        public static readonly byte[] EncryptMethod = new byte[] { 57, 56 }; //"98";
        public static readonly byte[] HeartBtInt = new byte[] { 49, 48, 56 }; //"108";
        public static readonly byte[] ApplID = new byte[] { 49, 49, 56, 48 }; //"1180";
    }

    /// <summary>
    /// Byte representations of CME FIX security definition tags.  Tag 35=d.
    /// </summary>
    internal static class SecurityDefinition
    {
        public static readonly byte[] TotNumReports = new byte[] { 57, 49, 49 }; //"911";
        public static readonly byte[] NoEvents = new byte[] { 56, 54, 52 }; //"864";
        public static readonly byte[] EventType = new byte[] { 56, 54, 53 }; //"865";
        public static readonly byte[] EventDate = new byte[] { 56, 54, 54 }; //"866";
        public static readonly byte[] EventTime = new byte[] { 49, 49, 52, 53 }; //"1145";
        public static readonly byte[] TradingReferencePrice = new byte[] { 49, 49, 53, 48 }; //"1150";
        public static readonly byte[] TradingReferenceDate = new byte[] { 53, 55, 57, 54 }; //"5796";
        public static readonly byte[] SettlPriceType = new byte[] { 55, 51, 49 }; //"731";
        public static readonly byte[] HighLimitPrice = new byte[] { 49, 49, 52, 57 }; //"1149";
        public static readonly byte[] LowLimitPrice = new byte[] { 49, 49, 52, 56 }; //"1148";
        public static readonly byte[] SecurityGroup = new byte[] { 49, 49, 53, 49 }; //"1151";
        public static readonly byte[] Symbol = new byte[] { 53, 53 }; //"55";
        public static readonly byte[] SecurityDesc = new byte[] { 49, 48, 55 }; //"107";
        public static readonly byte[] SecurityID = new byte[] { 52, 56 }; //"48";
        public static readonly byte[] SecurityIDSource = new byte[] { 50, 50 }; //"22";
        public static readonly byte[] CFICode = new byte[] { 52, 54, 49 }; //"461";
        public static readonly byte[] UnderlyingProduct = new byte[] { 52, 54, 50 }; //"462";
        public static readonly byte[] SecurityExchange = new byte[] { 50, 48, 55 }; //"207";
        public static readonly byte[] PricingModel = new byte[] { 57, 56, 53, 51 }; //"9853";
        public static readonly byte[] MinCabPrice = new byte[] { 57, 56, 53, 48 }; //"9850";
        public static readonly byte[] NoSecurityAltID = new byte[] { 52, 53, 52 }; //"454"; // Under development as of Dec 2013
        public static readonly byte[] SecurityAltID = new byte[] { 52, 53, 53 }; //"455"; // Under development as of Dec 2013
        public static readonly byte[] SecurityAltIDSource = new byte[] { 52, 53, 54 }; //"456"; // Under development as of Dec 2013
        public static readonly byte[] ExpirationCycle = new byte[] { 56, 50, 55 }; //"827";
        public static readonly byte[] UnitOfMeasureQty = new byte[] { 49, 49, 52, 55 }; //"1147";
        public static readonly byte[] StrikePrice = new byte[] { 50, 48, 50 }; //"202";
        public static readonly byte[] StrikeCurrency = new byte[] { 57, 52, 55 }; //"947";
        public static readonly byte[] MinTradeVol = new byte[] { 53, 54, 50 }; //"562";
        public static readonly byte[] MaxTradeVol = new byte[] { 49, 49, 52, 48 }; //"1140";
        public static readonly byte[] Currency = new byte[] { 49, 53 }; //"15";
        public static readonly byte[] SettlCurrency = new byte[] { 49, 50, 48 }; //"120";
        public static readonly byte[] NoMDFeedTypes = new byte[] { 49, 49, 52, 49 }; //"1141";
        public static readonly byte[] MDFeedType = new byte[] { 49, 48, 50, 50 }; //"1022";
        public static readonly byte[] MarketDepth = new byte[] { 50, 54, 52 }; //"264";
        public static readonly byte[] MatchAlgorithm = new byte[] { 49, 49, 52, 50 }; //"1142";
        public static readonly byte[] SecuritySubType = new byte[] { 55, 54, 50 }; //"762";
        public static readonly byte[] NoUnderlyings = new byte[] { 55, 49, 49 }; //"711";
        public static readonly byte[] UnderlyingSymbol = new byte[] { 51, 49, 49 }; //"311";
        public static readonly byte[] UnderlyingSecurityID = new byte[] { 51, 48, 57 }; //"309";
        public static readonly byte[] UnderlyingSecurityIDSource = new byte[] { 51, 48, 53 }; //"305";
        public static readonly byte[] MaxPriceVariation = new byte[] { 49, 49, 52, 51 }; //"1143";
        public static readonly byte[] ImpliedMarketIndicator = new byte[] { 49, 49, 52, 52 }; //"1144";
        public static readonly byte[] NbInstAttrib = new byte[] { 56, 55, 48 }; //"870";
        public static readonly byte[] InstAttribType = new byte[] { 56, 55, 49 }; //"871";
        public static readonly byte[] InstAttribValue = new byte[] { 56, 55, 50 }; //"872";
        public static readonly byte[] MaturityMonthYear = new byte[] { 50, 48, 48 }; //"200";
        public static readonly byte[] MinPriceIncrement = new byte[] { 57, 54, 57 }; //"969";
        public static readonly byte[] MinPriceIncrementAmount = new byte[] { 49, 49, 52, 54 }; //"1146";
        public static readonly byte[] LastUpdateTime = new byte[] { 55, 55, 57 }; //"779";
        public static readonly byte[] SecurityUpdateAction = new byte[] { 57, 56, 48 }; //"980";
        public static readonly byte[] DisplayFactor = new byte[] { 57, 55, 56, 55 }; //"9787";
        public static readonly byte[] NoLegs = new byte[] { 53, 53, 53 }; //"555";
        public static readonly byte[] LegSymbol = new byte[] { 54, 48, 48 }; //"600";
        public static readonly byte[] LegRatioQty = new byte[] { 54, 50, 51 }; //"623";
        public static readonly byte[] LegSecurityID = new byte[] { 54, 48, 50 }; //"602";
        public static readonly byte[] LegSecurityDesc = new byte[] { 54, 50, 48 }; //"620";
        public static readonly byte[] LegSecurityIDSource = new byte[] { 54, 48, 51 }; //"603";
        public static readonly byte[] LegSide = new byte[] { 54, 50, 52 }; //"624";
        public static readonly byte[] LegSecurityGroup = new byte[] { 53, 55, 57, 53 }; //"5795";
        public static readonly byte[] LegCFICode = new byte[] { 54, 48, 56 }; //"608";
        public static readonly byte[] LegSecuritySubType = new byte[] { 55, 54, 52 }; //"764";
        public static readonly byte[] LegCurrency = new byte[] { 53, 53, 54 }; //"556";
        public static readonly byte[] LegMaturityMonthYear = new byte[] { 54, 49, 48 }; //"610";
        public static readonly byte[] LegStrikePrice = new byte[] { 54, 49, 50 }; //"612";
        public static readonly byte[] LegSecurityExchange = new byte[] { 54, 49, 54 }; //"616";
        public static readonly byte[] LegStrikeCurrency = new byte[] { 57, 52, 50 }; //"942";
        public static readonly byte[] LegPrice = new byte[] { 53, 54, 54 }; //"566";
        public static readonly byte[] LegOptionDelta = new byte[] { 49, 48, 49, 55 }; //"1017";
        public static readonly byte[] ApplID = new byte[] { 49, 49, 56, 48 }; //"1180";
        public static readonly byte[] UserDefinedInstrument = new byte[] { 57, 55, 55, 57 }; //"9779";
        public static readonly byte[] PriceRatio = new byte[] { 57, 55, 55, 48 }; //"5770";
        public static readonly byte[] ContractMultiplierUnit = new byte[] { 49, 52, 51, 53 }; //"1435";
        public static readonly byte[] FlowScheduleType = new byte[] { 49, 52, 51, 57 }; //"1439";
        public static readonly byte[] ContractMultiplier = new byte[] { 50, 51, 49 }; //"231";
        public static readonly byte[] UnitOfMeasure = new byte[] { 57, 57, 54 }; //"996";
        public static readonly byte[] DecayQuantity = new byte[] { 53, 56, 49, 56 }; //"5818";
        public static readonly byte[] DecayStartDate = new byte[] { 53, 56, 49, 57 }; //"5819";
        public static readonly byte[] OriginalContractSize = new byte[] { 53, 56, 54, 57 }; //"5849";
        public static readonly byte[] ClearedVolume = new byte[] { 53, 55, 57, 49 }; //"5791";
        public static readonly byte[] OpenInterestQty = new byte[] { 53, 55, 57, 50 }; //"5792";
        public static readonly byte[] NoLotTypeRules = new byte[] { 49, 50, 51, 52 }; //"1234";
        public static readonly byte[] LotType = new byte[] { 49, 48, 57, 51 }; //"1093";
        public static readonly byte[] MinLotSize = new byte[] { 49, 50, 51, 49 }; //"1231";
    }

    /// <summary>
    /// Byte representations of CME FIX security status tags.  Tag 35=f.
    /// </summary>
    internal static class SecurityStatus
    {
        public static readonly byte[] SecurityID = new byte[] { 52, 56 }; //"48";
        public static readonly byte[] SecurityIDSource = new byte[] { 50, 50 }; //"22";
        public static readonly byte[] Symbol = new byte[] { 53, 53 }; //"55";
        public static readonly byte[] TradeDate = new byte[] { 55, 53 }; //"75";
        public static readonly byte[] HighPx = new byte[] { 51, 51, 50 }; //"332";
        public static readonly byte[] LowPx = new byte[] { 51, 51, 51 }; //"333";
        public static readonly byte[] SecurityTradingStatus = new byte[] { 51, 50, 54 }; //"326";
        public static readonly byte[] HaltReason = new byte[] { 51, 50, 55 }; //"327";
        public static readonly byte[] SecurityTradingEvent = new byte[] { 49, 49, 55, 52 }; //"1174";
    }

    /// <summary>
    /// Byte representations of CME FIX quote request tags.  Tag 35=R.
    /// </summary>
    internal static class QuoteRequest
    {
        public static readonly byte[] QuoteReqID = new byte[] { 49, 51, 49 }; //"131";
        public static readonly byte[] NoRelatedSym = new byte[] { 49, 52, 54 }; //"146";
        public static readonly byte[] Symbol = new byte[] { 53, 53 }; //"55";
        public static readonly byte[] OrderQty = new byte[] { 51, 56 }; //"38";
        public static readonly byte[] Side = new byte[] { 53, 52 }; //"54";
        public static readonly byte[] TransactTime = new byte[] { 54, 48 }; //"60";
        public static readonly byte[] QuoteType = new byte[] { 53, 51, 55 }; //"537";
        public static readonly byte[] SecurityID = new byte[] { 52, 56 }; //"48";
        public static readonly byte[] SecurityIDSource = new byte[] { 50, 50 }; //"22";
    }

    /// <summary>
    /// Byte representations of CME FIX market data request tags.  Tag 35=V.
    /// </summary>
    internal static class MarketDataRequest
    {
        public static readonly byte[] ApplID = new byte[] { 49, 49, 56, 48 }; //"1180";
        public static readonly byte[] MDReqID = new byte[] { 50, 54, 50 }; //"262";
        public static readonly byte[] ApplBeginSeqNo = new byte[] { 49, 49, 56, 50 }; //"1182";
        public static readonly byte[] ApplEndSeqNo = new byte[] { 49, 49, 56, 51 }; //"1183";
    }

    /// <summary>
    /// Byte representations of CME FIX market data snapshot full refresh tags.  Tag 35=W.
    /// </summary>
    internal static class MarketDataSnapshotFullRefresh
    {
        public static readonly byte[] TotNumReports = new byte[] { 57, 49, 49 }; //"911";
        public static readonly byte[] RptSeq = new byte[] { 56, 51 }; //"83";
        public static readonly byte[] MDBookType = new byte[] { 49, 48, 50, 49 }; //"1021";
        public static readonly byte[] NoMDEntries = new byte[] { 50, 54, 56 }; //"268";
        public static readonly byte[] MDEntryType = new byte[] { 50, 54, 57 }; //"269";
        public static readonly byte[] MDEntryPx = new byte[] { 50, 55, 48 }; //"270";
        public static readonly byte[] MDEntrySize = new byte[] { 50, 55, 49 }; //"271";
        public static readonly byte[] QuoteCondition = new byte[] { 50, 55, 54 }; //"276";
        public static readonly byte[] MDPriceLevel = new byte[] { 49, 48, 50, 51 }; //"1023";
        public static readonly byte[] NumberOfOrders = new byte[] { 51, 52, 54 }; //"346";
        public static readonly byte[] TradeVolume = new byte[] { 49, 48, 50, 48 }; //"1020";
        public static readonly byte[] TickDirection = new byte[] { 50, 55, 52 }; //"274";
        public static readonly byte[] NetChgPrevDay = new byte[] { 52, 53, 49 }; //"451";
        public static readonly byte[] SecurityID = new byte[] { 52, 56 }; //"48";
        public static readonly byte[] SecurityIDSource = new byte[] { 50, 50 }; //"22";
    }

    /// <summary>
    /// Byte representations of CME FIX market data incremental refresh tags.  Tag 35=X.
    /// </summary>
    internal static class MarketDataIncrementalRefresh
    {
        public static readonly byte[] SecurityIDSource = new byte[] { 50, 50 }; //"22";
        public static readonly byte[] SecurityID = new byte[] { 52, 56 }; //"48";
        public static readonly byte[] TradeDate = new byte[] { 55, 53 }; //"75";
        public static readonly byte[] RptSeq = new byte[] { 56, 51 }; //"83";
        public static readonly byte[] SecurityDesc = new byte[] { 49, 48, 55 }; //"107;
        public static readonly byte[] NoMDEntries = new byte[] { 50, 54, 56 }; //"268";
        public static readonly byte[] MDEntryType = new byte[] { 50, 54, 57 }; //"269";
        public static readonly byte[] MDEntryPx = new byte[] { 50, 55, 48 }; //"270";
        public static readonly byte[] MDEntrySize = new byte[] { 50, 55, 49 }; //"271";
        public static readonly byte[] QuoteCondition = new byte[] { 50, 55, 54 }; //"276";
        public static readonly byte[] MDPriceLevel = new byte[] { 49, 48, 50, 51 }; //"1023";
        public static readonly byte[] NumberOfOrders = new byte[] { 51, 52, 54 }; //"346";
        public static readonly byte[] TradeVolume = new byte[] { 49, 48, 50, 48 }; //"1020";
        public static readonly byte[] TickDirection = new byte[] { 50, 55, 52 }; //"274";
        public static readonly byte[] NetChgPrevDay = new byte[] { 52, 53, 49 }; //"451";
        public static readonly byte[] MDUpdateAction = new byte[] { 50, 55, 57 }; //"279";
        public static readonly byte[] MDEntryTime = new byte[] { 50, 55, 51 }; //"273";
        public static readonly byte[] TradingSessionID = new byte[] { 51, 51, 54 }; //"336";
        public static readonly byte[] OpenCloseSettleFlag = new byte[] { 50, 56, 54 }; //"286";
        public static readonly byte[] SettleDate = new byte[] { 54, 52 }; //"64";
        public static readonly byte[] TradeCondition = new byte[] { 50, 55, 55 }; //"277";
        public static readonly byte[] MDQuoteType = new byte[] { 49, 48, 55, 48 }; //"1070";
        public static readonly byte[] FixingBracket = new byte[] { 53, 55, 57, 48 }; //"5790";
        public static readonly byte[] AggressorSide = new byte[] { 53, 55, 57, 55 }; //"5797";
        public static readonly byte[] MatchEventIndicator = new byte[] { 53, 55, 57, 57 }; //"5799"; // Under development as of Dec 2013
    }
}
