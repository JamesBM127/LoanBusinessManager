using LBMLibrary.Entity.NotMappedClasses;

namespace LBMLibrary.Entity
{
    public abstract class BusinessClass : NotMappedProperties
    {
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
    }
}