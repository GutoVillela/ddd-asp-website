using Flunt.Notifications;
using Flunt.Validations;
using KadoshDomain.Entities;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.CustomerCommands.MergeCustomer
{
    public class MergeCustomerCommand : Notifiable<Notification>, ICommand
    {
        public int MainCustomerId { get; set; }

        /// <summary>
        /// Customers IDs to merge into Main Customer.
        /// </summary>
        public IList<int> CustomersToMerge { get; set; } = new List<int>();

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(CustomersToMerge, nameof(CustomersToMerge), CustomerValidationsErrors.NULL_CUSTOMERS_TO_MERGE)
                .IsNotEmpty(CustomersToMerge, nameof(CustomersToMerge), CustomerValidationsErrors.EMPTY_CUSTOMERS_TO_MERGE)
                .IsNotNull(MainCustomerId, nameof(MainCustomerId), CustomerValidationsErrors.NULL_MAIN_CUSTOMER_TO_MERGE)
                .IsFalse(MainCustomerId == 0, nameof(MainCustomerId), CustomerValidationsErrors.ZERO_MAIN_CUSTOMER_TO_MERGE)
            );
        }
    }
}

