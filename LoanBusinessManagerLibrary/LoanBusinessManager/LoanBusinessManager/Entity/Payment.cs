using LBMLibrary.Entity.NotMappedClasses;

namespace LBMLibrary.Entity
{
    public class Payment : NotMappedProperties
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public Guid PersonId { get; set; }
        public Person Person { get; set; }

        public static void ClonePropertiesValues(ref Payment paymentDestination, Payment paymentSource)
        {
            paymentDestination.Person = paymentSource.Person;
            paymentDestination.PersonId = paymentSource.PersonId;
            paymentDestination.ModificationDate = paymentSource.ModificationDate;
            paymentDestination.HistoryType = paymentSource.HistoryType;
            paymentDestination.Amount = paymentSource.Amount;
            paymentDestination.PaymentDate = paymentSource.PaymentDate;
        }

        public static Payment ClonePropertiesValues(Payment paymentSource)
        {
            Payment paymentDestination = new();
            ClonePropertiesValues(ref paymentDestination, paymentSource);

            return paymentDestination;
        }
    }
}
