using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Rebuild
{
    /// <summary>
    /// Represents a map of rollover dates and associated information.
    /// </summary>
    internal class RolloverMap : IList<KeyValuePair<int, DateTime>>
    {
        private List<KeyValuePair<int, DateTime>> baseList = new List<KeyValuePair<int, DateTime>>();

        /// <summary>
        /// Represents a map of rollover dates and associated information.
        /// </summary>
        public RolloverMap()
        {
        }

        public int IndexOf(KeyValuePair<int, DateTime> item)
        {
            return baseList.IndexOf(item);
        }

        public void Insert(int index, KeyValuePair<int, DateTime> item)
        {
            baseList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            baseList.RemoveAt(index);
        }

        public KeyValuePair<int, DateTime> this[int index]
        {
            get
            {
                return baseList[index];
            }
            set
            {
                baseList[index] = value;
            }
        }

        /// <summary>
        /// Works the same as System.Collections.Generic.List's 'Add' function, but throws an exception if identical elements are added.
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<int, DateTime> item)
        {
            bool containsKey = false;
            foreach (KeyValuePair<int, DateTime> kvp in baseList)
            {
                if (item.Key == kvp.Key)
                {
                    containsKey = true;
                    break;
                }
            }

            if (containsKey)
                throw new SystemException("EXCEPTION: RolloverKey already contains key \"" + item.Key + "\".");
            else
                baseList.Add(item);
        }

        public void Clear()
        {
            baseList.Clear();
        }

        public bool Contains(KeyValuePair<int, DateTime> item)
        {
            return baseList.Contains(item);
        }

        public void CopyTo(KeyValuePair<int, DateTime>[] array, int arrayIndex)
        {
            baseList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return baseList.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((IList<KeyValuePair<int, DateTime>>)baseList).IsReadOnly; }
        }

        public bool Remove(KeyValuePair<int, DateTime> item)
        {
            return baseList.Remove(item);
        }

        public IEnumerator<KeyValuePair<int, DateTime>> GetEnumerator()
        {
            return baseList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return baseList.GetEnumerator();
        }
    }
}
