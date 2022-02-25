namespace KadoshShared.Enums.CustomAtributes
{
    public class BookEntryTypeAttribute : Attribute
    {
        private EBookEntryType _bookEntryType;

        public BookEntryTypeAttribute(EBookEntryType bookEntryType)
        {
            _bookEntryType = bookEntryType;
        }

        public EBookEntryType BookEntryType { get { return _bookEntryType; } }
    }
}
