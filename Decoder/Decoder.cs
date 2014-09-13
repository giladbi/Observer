using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Observer.Decode.Value;
using System.Globalization;

namespace Observer.Decode
{
    /// <summary>
    /// Provides methods and properties for decoding CME FIX 5.0 SP2 messages.
    /// </summary>
    public sealed class Decoder
    {
        private const int MAX_VALUE_LENGTH = 256;
        private const int MAX_TAG_LENGTH = 64;
        private const int READ_BUFFER_SIZE = 32768 * 2;
        private const byte LINE_FEED = 10;
        private const byte EQUALS_SIGN = 61;
        private const byte SOH = 1;
        private static readonly string SendingTime = "52=";
        private static readonly string MDUpdateAction = "279=";

        private FieldInfo<byte> value = new FieldInfo<byte>(MAX_VALUE_LENGTH);
        private FieldInfo<byte> tag = new FieldInfo<byte>(MAX_TAG_LENGTH);
        private bool shouldCaptureTag = true;
        private bool gotHeader = false;
        private DateTime initialTimestamp = DateTime.MinValue;

        // Pool of Message, Header, Trailer, and various Update type objects.  Using a pool of these objects is useful in avoiding generating excess garbage.
        private Trailer trailer;
        private Header header;
        private MarketDataIncrementalRefresh xupdate;
        private Logout logout;
        private Logon logon;
        private MarketDataRequest vupdate;
        private SecurityStatus fupdate;
        private SecurityDefinition dupdate;
        private QuoteRequest rupdate;
        private MarketDataSnapshotFullRefresh wupdate;
        private Message message;

        /// <summary>
        /// Represents methods which handle events related to Header objects.
        /// </summary>
        /// <param name="header"></param>
        public delegate void HeaderEventHandler(Header header);

        /// <summary>
        /// Fires when the header of a message has been parsed.
        /// </summary>
        public event HeaderEventHandler HeaderParsed = delegate { }; // Initializing to the empty anonymous method will ensure that the event is never null, even if no methods are subscribed to it.

        /// <summary>
        /// Wrapper around the 'HeaderParsed' event, called when the header of a message has been parsed.
        /// </summary>
        internal void OnHeaderParsed(Header header)
        {
            HeaderEventHandler invoker = HeaderParsed;

            invoker(header);
        }

        /// <summary>
        /// Represents methods which handle events related to Message objects.
        /// </summary>
        /// <param name="message"></param>
        public delegate void MessageEventHandler(Message message);

        /// <summary>
        /// Fires when an entire message has been completely parsed.
        /// </summary>
        public event MessageEventHandler MessageParsed = delegate { }; // Initializing to the empty anonymous method will ensure that the event is never null, even if no methods are subscribed to it.

        /// <summary>
        /// Represents methods which handle events related to Update objects.
        /// </summary>
        /// <param name="update"></param>
        public delegate void UpdateEventHandler(MarketDataIncrementalRefresh.RepeatingGroup update);

        /// <summary>
        /// Fires when a trade update block has been parsed.
        /// </summary>
        public event UpdateEventHandler TradeParsed = delegate { }; // Initializing to the empty anonymous method will ensure that the event is never null, even if no methods are subscribed to it.

        /// <summary>
        /// Fires when a quote update block has been parsed.
        /// </summary>
        public event UpdateEventHandler QuoteParsed = delegate { }; // Initializing to the empty anonymous method will ensure that the event is never null, even if no methods are subscribed to it.

        /// <summary>
        /// Wrapper around the 'TradeParsed' event, called when a trade update has been parsed.
        /// </summary>
        /// <param name="update"></param>
        internal void OnTradeParsed(MarketDataIncrementalRefresh.RepeatingGroup update)
        {
            UpdateEventHandler invoker = TradeParsed;

            invoker(update);
        }

        /// <summary>
        /// Wrapper around the 'QuoteParsed' event, called when a quote update has been parsed.
        /// </summary>
        /// <param name="update"></param>
        internal void OnQuoteParsed(MarketDataIncrementalRefresh.RepeatingGroup update)
        {
            UpdateEventHandler invoker = QuoteParsed;

            invoker(update);
        }

        /// <summary>
        /// Represents methods which handle events related to changes in the status of the decode operation.
        /// </summary>
        /// <param name="timestamp"></param>
        public delegate void DecodeStatusEventHandler(DateTime timestamp);

        /// <summary>
        /// Fires when the decode operation is complete.
        /// </summary>
        public event DecodeStatusEventHandler DecodeComplete = delegate { }; // Initializing to the empty anonymous method will ensure that the event is never null, even if no methods are subscribed to it.

        /// <summary>
        /// Fires when the first timestamp has been parsed from the first file to be processed.
        /// </summary>
        public event DecodeStatusEventHandler DecodeStarted = delegate { }; // Initializing to the empty anonymous method will ensure that the event is never null, even if no methods are subscribed to it.

        /// <summary>
        /// Provides methods and properties for decoding FIX messages.
        /// </summary>
        public Decoder()
        {
            trailer = new Trailer();
            header = new Header(tag, value, trailer, this);
            xupdate = new MarketDataIncrementalRefresh(tag, value, trailer, this);
            logout = new Logout(tag, value, trailer);
            logon = new Logon(tag, value, trailer);
            vupdate = new MarketDataRequest(tag, value, trailer);
            fupdate = new SecurityStatus(tag, value, trailer);
            dupdate = new SecurityDefinition(tag, value, trailer);
            rupdate = new QuoteRequest(tag, value, trailer);
            wupdate = new MarketDataSnapshotFullRefresh(tag, value, trailer);
            message = new Message();
        }

        /// <summary>
        /// Decodes a CME FIX 5.0 SP2 data set.
        /// </summary>
        /// <param name="rawFiles"></param>
        public void DecodeHistorical(IEnumerable<string> rawFiles)
        {
            PerformanceHelper.ApplyPerformanceSettings();

            CacheInitialTimestamp(rawFiles.First());

            foreach (string file in rawFiles)
            {
                ProcessFile(file);
            }

            DecodeComplete.Invoke(header.SendingTime);
        }

        private DateTime GetInitialTimestamp(string filePath)
        {
            CultureInfo tProvider = CultureInfo.InvariantCulture;
            DateTime initialTimestamp = DateTime.MinValue;

            using (StreamReader sr = new StreamReader(filePath))
            {
                string[] firstLine = sr.ReadLine().Split(new string[] { "\x01" + MDUpdateAction }, StringSplitOptions.None);
                string[] beginningOfFirstLine = firstLine[0].Split('\x01');

                foreach (string tagValuePair in beginningOfFirstLine)
                {
                    if (tagValuePair.Contains(SendingTime))
                    {
                        string timeString = tagValuePair.Split('=')[1];

                        initialTimestamp = DateTime.ParseExact(timeString, "yyyyMMddHHmmssfff", tProvider);
                    }
                }
            }

            return initialTimestamp;
        }

        private void CacheInitialTimestamp(string filePath)
        {
            if (initialTimestamp.IsUninitialized())
            {
                initialTimestamp = GetInitialTimestamp(filePath);
                DecodeStarted.Invoke(initialTimestamp);
            }
        }

        private void ResetDataBlockCounts()
        {
            xupdate.DataBlockCount = 0;
            wupdate.DataBlockCount = 0;
            dupdate.DataBlockEventCount = 0;
            dupdate.DataBlockMDFeedTypeCount = 0;
        }

        internal void ProcessUpdate(FieldInfo<byte> valueInfo)
        {
            if (header.MsgType == MsgType.MarketDataIncrementalRefresh)
            {
                xupdate.Parse();
            }
            else if (header.MsgType == MsgType.Heartbeat)
            {
                // I guess you would send a heartbeat message back to the sender here?
            }
            else if (header.MsgType == MsgType.MarketDataSnapshotFullRefresh)
            {
                wupdate.Parse();
            }
            else if (header.MsgType == MsgType.MarketDataRequest)
            {
                vupdate.Parse();
            }
            else if (header.MsgType == MsgType.MarketDataRequestReject)
            {
                // Not described in the MDP message specification.
            }
            else if (header.MsgType == MsgType.SecurityDefinition)
            {
                dupdate.Parse();
            }
            else if (header.MsgType == MsgType.SecurityStatus)
            {
                fupdate.Parse();
            }
            else if (header.MsgType == MsgType.QuoteRequest)
            {
                rupdate.Parse();
            }
            else if (header.MsgType == MsgType.Logon)
            {
                logon.Parse();
            }
            else if (header.MsgType == MsgType.Logout)
            {
                logout.Parse();
            }
            else
            {
                throw new ApplicationException("Unknown message type encountered, application shutting down.  MsgType in question: " + header.MsgType.ToString());
            }
        }

        private void BuildMessage()
        {
            if (header.MsgType == MsgType.MarketDataIncrementalRefresh)
            {
                message.Body = xupdate;
            }
            else if (header.MsgType == MsgType.Heartbeat)
            {
                // I guess you would send a heartbeat message back to the sender here, if this were to be used as part of a live FIX engine.
            }
            else if (header.MsgType == MsgType.MarketDataSnapshotFullRefresh)
            {
                throw new NotImplementedException("MarketDataSnapshotFullRefresh update type not yet implemented.");
            }
            else if (header.MsgType == MsgType.MarketDataRequest)
            {
                message.Body = vupdate;
            }
            else if (header.MsgType == MsgType.MarketDataRequestReject)
            {
                // Not described in the MDP message specification.
            }
            else if (header.MsgType == MsgType.SecurityDefinition)
            {
                throw new NotImplementedException("SecurityDefinition update type not yet implemented.");
            }
            else if (header.MsgType == MsgType.SecurityStatus)
            {
                message.Body = fupdate;
            }
            else if (header.MsgType == MsgType.QuoteRequest)
            {
                throw new NotImplementedException("QuoteRequest update type not yet implemented.");
            }
            else if (header.MsgType == MsgType.Logon)
            {
                message.Body = logon;
            }
            else if (header.MsgType == MsgType.Logout)
            {
                message.Body = logout;
            }

            message.Header = header;
            message.Trailer = trailer;

            //Reset the properties/fields below, since they may not get read in on each new message.
            header.LastMsgSeqNumProcessed = NullEnumerations.IntEnum;
            header.PossDupFlag = NullEnumerations.Char;
            header.MDSecurityTradingStatus = NullEnumerations.IntEnum;
        }

        private void DecodeBuffered(byte[] buffer, long lengthOfFile, ref long lengthRead)
        {
            byte currentByte;

            for (int i = 0; i < READ_BUFFER_SIZE; i++)
            {
                if (lengthRead++ == lengthOfFile - 1)
                    break;

                currentByte = buffer[i];

                if (currentByte == EQUALS_SIGN)
                {
                    // Tag has been read in.

                    shouldCaptureTag = false;
                }
                else if (currentByte == SOH)
                {
                    // Value has been read in.

                    if (!gotHeader)
                    {
                        gotHeader = header.IsHeaderEnd();
                    }
                    else
                    {
                        ProcessUpdate(value);
                    }

                    tag.Length = 0;
                    value.Length = 0;
                    shouldCaptureTag = true;
                }
                else if (currentByte == LINE_FEED)
                {
                    BuildMessage();
                    MessageParsed.Invoke(message);

                    ResetDataBlockCounts();
                    gotHeader = false;
                }
                else
                {
                    if (shouldCaptureTag)
                    {
                        tag.Contents[tag.Length] = currentByte;
                        tag.Length++;
                    }
                    else
                    {
                        value.Contents[value.Length] = currentByte;
                        value.Length++;
                    }
                }
            }
        }

        private void ProcessFile(string filePath)
        {
            //Console.WriteLine("Processing: " + filePath);

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                byte[] buffer = new byte[READ_BUFFER_SIZE];
                long lengthOfFile = fs.Length;
                long lengthRead = 0;
                while (fs.Read(buffer, 0, READ_BUFFER_SIZE) != 0)
                {
                    DecodeBuffered(buffer, lengthOfFile, ref lengthRead);
                }
            }
        }
    }
}
