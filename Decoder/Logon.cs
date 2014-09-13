using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer.Decode
{
    /// <summary>
    /// Provides properties for storing and retrieving CME FIX logon fields.
    /// </summary>
    public sealed class Logon : Update, IParsable
    {
        private const int USERNAME_FIELD_LENGTH = 100;
        private const int PASSWORD_FIELD_LENGTH = 100;

        // From customer to CME
        public FieldInfo<char> Username { get; set; }
        public FieldInfo<char> Password { get; set; }
        public char DefaultApplVerID { get; set; } // This can also be sent from CME to customer.
        // From CME to customer
        public FieldInfo<char> ApplID { get; set; }
        public int EncryptMethod { get; set; }
        public int HeartBtInt { get; set; }

        /// <summary>
        /// Provides properties for storing and retrieving CME FIX logon fields.
        /// </summary>
        public Logon(FieldInfo<byte> tagInfo, FieldInfo<byte> valueInfo, Trailer trailer)
            : base(tagInfo, valueInfo, trailer)
        {
            Username = new FieldInfo<char>(USERNAME_FIELD_LENGTH);
            Password = new FieldInfo<char>(PASSWORD_FIELD_LENGTH);
            ApplID = new FieldInfo<char>(APPL_ID_FIELD_LENGTH);
        }

        /// <summary>
        /// Parses each field of a Logon update.
        /// </summary>
        public void Parse()
        {
            int tagLength = tag.Length;

            if (tagLength == 2)
            {
                if (IsTag[tagLength](Tag.Logon.EncryptMethod))
                {
                    EncryptMethod = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.Trailer.CheckSum))
                {
                    trailer.CheckSum = ParseIntField();
                }
            }
            else if (tagLength == 3)
            {
                if (IsTag[tagLength](Tag.Logon.HeartBtInt))
                {
                    HeartBtInt = ParseIntField();
                }
                else if (IsTag[tagLength](Tag.Logon.Username))
                {
                    ParseCharArrayField(Username);
                }
                else if (IsTag[tagLength](Tag.Logon.Password))
                {
                    ParseCharArrayField(Password);
                }
            }
            else if (tagLength == 4)
            {
                if (IsTag[tagLength](Tag.Logon.DefaultApplVerID))
                {
                    DefaultApplVerID = ParseCharField();
                }
                else if (IsTag[tagLength](Tag.Logon.ApplID))
                {
                    ParseCharArrayField(ApplID);
                }
            }
        }
    }
}
