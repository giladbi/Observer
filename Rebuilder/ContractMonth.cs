using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Rebuild
{
    /// <summary>
    /// Represents both the name and the rollover date of a given contract month.
    /// </summary>
    internal sealed class ContractMonth
    {
        public char[] Name { get; set; }

        /// <summary>
        /// The date on which the given contract month is no longer valid.
        /// </summary>
        public DateTime RolloverDate { get; set; }

        /// <summary>
        /// Represents both the name and the rollover date of a given contract month.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rolloverDate">The date on which the given contract month is no longer valid.</param>
        public ContractMonth(char[] name, DateTime rolloverDate)
        {
            Name = name;
            RolloverDate = rolloverDate;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) 
                return false;
            if (obj.GetType() != this.GetType()) 
                return false;

            ContractMonth month = obj as ContractMonth;

            return this.Name == month.Name && this.RolloverDate == month.RolloverDate;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (RolloverDate.GetHashCode()) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }
}
