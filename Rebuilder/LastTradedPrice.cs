using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Rebuild
{
    /// <summary>
    /// Represents the last traded price of an instrument.
    /// </summary>
    public sealed class LastTradedPrice
    {
        public int? Price { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Size { get; set; }

        /// <summary>
        /// Represents the last traded price of an instrument.
        /// </summary>
        public LastTradedPrice(int? price, DateTime time, int size)
        {
            Price = price;
            UpdateTime = time;
            Size = size;
        }

        /// <summary>
        /// Deep copies the calling LastTradedPrice to the LastTradedPrice object specified as a parameter.
        /// </summary>
        /// <param name="destinationLastTradedPrice"></param>
        public void CopyInto(LastTradedPrice destinationLastTradedPrice)
        {
            destinationLastTradedPrice.Price = Price;
            destinationLastTradedPrice.Size = Size;
            destinationLastTradedPrice.UpdateTime = UpdateTime;
        }

        /// <summary>
        /// Returns a new, "blank" LastTradedPrice object.
        /// </summary>
        /// <returns></returns>
        public static LastTradedPrice GetDummy()
        {
            return new LastTradedPrice(null, DateTime.MinValue, 0);
        }
    }
}
