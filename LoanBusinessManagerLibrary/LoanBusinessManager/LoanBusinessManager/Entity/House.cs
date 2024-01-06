using LBMLibrary.Entity.NotMappedClasses;

namespace LBMLibrary.Entity
{
    public class House : NotMappedProperties
    {
        public string Landlord { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Neighborhood { get; set; }
        public string StreetName { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public Guid? RentId { get; set; }
        public Rent Rent { get; set; }
    }
}
