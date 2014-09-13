using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Observer.Rebuild
{
    /// <summary>
    /// Provides methods and properties representing a tradable financial instrument.
    /// </summary>
    internal sealed class Instrument
    {
        /// <summary>
        /// The name of the instrument.  This property is assigned from the name of the instrument's configuration file.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Contains rollover dates and associated information for the given instrument.  This information is parsed from the instrument's configuration file.
        /// </summary>
        public RolloverMap RolloverMap { get; set; }

        /// <summary>
        /// A collection of Int32s representing the instrument IDs of the instrument's pertinent contracts.
        /// </summary>
        public List<int> PertinentContractMonths { get; set; }

        /// <summary>
        /// An Int32 representation of the instrument's tick size.
        /// </summary>
        public int TickSize { get; private set; }
        
        private DateTime previousCallTime;
        private List<int> pertinentContracts = new List<int>();
        
        /// <summary>
        /// Provides methods and properties representing a tradable financial instrument.
        /// </summary>
        /// <param name="filePath">The file path of the instrument's configuration file.</param>
        public Instrument(string filePath)
        {
            Name = GetNameFromFilePath(filePath);
            List<ContractMonth> months = ParseInstrumentFile(filePath);
            InsertMonthsIntoRolloverKey(months);
        }

        private string GetNameFromFilePath(string filePath)
        {
            FileInfo keyFile = new FileInfo(filePath);

            return keyFile.GetNameWithoutExtension();
        }

        private List<ContractMonth> ParseInstrumentFile(string filePath)
        {
            IEnumerable<string> linesFromFile = File.ReadLines(filePath);

            GetContractMonthsFromFile(linesFromFile);

            return GetRolloverDataFromFile(linesFromFile);
        }

        private void GetContractMonthsFromFile(IEnumerable<string> fileEnumerator)
        {
            PertinentContractMonths = new List<int>();

            int month = 0; // 0 means front month (i.e. the PertinentContractMonths collection is zero-based).
            foreach (string line in fileEnumerator)
            {
                if (line[0] == '*')
                {
                    PertinentContractMonths.Add(month);
                }

                month++;
            }
        }

        private List<ContractMonth> GetRolloverDataFromFile(IEnumerable<string> fileEnumerator)
        {
            List<ContractMonth> months = new List<ContractMonth>();

            foreach (string line in fileEnumerator)
            {
                string bareLine = line.Replace("*", "");
                string[] partitionedLine = bareLine.Split(' ');

                char[] contractName = partitionedLine[0].ToCharArray();
                DateTime contractRolloverDate = DateTime.Parse(partitionedLine[1]);

                months.Add(new ContractMonth(contractName, contractRolloverDate));
            }

            return months;
        }

        private void InsertMonthsIntoRolloverKey(List<ContractMonth> months)
        {
            RolloverMap = new RolloverMap();

            foreach (ContractMonth month in months)
            {
                KeyValuePair<int, DateTime> monthKey = new KeyValuePair<int,DateTime>(month.Name.GetId(), month.RolloverDate);
                RolloverMap.Add(monthKey);
            }
        }

        /// <summary>
        /// Selects the correct contract month given the input time.
        /// </summary>
        /// <param name="month">The month to be accessed.  Note that the front month contract has an index of zero.</param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        private int GetMonth(int month, DateTime currentTime)
        {
            int contractMonthCount = RolloverMap.Count;
            KeyValuePair<int, DateTime>? match = null;
            int frontMonthIndex = 0;
            for (int i = 0; i < contractMonthCount; i++)
            {
                if (RolloverMap[i].Value > currentTime)
                {
                    match = RolloverMap[i];
                    frontMonthIndex = i;
                    break;
                }
            }
            
            if (match == null)
            {
                if (month == 0)
                { 
                    throw new SystemException("EXCEPTION:  \"Month\" parameter cannot be zero.  To acces the front month, use a \"month\" value of 'one'.");
                }
                else 
                { 
                    throw new SystemException("EXCEPTION:  No valid contracts included in RolloverKey for instrument \"" + this.Name + "\"at " + currentTime.ToString() + ".");
                }
            }
            else
            {
                int addend = month - 1;
                int specifiedMonthIndex = frontMonthIndex + addend;

                return RolloverMap.ElementAt(specifiedMonthIndex).Key;
            }
        }

        /// <summary>
        /// Returns an up-to-date list of the instrument's pertinent contracts, as defined by the instrument's configuration file.
        /// </summary>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        public List<int> PertinentContracts(DateTime currentTime)
        {
            if (currentTime.IsWithinCacheRangeOf(previousCallTime))
            {
                return pertinentContracts;
            }

            pertinentContracts.Clear();
            
            foreach (int x in PertinentContractMonths)
            {
                pertinentContracts.Add(GetMonth(x, currentTime));
            }

            previousCallTime = currentTime;

            return pertinentContracts;
        }
    }    
}
