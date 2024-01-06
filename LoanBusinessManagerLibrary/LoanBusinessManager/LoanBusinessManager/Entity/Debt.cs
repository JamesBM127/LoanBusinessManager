using LBMLibrary.Entity.NotMappedClasses;
using LBMLibrary.Enums;

namespace LBMLibrary.Entity
{
    public class Debt : NotMappedProperties
    {
        public decimal AmountRaw { get; set; }
        public decimal InterestPerMonth { get; set; }
        public byte? DayOfMonthDueDate { get; set; }
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
        public InterestType InterestType { get; set; } = InterestType.Simple;
    }
}
