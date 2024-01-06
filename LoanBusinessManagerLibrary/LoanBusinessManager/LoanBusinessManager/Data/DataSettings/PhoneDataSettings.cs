using LBMLibrary.Entity;
using JBMDatabase.Data.DataSettings;
using Microsoft.EntityFrameworkCore;

namespace LBMLibrary.Data.DataSettings
{
    public static class PhoneDataSettings
    {
        public static void PhoneModelBuilder(this ModelBuilder modelBuilder)
        {
            modelBuilder.BaseHistoryEntityBuilder<Phone>();

            modelBuilder.Entity<Phone>(model =>
            {
                model.ToTable("Phone");

                model.Property(x => x.PhoneNumber)
                     .HasColumnName("Phone Number")
                     .HasColumnType("varchar(14)")
                     .HasColumnOrder(2)
                     .IsRequired(false);

                model.Property(x => x.IsWhatsapp)
                     .HasColumnName("Is Whatsapp")
                     .HasColumnOrder(3)
                     .IsRequired();

                model.Property(x => x.PersonId)
                     .HasColumnName("Person Id")
                     .HasColumnOrder(4)
                     .IsRequired();

                model.HasOne(x => x.Person)
                     .WithMany(x => x.Phones)
                     .HasForeignKey(x => x.PersonId);
            });
        }
    }
}
