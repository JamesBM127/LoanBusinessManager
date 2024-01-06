using LBMLibrary.Entity;
using LBMLibrary.Data.DataSettings;
using Microsoft.EntityFrameworkCore;

namespace LBMLibrary.Data.DataSettings
{
    public static class RentDataSettings
    {
        public static void RentModelBuilder(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rent>(model =>
            {
                model.ToTable("Rent");

                model.Property(x => x.DueDate)
                     .HasColumnName("Due Date")
                     .HasColumnOrder(6)
                     .IsRequired();

                model.Property(x => x.HouseId)
                     .HasColumnName("House Id")
                     .HasColumnOrder(7)
                     .IsRequired();

                model.HasOne(x => x.House)
                     .WithOne(x => x.Rent)
                     .HasForeignKey<Rent>(x => x.HouseId);
            });

            modelBuilder.BusinessModelBuilder<Rent>();
        }
    }
}
