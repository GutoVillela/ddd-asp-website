using KadoshDomain.Enums;
using KadoshShared.Entities;

namespace KadoshDomain.Entities
{
    public class CustomerPosting : Entity
    {
        #region Constructor
        public CustomerPosting(Customer customer, ECustomerPostingType type, decimal value, Sale sale, DateTime postingDate)
        {
            Customer = customer;
            Type = type;
            Value = value;
            Sale = sale;
            PostingDate = postingDate;
        }
        #endregion Constructor

        #region Properties
        public Customer Customer { get; private set; }
        public ECustomerPostingType Type { get; private set; }
        public decimal Value { get; private set; }
        public Sale Sale { get; private set; }
        public DateTime PostingDate { get; set; }

        #endregion Properties

        #region Methods
        #endregion Methods
    }
}
