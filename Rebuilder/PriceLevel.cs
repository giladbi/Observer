using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Rebuild
{
    /// <summary>
    /// Represents a single price level within the limit order book.
    /// </summary>
    public sealed class PriceLevel
    {
        public int? Price { get; set; }
        public int Size { get; set; }
        public int ImpliedSize { get; set; }
        public DateTime UpdateTime { get; set; }
        public int NumberOfOrders { get; set; }
        public bool HasImpliedSize { get; set; }

        /// <summary>
        /// Represents a single price level within the limit order book.
        /// </summary>
        /// <param name="price"></param>
        /// <param name="size"></param>
        /// <param name="time"></param>
        /// <param name="numberOfOrders"></param>
        public PriceLevel(int? price, int size, DateTime time, int numberOfOrders)
        {
            Price = price;
            Size = size;
            UpdateTime = time;
            NumberOfOrders = numberOfOrders;
        }

        /// <summary>
        /// Represents a single price level within the limit order book.  This signature is only used in the Consolidated (outright + implied) Book.
        /// </summary>
        /// <param name="price"></param>
        /// <param name="size"></param>
        /// <param name="impliedSize"></param>
        /// <param name="time"></param>
        /// <param name="numberOfOrders"></param>
        public PriceLevel(int? price, int size, int impliedSize, DateTime time, int numberOfOrders)
        {
            Price = price;
            Size = size;
            ImpliedSize = impliedSize;
            UpdateTime = time;
            NumberOfOrders = numberOfOrders;
        }

        public bool IsImplied()
        {
            if ((NumberOfOrders == 0 && Price != null) || HasImpliedSize)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Resets all properties of the given PriceLevel to their defaults (except for the 'Time' property), and updates the 'Time' property.
        /// </summary>
        /// <param name="time"></param>
        public void Delete(DateTime time)
        {
            Price = null;
            Size = 0;
            ImpliedSize = 0;
            UpdateTime = time;
            NumberOfOrders = 0;
        }

        /// <summary>
        /// Deep copies the calling PriceLevel to the PriceLevel object specified as a parameter.
        /// </summary>
        /// <param name="destinationPriceLevel"></param>
        public void CopyInto(PriceLevel destinationPriceLevel)
        {
            destinationPriceLevel.Price = Price;
            destinationPriceLevel.Size = Size;
            destinationPriceLevel.UpdateTime = UpdateTime;
            destinationPriceLevel.NumberOfOrders = NumberOfOrders;
        }

        /// <summary>
        /// Returns a new, "blank" PriceLevel object.
        /// </summary>
        /// <returns></returns>
        public static PriceLevel GetDummy()
        {
            return new PriceLevel(null, 0, 0, DateTime.MinValue, 0);
        }
    }
}
