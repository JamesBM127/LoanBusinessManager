using LBMLibrary.Entity;
using JBMDatabase.Data.DataSettings;
using Microsoft.EntityFrameworkCore;

namespace LBMLibrary.Data.DataSettings
{
    public static class PersonDataSettings
    {
        public static void PersonModelBuilder(this ModelBuilder modelBuilder)
        {
            modelBuilder.BaseHistoryEntityBuilder<Person>();

            modelBuilder.Entity<Person>(model =>
            {
                model.ToTable("Person");

                model.Property(x => x.Name)
                     .HasColumnName("Name")
                     .HasColumnType("varchar(50)")
                     .HasColumnOrder(2)
                     .IsRequired(false);

                model.Property(x => x.Surname)
                     .HasColumnName("Surname")
                     .HasColumnType("varchar(50)")
                     .HasColumnOrder(3)
                     .IsRequired(false);

                model.Property(x => x.Nickname)
                     .HasColumnName("Nickname")
                     .HasColumnType("varchar(25)")
                     .HasColumnOrder(4)
                     .IsRequired(false);

                model.Property(x => x.PaymentStatus)
                     .HasColumnName("Payment Status")
                     .HasColumnOrder(5)
                     .IsRequired();

                model.Property(x => x.DebtId)
                     .HasColumnName("Debt Id")
                     .HasColumnOrder(6)
                     .IsRequired(false);

                model.HasMany(x => x.Phones)
                     .WithOne(x => x.Person)
                     .HasForeignKey(x => x.PersonId);

                model.HasMany(x => x.Payments)
                     .WithOne(x => x.Person)
                     .HasForeignKey(x => x.PersonId);

                model.HasMany(x => x.Loans)
                     .WithOne(x => x.Person)
                     .HasForeignKey(x => x.PersonId);

                model.HasOne(x => x.Debt)
                     .WithOne(x => x.Person)
                     .HasForeignKey<Debt>(x => x.PersonId);
            });
        }
    }
}