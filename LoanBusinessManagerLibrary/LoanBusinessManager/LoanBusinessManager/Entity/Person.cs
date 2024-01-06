using LBMLibrary.Entity.NotMappedClasses;
using LBMLibrary.Enums;

namespace LBMLibrary.Entity
{
    public class Person : NotMappedProperties
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }
        public Guid? DebtId { get; set; }
        public Debt? Debt { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public IEnumerable<Phone> Phones { get; set; }
        public IEnumerable<Payment> Payments { get; set; }
        public IEnumerable<Loan> Loans { get; set; }
    }
}
