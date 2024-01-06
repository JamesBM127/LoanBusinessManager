using JBMDatabase;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBMLibrary.Entity.NotMappedClasses
{
    public abstract class NotMappedProperties : BaseHistoryEntity
    {
        [NotMapped]
        public int Counter { get; set; } = 1;
    }
}
