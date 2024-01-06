using LBMLibrary.Entity;
using JBMDatabase.Data.DataSettings;
using Microsoft.EntityFrameworkCore;

namespace LBMLibrary.Data.DataSettings
{
    public static class DescriptionHistoryDataSettings
    {
        public static void DescriptionHistoryModelBuilder(this ModelBuilder modelBuilder)
        {
            modelBuilder.BaseHistoryEntityBuilder<DescriptionHistory>();

            modelBuilder.Entity<DescriptionHistory>(model =>
            {
                model.Property(x => x.ItemId)
                     .HasColumnName("Item Id")
                     .HasColumnOrder(2)
                     .IsRequired();

                model.Property(x => x.Description)
                     .HasColumnName("Description")
                     .HasColumnOrder(3)
                     .IsRequired();

                model.Property(x => x.ModificationType)
                     .HasColumnName("Modification Type")
                     .HasColumnOrder(4)
                     .IsRequired();

                //model.Ignore(x => x.Counter);
            });
        }
    }
}
