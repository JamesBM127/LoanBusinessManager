using LBMLibrary.Entity.NotMappedClasses;

namespace LBMLibrary.Entity
{
    public class Phone : NotMappedProperties
    {
        public string PhoneNumber { get; set; }
        public bool IsWhatsapp { get; set; }
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
    }
}
