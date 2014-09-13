using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode.Value
{
    //
    //
    // NOTE: The normal convention is to name constants in ALL_CAPS, however, since all of the following constants are actually enumerations, 
    // I felt that the all-caps naming convention would be superfluous and detrimental to readability.
    //
    //
    
    /// <summary>
    /// Instead of using nullable types, which I felt would introduce too much overhead.
    /// </summary>
    public static class NullEnumerations
    {
        public const char Char = '\0';
        public const int IntEnum = -1;
        public const int IntValue = -999999999;
        public static readonly DateTime DateTime = DateTime.MinValue;
    }

    public static class ApplVerID
    {
        public const char FIX50SP2 = '9';
    }

    public static class MsgType
    {
        public const char Heartbeat = '0';
        public const char Logout = '5';
        public const char Logon = 'A';
        public const char SecurityDefinition = 'd';
        public const char SecurityStatus = 'f';
        public const char QuoteRequest = 'R';
        public const char MarketDataRequest = 'V';
        public const char MarketDataSnapshotFullRefresh = 'W';
        public const char MarketDataIncrementalRefresh = 'X';
        public const char MarketDataRequestReject = 'Y';
    }

    public static class PossDupFlag
    {
        public const char PossibleDuplicate = 'Y';
        public const char OriginalTransmission = 'N';
    }

    public static class MDSecurityTradingStatus
    {
        public const int TradingHalt = 2;
        public const int PriceIndication = 5;
        public const int ReadyToTrade = 17;
        public const int NotAvailableForTrading = 18;
        public const int UnknownOrInvalid = 20;
        public const int PreOpen = 21;
        public const int OpeningRotation = 22;
        public const int PreCross = 24;
        public const int Cross = 25;
        public const int NoCancel = 26;
    }

    public static class DefaultApplVerID
    {
        public const char FIX50SP2 = '9';
    }

    public static class EncryptMethod
    {
        public const int None = 0;
    }

    public static class EventType
    {
        public const int Activation = 5;
        public const int LastEligibleTradeDate = 7;
    }

    public static class SettlPriceType
    {
        public const int Final = 1;
        public const int Theoretical = 2;
        public const int RoundedPrimary = 100;
        public const int UnroundedPrimary = 101;
    }

    public static class SecurityIDSource
    {
        public const char ExchSymb = '8';
    }

    public static class UnderlyingProduct
    {
        public const int CommodityAgriculture = 2;
        public const int Currency = 4;
        public const int Equity = 5;
        public const int Other = 12;
        public const int InterestRate = 14;
        public const int FXCash = 15;
        public const int Energy = 16;
        public const int Metals = 17;
    }

    public static class SecurityExchange
    {
        public static readonly char[] ChicagoBoardOfTrade = new char[4] { 'X', 'C', 'B', 'T' };
        public static readonly char[] ChicagoMercantileExchange = new char[4] { 'X', 'C', 'M', 'E' };
        public static readonly char[] NewYorkMercantileExchange = new char[4] { 'X', 'N', 'Y', 'M' };
        public static readonly char[] CommoditiesExchangeCenter = new char[4] { 'X', 'C', 'E', 'C' };
        public static readonly char[] KansasCityBoardOfTrade = new char[4] { 'X', 'K', 'B', 'T' };
        public static readonly char[] MinneapolisGrainExchange = new char[4] { 'X', 'M', 'G', 'E' };
        public static readonly char[] DubaiMercantileExchange = new char[4] { 'D', 'U', 'M', 'X' };
        public static readonly char[] BMandFBOVESPA = new char[4] { 'X', 'B', 'M', 'F' };
        public static readonly char[] BolsaMexicanaDeValores = new char[4] { 'X', 'M', 'E', 'X' };
        public static readonly char[] BursaMalaysia = new char[4] { 'X', 'K', 'L', 'S' };
        public static readonly char[] KoreaExchange = new char[4] { 'X', 'K', 'F', 'E' };
        public static readonly char[] GreenExchange = new char[4] { 'G', 'R', 'E', 'E' };
        public static readonly char[] NYMEXvsDMEInterExchange = new char[4] { 'N', 'Y', 'U', 'M' };
    }

    public static class PricingModel
    {
        public const char FisherBlack = 'F';
        public const char Whaley = 'W';
    }

    public static class ExpirationCycle
    {
        public const int ExpireOnTradingSessionClose = 0;
        public const int ExpirationAtGivenDate = 2;
    }

    public static class MatchAlgorithm
    {
        public const char FIFO = 'F';
        public const char FIFOProRata = 'K';
        public const char ProRata = 'C';
        public const char NymexFIFOWithLMM = 'N';
        public const char Allocation = 'A';
        public const char FIFOWithLMM = 'T';
        public const char ThresholdProRata = 'O';
        public const char FIFOWithTOPandLMM = 'S';
        public const char ThresholdProRataWithLMM = 'Q';
        public const char EurodollarOptions = 'Y';
    }

    public static class SecuritySubType
    {
        // Options Strategies
        public static readonly char[] ThreeWay = new char[2] { '3', 'W' };
        public static readonly char[] ThreeWayStraddleVSCall = new char[2] { '3', 'C' };
        public static readonly char[] ThreeWayStraddleVSPut = new char[2] { '3', 'P' };
        public static readonly char[] Box = new char[2] { 'B', 'X' };
        public static readonly char[] ButterflyOptions = new char[2] { 'B', 'O' };
        public static readonly char[] XmasTree = new char[2] { 'X', 'T' };
        public static readonly char[] ConditionalCurve = new char[2] { 'C', 'C' };
        public static readonly char[] CondorOptions = new char[2] { 'C', 'O' };
        public static readonly char[] Double = new char[2] { 'D', 'B' };
        public static readonly char[] Horizontal = new char[2] { 'H', 'O' };
        public static readonly char[] HorizontalStraddle = new char[2] { 'H', 'S' };
        public static readonly char[] IronCondor = new char[2] { 'I', 'C' };
        public static readonly char[] Ratio1x2 = new char[2] { '1', '2' };
        public static readonly char[] Ratio1x3 = new char[2] { '1', '3' };
        public static readonly char[] Ratio2x3 = new char[2] { '2', '3' };
        public static readonly char[] RiskReversal = new char[2] { 'R', 'R' };
        public static readonly char[] StraddleStrip = new char[2] { 'S', 'S' };
        public static readonly char[] Straddle = new char[2] { 'S', 'T' };
        public static readonly char[] Strangle = new char[2] { 'S', 'G' };
        public static readonly char[] StripOptions = new char[2] { 'S', 'R' };
        public static readonly char[] Vertical = new char[2] { 'V', 'T' };
        public static readonly char[] JellyRoll = new char[2] { 'J', 'R' };
        public static readonly char[] IronButterFly = new char[2] { 'I', 'B' };
        public static readonly char[] Guts = new char[2] { 'G', 'T' };
        public static readonly char[] Generic = new char[2] { 'G', 'N' };
        // Futures Strategies
        public static readonly char[] CalendarSpread = new char[2] { 'S', 'P' };
        public static readonly char[] FXCalendarSpread = new char[2] { 'F', 'X' };
        public static readonly char[] ReducedTickCalendarSpread = new char[2] { 'R', 'T' };
        public static readonly char[] EquityCalendarSpread = new char[2] { 'E', 'Q' };
        public static readonly char[] ButterflyFutures = new char[2] { 'B', 'F' };
        public static readonly char[] CondorFutures = new char[2] { 'C', 'F' };
        public static readonly char[] StripFutures = new char[2] { 'F', 'S' };
        public static readonly char[] InterCommoditySpread = new char[2] { 'I', 'S' };
        public static readonly char[] Pack = new char[2] { 'P', 'K' };
        public static readonly char[] MonthPack = new char[2] { 'M', 'P' };
        public static readonly char[] PackButterfly = new char[2] { 'P', 'B' };
        public static readonly char[] DoubleButterfly = new char[2] { 'D', 'F' };
        public static readonly char[] PackSpread = new char[2] { 'P', 'S' };
        public static readonly char[] Crack1to1 = new char[2] { 'C', '1' };
        public static readonly char[] Bundle = new char[2] { 'F', 'B' };
        public static readonly char[] BundleSpread = new char[2] { 'B', 'S' };
        public static readonly char[] ImpliedTreasuryIntercommoditySpread = new char[2] { 'I', 'V' };
        public static readonly char[] TASCalendarSpread = new char[2] { 'E', 'C' };
        public static readonly char[] CommoditiesIntercommoditySpread = new char[2] { 'S', 'I' };
        public static readonly char[] BondIndexSpread = new char[2] { 'S', 'D' };
        public static readonly char[] BMDFuturesStrip = new char[2] { 'M', 'S' };
    }

    public static class ImpliedMarketIndicator
    {
        public const int NotImpliedButLegsAreSent = 0;
        public const int ImpliedInAndOut = 3;
    }

    public static class InstAttribType
    {
        public const int VariableTickTable = 23;
        public const int Eligibility = 24;
        public const int MainFraction = 25;
        public const int SubFraction = 26;
        public const int DisplayForPricePrecision = 27;
    }

    public static class InstAttribValue
    {
        // Not sure if I need to handle this or not...
    }

    public static class SecurityUpdateAction
    {
        public const char Delete = 'D';
        public const char Modify = 'M';
    }

    public static class LegSecurityIDSource
    {
        public const char ExchSymb = '8';
    }

    public static class LegSide
    {
        public const char Buy = '5';
        public const char Sell = '2';
    }

    public static class UserDefinedInstrument
    {
        public const char YesUserDefinedInstrument = 'Y';
        public const char NoUserDefinedInstrument = 'N';
    }

    public static class ContractMultiplierUnit
    {
        public const int MultipliedByHour = 1;
        public const int MultipliedByDay = 2;
    }

    public static class FlowScheduleType
    {
        public const int NERCEasternOffPeak = 0;
        public const int NERCWesternOffPeak = 1;
        public const int CalendarAllDaysInMonth = 2;
        public const int NERCEasternPeak = 3;
        public const int NERCWesternPeak = 4;
    }

    public static class LotType
    {
        public const char MinimumOrderQty = '2';
        public const char MinimumBlockQty = '3';
        public const char RoundLot = '4';
    }

    public static class SecurityTradingStatus
    {
        public const int TradingHalt = 2;
        public const int PriceIndication = 5;
        public const int ReadyToTrade = 17;
        public const int NotAvailableForTrading = 18;
        public const int UnknownOrInvalid = 20;
        public const int PreOpen = 21;
        public const int PreCross = 24;
        public const int Cross = 25;
        public const int NoCancel = 26;
    }

    public static class HaltReason
    {
        public const int SuspendedBySurveillance = 1;
        public const int Trading = 2;
        public const int InstrumentAuthorization = 3;
        public const int ReturnToNormalState = 4;
    }

    public static class SecurityTradingEvent
    {
        public const int TradingHalt = 1;
        public const int ResumeOpen = 2;
        public const int EndOfTradingSession = 4;
        public const int PauseInTrading = 100;
    }

    public static class Side
    {
        public const char Buy = '1';
        public const char Sell = '2';
    }

    public static class QuoteType
    {
        public const int Tradable = 1;
    }

    public static class MDBookType
    {
        public const int PriceDepth = 2;
    }

    public static class MDEntryType
    {
        public const char Bid = '0';
        public const char Offer = '1';
        public const char Trade = '2';
        public const char OpeningPrice = '4';
        public const char SettlementPrice = '6';
        public const char TradingSessionHighPrice = '7';
        public const char TradingSessionLowPrice = '8';
        public const char TradeVolume = 'B';
        public const char OpenInterest = 'C';
        public const char SimulatedSell = 'E';
        public const char SimulatedBuy = 'F';
        public const char EmptyBook = 'J';
        public const char SessionHighBid = 'N';
        public const char SessionLowOffer = 'O';
        public const char FixingPrice = 'W';
        public const char CashNote = 'X';
    }

    public static class QuoteCondition
    {
        public const char Implied = 'K';
        public const char ExchangeBest = 'C';
    }

    public static class TickDirection
    {
        public const char PlusTick = '0';
        public const char MinusTick = '2';
    }

    public static class MDUpdateAction
    {
        public const int New = 0;
        public const int Change = 1;
        public const int Delete = 2;
        public const int Overlay = 5;
    }

    public static class TradingSessionID
    {
        public const char PreOpening = '0';
        public const char OpeningMode = '1';
        public const char ContinuousTradingMode = '2';
    }

    public static class OpenCloseSettleFlag
    {
        public const int TheoreticalPriceValue = 5;
        public const int RoundedPreliminary = 100;
        public const int UnroundedPreliminary = 101;
    }

    public static class TradeCondition
    {
        public const char OpeningTrade = 'E';
        public const char PriceCalculatedByGlobex = '1';
    }

    public static class MDQuoteType
    {
        public const int Tradable = 1;
    }

    public static class AggressorSide
    {
        public const char Buy = '1';
        public const char Sell = '2';
    }

    // Under development as of Dec 2013
    public static class MatchEventIndicator
    {
        public const char MidEvent = '0';
        public const char BeginningOfEvent = '1';
        public const char EndOfEvent = '2';
        public const char BeginningAndEndOfEvent = '3';
    }
}
