using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode
{
    /// <summary>
    /// Base class from which all market data update types (MarketDataIncrementalRefresh, SecurityStatus, etc.) are derived.
    /// </summary>
    public abstract class Update
    {
        // This is the number of repeating groups which can be stored for a single message.  128 is an arbitrary number which I thought
        // wouldn't be exceeded in practice, but there's probably a better way to come up with this number.
        protected const int REPEATING_GROUP_ARRAY_LENGTH = 128;

        // These are the maximum lengths for the following fields, according to the 'SDKFFMessageSpecs2012.pdf' document (v2.20, 05/01/2012).
        protected const int APPL_ID_FIELD_LENGTH = 50;
        protected const int SYMBOL_FIELD_LENGTH = 6;
        protected const int SECURITY_ID_FIELD_LENGTH = 12;
        protected const int SECURITY_DESC_FIELD_LENGTH = 20;
        private const byte NEGATIVE_SIGN = 45;

        protected FieldInfo<byte> tag;
        protected FieldInfo<byte> value;
        protected Trailer trailer;
        private static readonly int[] power = new int[] { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000 };

        /// <summary>
        /// Base class from which all market data update types (MarketDataIncrementalRefresh, SecurityStatus, etc.) are derived.
        /// </summary>
        public Update()
        {
        }

        /// <summary>
        /// Base class from which all market data update types (MarketDataIncrementalRefresh, SecurityStatus, etc.) are derived.
        /// </summary>
        public Update(FieldInfo<byte> tagInfo, FieldInfo<byte> valueInfo, Trailer trailer)
        {
            tag = tagInfo;
            value = valueInfo;
            this.trailer = trailer;

            // The lambda function in index zero is just a dummy function that's there to align the TagContainer functions with their correct index positions.
            // This allows 'tag.Length' to be used as the indexer, rather than 'tag.Length - 1'.
            IsTag = new Func<byte[], bool>[5] { (a => a.IsDummy()), TagContainer1Is, TagContainer2Is, TagContainer3Is, TagContainer4Is };
        }

        protected bool TagContainer1Is(byte[] matchingTag)
        {
            return tag.Contents[0] == matchingTag[0];
        }
        protected bool TagContainer2Is(byte[] matchingTag)
        {
            if (tag.Contents[0] != matchingTag[0] || tag.Contents[1] != matchingTag[1])
                return false;

            return true;
        }
        protected bool TagContainer3Is(byte[] matchingTag)
        {
            if (tag.Contents[0] != matchingTag[0] || tag.Contents[1] != matchingTag[1] || tag.Contents[2] != matchingTag[2])
                return false;

            return true;
        }
        protected bool TagContainer4Is(byte[] matchingTag)
        {
            if (tag.Contents[0] != matchingTag[0] || tag.Contents[1] != matchingTag[1] || tag.Contents[2] != matchingTag[2] || tag.Contents[3] != matchingTag[3])
                return false;

            return true;
        }

        protected Func<byte[], bool>[] IsTag;

        protected int ParseIntField()
        {
            int valueLength = value.Length;
            int intContainer = 0;

            for (int i = 0; i < valueLength; i++)
            {
                intContainer += (value.Contents[i] - 48) * power[(valueLength - 1) - i];
            }

            return intContainer;
        }
        protected int ParseNegativeIntField()
        {
            int valueLength = value.Length;
            int intContainer = 0;
            byte currentValue = 0;
            bool isNegative = false;

            for (int i = 0; i < valueLength; i++)
            {
                currentValue = value.Contents[i];

                // Check for negative symbol.
                if (currentValue == NEGATIVE_SIGN)
                    isNegative = true;
                else
                    intContainer += (currentValue - 48) * power[(valueLength - 1) - i];
            }

            if (isNegative)
            {
                intContainer *= -1;
                isNegative = false;
            }

            return intContainer;
        }
        protected DateTime ParseDateField()
        {
            int dateTimeComponentYear = 0;
            int dtcYearStartIndex = 0;
            int dtcYearEndIndex = 4;
            int dateTimeComponentMonth = 0;
            int dtcMonthStartIndex = 4;
            int dtcMonthEndIndex = 6;
            int dateTimeComponentDay = 0;
            int dtcDayStartIndex = 6;
            int dtcDayEndIndex = 8;

            for (int i = dtcYearStartIndex; i < dtcYearEndIndex; i++)
            {
                dateTimeComponentYear += (value.Contents[i] - 48) * power[((dtcYearEndIndex - dtcYearStartIndex) - 1) - i];
            }
            for (int i = dtcMonthStartIndex; i < dtcMonthEndIndex; i++)
            {
                dateTimeComponentMonth += (value.Contents[i] - 48) * power[((dtcMonthEndIndex - dtcMonthStartIndex) - 1) - (i - dtcMonthStartIndex)];
            }
            for (int i = dtcDayStartIndex; i < dtcDayEndIndex; i++)
            {
                dateTimeComponentDay += (value.Contents[i] - 48) * power[((dtcDayEndIndex - dtcDayStartIndex) - 1) - (i - dtcDayStartIndex)];
            }

            return new DateTime(dateTimeComponentYear, dateTimeComponentMonth, dateTimeComponentDay);
        }
        protected TimeSpan ParseTimeWithMsField()
        {
            int dateTimeComponentHour = 0;
            int dtcHourStartIndex = 8;
            int dtcHourEndIndex = 10;
            int dateTimeComponentMinute = 0;
            int dtcMinuteStartIndex = 10;
            int dtcMinuteEndIndex = 12;
            int dateTimeComponentSecond = 0;
            int dtcSecondStartIndex = 12;
            int dtcSecondEndIndex = 14;
            int dateTimeComponentMillisecond = 0;
            int dtcMillisecondStartIndex = 14;
            int dtcMillisecondEndIndex = 17;

            for (int i = dtcHourStartIndex; i < dtcHourEndIndex; i++)
            {
                dateTimeComponentHour += (value.Contents[i] - 48) * power[((dtcHourEndIndex - dtcHourStartIndex) - 1) - (i - dtcHourStartIndex)];
            }
            for (int i = dtcMinuteStartIndex; i < dtcMinuteEndIndex; i++)
            {
                dateTimeComponentMinute += (value.Contents[i] - 48) * power[((dtcMinuteEndIndex - dtcMinuteStartIndex) - 1) - (i - dtcMinuteStartIndex)];
            }
            for (int i = dtcSecondStartIndex; i < dtcSecondEndIndex; i++)
            {
                dateTimeComponentSecond += (value.Contents[i] - 48) * power[((dtcSecondEndIndex - dtcSecondStartIndex) - 1) - (i - dtcSecondStartIndex)];
            }
            for (int i = dtcMillisecondStartIndex; i < dtcMillisecondEndIndex; i++)
            {
                dateTimeComponentMillisecond += (value.Contents[i] - 48) * power[((dtcMillisecondEndIndex - dtcMillisecondStartIndex) - 1) - (i - dtcMillisecondStartIndex)];
            }

            return new TimeSpan(0, dateTimeComponentHour, dateTimeComponentMinute, dateTimeComponentSecond, dateTimeComponentMillisecond);
        }
        protected DateTime ParseDateTimeWithMsField()
        {
            DateTime date = ParseDateField();
            TimeSpan time = ParseTimeWithMsField();
            
            return date + time;
        }
        protected char ParseCharField()
        {
            return (char)value.Contents[0];
        }
        protected void ParseCharArrayField(FieldInfo<char> writeValue)
        {
            int valueLength = value.Length;
            for (int i = 0; i < valueLength; i++)
            {
                writeValue.Contents[i] = (char)value.Contents[i];
            }

            writeValue.Length = valueLength;
        }
    }
}
