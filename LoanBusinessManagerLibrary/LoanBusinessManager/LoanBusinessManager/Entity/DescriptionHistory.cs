using LBMLibrary.Entity.NotMappedClasses;
using LBMLibrary.Enums;

namespace LBMLibrary.Entity
{
    public class DescriptionHistory : NotMappedProperties
    {
        public Guid ItemId { get; set; }
        public string Description { get; set; }
        public ModificationType ModificationType { get; set; }
        public int Counter { get; set; } = 1;
    }
}
