using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Observer.Decode;
using Observer.Decode.Value;

namespace Observer.Rebuild
{
    /// <summary>
    /// Provides methods and properties which represent a CME limit order book.
    /// </summary>
    public class OrderBook
    {
        /// <summary>
        /// Represents all bid levels of an instrument.  Note that the best bid is index one, rather than index zero.
        /// </summary>
        public PriceLevel[] Bid { get; private set; }

        /// <summary>
        /// Represents all offered levels of an instrument.  Note that the best offer is index one, rather than index zero.
        /// </summary>
        public PriceLevel[] Ask { get; private set; }

        /// <summary>
        /// The number of order book depth levels supported by the instrument.
        /// </summary>
        public int Depth { get; private set; }

        /// <summary>
        /// Represents an instrument's last trade.
        /// </summary>
        public LastTradedPrice LastTradedPrice { get; private set; }

        public DateTime UpdateTime { get; private set; }

        /// <summary>
        /// An integer representation of an instrument's symbol.
        /// </summary>
        public int InstrumentId { get; private set; }

        private const int NOT_FOUND = 0;

        private DateTime previousCallTime;
        private bool isPertinent;

        /// <summary>
        /// Provides methods and properties which represent a CME limit order book.
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="instrumentId"></param>
        public OrderBook(int depth, int instrumentId)
        {
            InitializeOrderBook(depth);

            Depth = depth;
            LastTradedPrice = LastTradedPrice.GetDummy();
            InstrumentId = instrumentId;
        }

        /// <summary>
        /// Provides methods and properties which represent a CME limit order book.
        /// </summary>
        /// <param name="depth"></param>
        public OrderBook(int depth)
        {
            InitializeOrderBook(depth);

            Depth = depth;
            LastTradedPrice = LastTradedPrice.GetDummy();
        }

        /// <summary>
        /// Deep copies the calling OrderBook to the OrderBook object specified as a parameter.
        /// </summary>
        /// <returns></returns>
        public void CopyInto(OrderBook destinationOrderBook)
        {
            for (int i = 1; i < Depth + 1; i++)
            {
                Ask[i].CopyInto(destinationOrderBook.Ask[i]);
                Bid[i].CopyInto(destinationOrderBook.Bid[i]);
            }

            destinationOrderBook.LastTradedPrice = LastTradedPrice;
            destinationOrderBook.UpdateTime = UpdateTime;
            destinationOrderBook.InstrumentId = InstrumentId;
        }

        /// <summary>
        /// Applies the current update to the limit order book.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="update"></param>
        public void Build(Header header, MarketDataIncrementalRefresh.RepeatingGroup update)
        {
            if (update.MDUpdateAction == MDUpdateAction.New)
            {
                New(header, update);
            }
            else if (update.MDUpdateAction == MDUpdateAction.Change)
            {
                Change(header, update);
            }
            else if (update.MDUpdateAction == MDUpdateAction.Delete)
            {
                Delete(header, update);
            }

            UpdateTime = header.SendingTime;
        }

        // NOTE: In the context of the CME's data platform, a NEW OrderAction means the addition of an entire price level
        // to the book, not simply the addition of a single order.  Additions of single
        // orders (to a pre-existing price level) will be communicated via the
        // CHANGE OrderAction.
        /// <summary>
        /// Inserts a new price level into the limit order book.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="update"></param>
        private void New(Header header, MarketDataIncrementalRefresh.RepeatingGroup update)
        {
            int level = update.MDPriceLevel;

            if (update.MDEntryType == MDEntryType.Bid)
            {
                ShiftDepthLevelsDown(Bid, from: Depth, to: level);

                Bid[level].Price = update.MDEntryPx;
                Bid[level].Size = update.MDEntrySize;
                Bid[level].UpdateTime = header.SendingTime;
                Bid[level].NumberOfOrders = update.NumberOfOrders;
            }
            else if (update.MDEntryType == MDEntryType.Offer)
            {
                ShiftDepthLevelsDown(Ask, from: Depth, to: level);

                Ask[level].Price = update.MDEntryPx;
                Ask[level].Size = update.MDEntrySize;
                Ask[level].UpdateTime = header.SendingTime;
                Ask[level].NumberOfOrders = update.NumberOfOrders;
            }
        }

        // NOTE: In the context of the CME's data platform, a CHANGE OrderAction means a change to the aggregate size
        // of a pre-existing level.  It DOES NOT mean a change to the price of a resting order.
        // A change to the price of a resting order would be communicated via a DELETE OrderAction
        // at the old price, followed immediately by a NEW OrderAction at the new price.
        /// <summary>
        /// Modifies a price level which is already displayed in the limit order book.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="update"></param>
        private void Change(Header header, MarketDataIncrementalRefresh.RepeatingGroup update)
        {
            int level = update.MDPriceLevel;

            if (update.MDEntryType == MDEntryType.Bid)
            {
                if (Bid[level].Price == null)
                {
                    New(header, update);
                    return;
                }

                Bid[level].Size = update.MDEntrySize;
                Bid[level].UpdateTime = header.SendingTime;
                Bid[level].NumberOfOrders = update.NumberOfOrders;
            }
            else if (update.MDEntryType == MDEntryType.Offer)
            {
                if (Ask[level].Price == null)
                {
                    New(header, update);
                    return;
                }

                Ask[level].Size = update.MDEntrySize;
                Ask[level].UpdateTime = header.SendingTime;
                Ask[level].NumberOfOrders = update.NumberOfOrders;
            }
        }

        // NOTE: In the context of the CME's data platform, a DELETE OrderAction means the deletion of an entire price level
        // from the book, not simply the deletion of a single order.  Deletions of single
        // orders (while the price level remains intact) will be communicated via the
        // CHANGE OrderAction.
        /// <summary>
        /// Removes a price level from the limit order book.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="update"></param>
        private void Delete(Header header, MarketDataIncrementalRefresh.RepeatingGroup update)
        {
            int level = update.MDPriceLevel;

            if (update.MDEntryType == MDEntryType.Bid)
            {
                // TO DO: Find a better way to implement below.
                //if (Bid[level].Price != update.MDEntryPx)
                //{
                //    throw new InvalidOperationException("Rebuild operation is attempting to delete a price level (" + update.MDEntryPx + ") that does not exist in the order book!");
                //}

                ShiftDepthLevelsUp(Bid, header.SendingTime, from: level, to: Depth);

                if (level != Depth) return;

                Bid[level].Delete(header.SendingTime);
            }
            else if (update.MDEntryType == MDEntryType.Offer)
            {
                // TO DO: Find a better way to implement below.
                //if (Ask[level].Price != update.MDEntryPx)
                //{
                //    throw new InvalidOperationException("Rebuild operation is attempting to delete a price level (" + update.MDEntryPx + ") that does not exist in the order book!");
                //}

                ShiftDepthLevelsUp(Ask, header.SendingTime, from: level, to: Depth);

                if (level != Depth) return;

                Ask[level].Delete(header.SendingTime);
            }
        }

        /// <summary>
        /// Consolidates the implied book and the outright book into a single limit order book.
        /// </summary>
        /// <param name="outrightOrderBook"></param>
        /// <param name="impliedOrderBook"></param>
        public void Consolidate(OrderBook outrightOrderBook, OrderBook impliedOrderBook)
        {
            int outrightDepth = outrightOrderBook.Depth;
            int impliedDepth = impliedOrderBook.Depth;

            CopyAllDepth(outrightOrderBook, outrightDepth);
            MergeAllDepth(impliedOrderBook, outrightDepth, impliedDepth);
            
            outrightOrderBook.LastTradedPrice.CopyInto(LastTradedPrice);

            UpdateTime = (outrightOrderBook.UpdateTime > impliedOrderBook.UpdateTime ? outrightOrderBook.UpdateTime : impliedOrderBook.UpdateTime);
        }

        /// <summary>
        /// Populates the LastTradedPrice property of the order book.
        /// </summary>
        /// <param name="price"></param>
        /// <param name="time"></param>
        /// <param name="size"></param>
        public void CommitTrade(int price, DateTime time, int size)
        {
            LastTradedPrice.Price = price;
            LastTradedPrice.UpdateTime = time;
            LastTradedPrice.Size = size;
        }

        /// <summary>
        /// Returns 'true' if the InstrumentId property of the order book is pertinent, according to the collection of pertinent instrument IDs provided as an argument.
        /// </summary>
        /// <param name="pertinentContracts"></param>
        /// <returns></returns>
        public bool IsPertinent(List<int> pertinentContracts, DateTime currentTime)
        {
            if (currentTime.IsWithinCacheRangeOf(previousCallTime))
            {
                return isPertinent;
            }

            isPertinent = false;
            foreach (int pertinentInstrumentId in pertinentContracts)
            {
                if (InstrumentId == pertinentInstrumentId)
                {
                    isPertinent = true;
                    break;
                }
            }

            previousCallTime = currentTime;

            return isPertinent;
        }

        private void InitializeOrderBook(int depth)
        {
            Bid = new PriceLevel[11];
            Ask = new PriceLevel[11];

            for (int i = 0; i < depth + 1; i++)
            {
                Bid[i] = PriceLevel.GetDummy();
                Ask[i] = PriceLevel.GetDummy();
            }
        }

        private void ShiftDepthLevelsUp(PriceLevel[] depth, DateTime updateTime, int from, int to)
        {
            for (int i = from; i < to; i++)
            {
                depth[i + 1].CopyInto(depth[i]);

                if (i == Depth - 1)
                {
                    depth[Depth].Delete(updateTime);
                }
            }
        }

        private void ShiftDepthLevelsDown(PriceLevel[] depth, int from, int to)
        {
            for (int i = from; i > to; i--)
            {
                depth[i - 1].CopyInto(depth[i]);
            }
        }

        private void CopyAllDepth(OrderBook outrightOrderBook, int outrightDepth)
        {
            for (int i = 1; i < outrightDepth + 1; i++)
            {
                outrightOrderBook.Bid[i].CopyInto(Bid[i]);
                outrightOrderBook.Ask[i].CopyInto(Ask[i]);
            }
        }

        private void MergeBids(OrderBook impliedOrderBook, int outrightDepth, int impliedDepth)
        {
            int previousMatch = 1;

            for (int i = 1; i < impliedDepth + 1; i++)
            {
                int? impliedPrice = impliedOrderBook.Bid[i].Price;
                if (impliedPrice == null) 
                    continue;

                int match = GetMatchingOutrightLevel(Bid, impliedPrice, outrightDepth, ref previousMatch);

                if (match != NOT_FOUND)
                {
                    MergeDepth(impliedOrderBook.Bid[i], Bid[i]);
                }
                else
                {
                    int insertionPoint = FindImpliedBidInsertionPoint(impliedPrice, outrightDepth);

                    if (insertionPoint == NOT_FOUND) 
                        continue;
                    
                    if (Bid[insertionPoint].Price == null)
                    {
                        InsertIntoNullLevel(impliedOrderBook.Bid[i], Bid[insertionPoint]);
                        continue;
                    }

                    InsertIntoNonNullLevel(Bid, impliedOrderBook.Bid[i], outrightDepth, insertionPoint);
                }
            }
        }

        private int GetMatchingOutrightLevel(PriceLevel[] consolidatedDepth, int? impliedPrice, int outrightDepth, ref int previousMatch)
        {
            for (int i = previousMatch; i < outrightDepth + 1; i++)
            {
                int? outrightPrice = consolidatedDepth[i].Price;

                if (outrightPrice == impliedPrice)
                {
                    previousMatch = i;
                    return i;
                }

                if (impliedPrice > outrightPrice)
                {
                    break;
                }
            }

            return 0;
        }

        private void MergeDepth(PriceLevel impliedDepthLevel, PriceLevel consolidatedDepthLevel)
        {
            consolidatedDepthLevel.Size += impliedDepthLevel.Size;
            consolidatedDepthLevel.ImpliedSize = impliedDepthLevel.Size;

            if (impliedDepthLevel.UpdateTime > consolidatedDepthLevel.UpdateTime)
                consolidatedDepthLevel.UpdateTime = impliedDepthLevel.UpdateTime;

            consolidatedDepthLevel.HasImpliedSize = true;
        }

        private int FindImpliedBidInsertionPoint(int? impliedPrice, int outrightDepth)
        {
            for (int i = outrightDepth; i > 0; i--)
            {
                if (impliedPrice < Bid[i].Price && Bid[i].Price != null)
                {
                    break;
                }
                else if (impliedPrice > Bid[i].Price || Bid[i].Price == null)
                {
                    return i;
                }
            }

            return 0;
        }

        private void MergeAsks(OrderBook impliedOrderBook, int outrightDepth, int impliedDepth)
        {
            int previousMatch = 1;

            for (int i = 1; i < impliedDepth + 1; i++)
            {
                int? impliedPrice = impliedOrderBook.Ask[i].Price;
                if (impliedPrice == null)
                    continue;

                int match = GetMatchingOutrightLevel(Ask, impliedPrice, outrightDepth, ref previousMatch);

                if (match != NOT_FOUND)
                {
                    MergeDepth(impliedOrderBook.Ask[i], Ask[i]);
                }
                else
                {
                    int insertionPoint = FindImpliedAskInsertionPoint(impliedPrice, outrightDepth);

                    if (insertionPoint == NOT_FOUND) 
                        continue;

                    if (Ask[insertionPoint].Price == null)
                    {
                        InsertIntoNullLevel(impliedOrderBook.Ask[i], Ask[insertionPoint]);

                        continue;
                    }

                    InsertIntoNonNullLevel(Ask, impliedOrderBook.Ask[i], outrightDepth, insertionPoint);
                }
            }
        }

        private int FindImpliedAskInsertionPoint(int? impliedPrice, int outrightDepth)
        {
            for (int i = outrightDepth; i > 0; i--)
            {
                if (impliedPrice > Ask[i].Price && Ask[i].Price != null)
                {
                    break;
                }
                else if (impliedPrice < Ask[i].Price || Ask[i].Price == null)
                {
                    return i;
                }
            }

            return 0;
        }

        private void InsertIntoNullLevel(PriceLevel impliedDepthLevel, PriceLevel consolidatedDepthLevel)
        {
            impliedDepthLevel.CopyInto(consolidatedDepthLevel);
        }

        private void InsertIntoNonNullLevel(PriceLevel[] consolidatedDepth, PriceLevel impliedDepthLevel, int outrightDepth, int insertionPoint)
        {
            ShiftDepthLevelsDown(consolidatedDepth, from: outrightDepth, to: insertionPoint);

            impliedDepthLevel.CopyInto(consolidatedDepth[insertionPoint]);
        }

        private void MergeAllDepth(OrderBook impliedOrderBook, int outrightDepth, int impliedDepth)
        {
            MergeBids(impliedOrderBook, outrightDepth, impliedDepth);
            MergeAsks(impliedOrderBook, outrightDepth, impliedDepth);
        }
    }
}
