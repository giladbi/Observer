using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using Observer.Decode;
using Observer.Decode.Value;

namespace Observer.Rebuild
{
    /// <summary>
    /// Provides methods and properties for decoding CME FIX messages.
    /// </summary>
    public class Rebuilder : IDisposable
    {
        /// <summary>
        /// A collection of each of the pertinent instruments' non-implied order books.
        /// </summary>
        public List<OrderBook> OrderBooks { get; private set; }

        /// <summary>
        /// A collection of each of the pertinent instruments' implied order books.
        /// </summary>
        public List<OrderBook> ImpliedOrderBooks { get; private set; }
        
        /// <summary>
        /// A collection of each of the pertinent instruments' consolidated (non-implied + implied) order books.
        /// </summary>
        public List<OrderBook> ConsolidatedOrderBooks { get; private set; }
        
        /// <summary>
        /// The directory from which the raw FIX data is read.
        /// </summary>
        public string DataDirectory { get; private set; }

        /// <summary>
        /// Returns 'true' if the order book rebuild operation is complete.
        /// </summary>
        public bool IsRebuildComplete { get; private set; }

        private const int NOT_FOUND = -1;
        private const int OUTRIGHT_DEPTH = 10; 
        private const int IMPLIED_DEPTH = 2;

        private Observer.Decode.Decoder fixDecoder = new Observer.Decode.Decoder();
        private InstrumentCollection instrumentCollection;
        private List<int> pertinentContracts;
        private int orderBookNumber;
        private Header header;
        private bool messageContainsValidTrade = false;
        private bool messageContainsValidQuote = false;
        private DateTime currentTime;
        private DateTime bookRollPreviousCheckTime;

        /// <summary>
        /// Provides methods and properties for decoding CME FIX messages.
        /// </summary>
        public Rebuilder(string instrumentConfigurationDirectory)
        {
            OrderBooks = new List<OrderBook>();
            ImpliedOrderBooks = new List<OrderBook>();
            ConsolidatedOrderBooks = new List<OrderBook>();

            fixDecoder.HeaderParsed += OnHeaderParsed;
            fixDecoder.TradeParsed += OnTradeParsed;
            fixDecoder.QuoteParsed += OnQuoteParsed;
            fixDecoder.MessageParsed += OnMessageParsed;
            fixDecoder.DecodeComplete += OnDecodeComplete;
            fixDecoder.DecodeStarted += OnDecodeStarted;

            instrumentCollection = new InstrumentCollection(instrumentConfigurationDirectory);
        }

        /// <summary>
        /// Rebuilds the limit order book from historical FIX messages.
        /// </summary>
        /// <param name="rawFiles"></param>
        public void Rebuild(IEnumerable<string> rawFiles)
        {
            FileInfo firstRawFile = new FileInfo(rawFiles.First());
            
            DataDirectory = firstRawFile.DirectoryName;
            IsRebuildComplete = false;
            
            fixDecoder.DecodeHistorical(rawFiles);
        }

        /// <summary>
        /// Called when the header of a FIX message has been decoded.
        /// </summary>
        /// <param name="header"></param>
        protected virtual void OnHeaderParsed(Header header)
        {
            this.header = header;
            currentTime = header.SendingTime;

            RollOrderBooks(currentTime);

            messageContainsValidTrade = false;
            messageContainsValidQuote = false;
        }

        /// <summary>
        /// Called when a trade has been decoded from a market data incremental refresh FIX message.
        /// </summary>
        /// <param name="update"></param>
        protected virtual void OnTradeParsed(MarketDataIncrementalRefresh.RepeatingGroup update)
        {
            if (!IsValidTrade(update))
            {
                return;
            }

            int contractIndex = GetContractIndex(update);
            if (contractIndex == NOT_FOUND)
            {
                return;
            }

            messageContainsValidTrade = true;

            OrderBooks[contractIndex].CommitTrade(update.MDEntryPx, currentTime, update.TradeVolume);
        }

        /// <summary>
        /// Called when a quote has been decoded from a market data incremental refresh FIX message.
        /// </summary>
        /// <param name="update"></param>
        protected virtual void OnQuoteParsed(MarketDataIncrementalRefresh.RepeatingGroup update)
        {
            if (!IsValidQuote(update))
            {
                return;
            }

            int contractIndex = GetContractIndex(update);
            if (contractIndex == NOT_FOUND)
            {
                return;
            }

            orderBookNumber = contractIndex;

            messageContainsValidQuote = true;

            BuildOrderBook(update);
        }

        /// <summary>
        /// Called when an entire FIX message has been decoded.
        /// </summary>
        /// <param name="message"></param>
        protected virtual void OnMessageParsed(Message message)
        {
            if (!IsValidMessage(message))
            {
                return;
            }

            ConsolidatedOrderBooks[orderBookNumber].Consolidate(OrderBooks[orderBookNumber], ImpliedOrderBooks[orderBookNumber]);
        }

        /// <summary>
        /// Called when the decode operation has finished.
        /// </summary>
        /// <param name="timestamp"></param>
        protected virtual void OnDecodeComplete(DateTime timestamp)
        {
            //Console.WriteLine("Gen 0 collections: " + GC.CollectionCount(0));
            //Console.WriteLine("Gen 1 collections: " + GC.CollectionCount(1));
            //Console.WriteLine("Gen 2 collections: " + GC.CollectionCount(2));

            //Console.WriteLine("Operation complete!  Press any key to exit.");

            Dispose();
            IsRebuildComplete = true;
        }

        /// <summary>
        /// Called when the decode operation has started.
        /// </summary>
        /// <param name="timestamp"></param>
        protected virtual void OnDecodeStarted(DateTime timestamp)
        {
            //Console.WriteLine("Initial timestamp parsed: " + timestamp.ToString());

            ClearOrderBooks(instrumentCollection.PertinentContracts(timestamp));
        }

        /// <summary>
        /// Uses various elements of a given message to determine whether or not a trade is valid.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        protected virtual bool IsValidTrade(MarketDataIncrementalRefresh.RepeatingGroup update)
        {
            return update.QuoteCondition != QuoteCondition.ExchangeBest 
                && update.TradeCondition != TradeCondition.PriceCalculatedByGlobex;
        }

        /// <summary>
        /// Uses various elements of a given message to determine whether or not a quote is valid.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        protected virtual bool IsValidQuote(MarketDataIncrementalRefresh.RepeatingGroup update)
        {
            return update.QuoteCondition != QuoteCondition.ExchangeBest;
        }

        /// <summary>
        /// Uses various elements of a given message to determine whether or not said message is valid.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual bool IsValidMessage(Message message)
        {
            return message.Header.MsgType == MsgType.MarketDataIncrementalRefresh
                && (messageContainsValidTrade || messageContainsValidQuote);
        }
        
        private void BuildOrderBook(MarketDataIncrementalRefresh.RepeatingGroup update)
        {
            if (update.QuoteCondition != QuoteCondition.Implied)
            {
                OrderBooks[orderBookNumber].Build(header, update);
            }
            else if (update.QuoteCondition == QuoteCondition.Implied)
            {
                ImpliedOrderBooks[orderBookNumber].Build(header, update);
            }
        }

        private bool RollOrderBooks(DateTime currentTime)
        {
            if (!currentTime.IsWithinCacheRangeOf(bookRollPreviousCheckTime))
            {
                pertinentContracts = instrumentCollection.PertinentContracts(currentTime);
                ReconcileMissingContracts();
                ReconcileExpiredContracts();

                bookRollPreviousCheckTime = currentTime;

                return true;
            }

            return false;
        }

        private void ClearOrderBooks(List<int> pertinentContracts)
        {
            OrderBooks.Clear();
            ImpliedOrderBooks.Clear();
            ConsolidatedOrderBooks.Clear();

            int contractCount = pertinentContracts.Count;
            for (int i = 0; i < contractCount; i++)
            {
                int instrument = pertinentContracts[i];

                AddNewOrderBooksFor(instrument);
            }
        }

        private void AddNewOrderBooksFor(int instrumentId)
        {
            OrderBooks.Add(new OrderBook(OUTRIGHT_DEPTH, instrumentId));
            ImpliedOrderBooks.Add(new OrderBook(IMPLIED_DEPTH, instrumentId));
            ConsolidatedOrderBooks.Add(new OrderBook(OUTRIGHT_DEPTH, instrumentId));
        }

        private void ReconcileMissingContracts()
        {
            foreach (int pertinentContract in pertinentContracts)
            {
                bool matchFound = false;
                foreach (OrderBook existingContract in ConsolidatedOrderBooks)
                {
                    if (pertinentContract == existingContract.InstrumentId)
                    {
                        matchFound = true;
                        break;
                    }
                }

                if (!matchFound)
                {
                    AddNewOrderBooksFor(pertinentContract);
                }
            }
        }

        private void ReconcileExpiredContracts()
        {
            for (int i = 0; i < ConsolidatedOrderBooks.Count; i++)
            {
                bool matchFound = false;
                foreach (int pertinentContract in pertinentContracts)
                {
                    if (ConsolidatedOrderBooks[i].InstrumentId == pertinentContract)
                    {
                        matchFound = true;
                        break;
                    }
                }

                if (!matchFound)
                {
                    OrderBooks.RemoveAt(i);
                    ImpliedOrderBooks.RemoveAt(i);
                    ConsolidatedOrderBooks.RemoveAt(i);
                }
            }
        }

        private int GetContractIndex(MarketDataIncrementalRefresh.RepeatingGroup update)
        {
            int contractHash = update.SecurityDesc.GetId();
            
            return instrumentCollection[0].PertinentContracts(currentTime).IndexOf(contractHash);
        }

        public void Dispose()
        {
            fixDecoder.HeaderParsed -= OnHeaderParsed;
            fixDecoder.TradeParsed -= OnTradeParsed;
            fixDecoder.QuoteParsed -= OnQuoteParsed;
            fixDecoder.MessageParsed -= OnMessageParsed;
            fixDecoder.DecodeComplete -= OnDecodeComplete;
            fixDecoder.DecodeStarted -= OnDecodeStarted;
        }
    }
}
