using LBMLibrary.Entity;
using JBMDatabase.Data.DataSettings;
using Microsoft.EntityFrameworkCore;

namespace LBMLibrary.Data.DataSettings
{
    public static class PaymentDataSettings
    {
        public static void PaymentModelBuilder(this ModelBuilder modelBuilder)
        {
            modelBuilder.BaseHistoryEntityBuilder<Payment>();

            modelBuilder.Entity<Payment>(model =>
            {
                model.ToTable("Payment");

                model.Property(x => x.Amount)
                     .HasColumnName("Amount")
                     .HasColumnOrder(2)
                     .IsRequired();

                model.Property(x => x.PaymentDate)
                     .HasColumnName("Payment Date")
                     .HasColumnOrder(3)
                     .IsRequired();

                model.Property(x => x.PersonId)
                     .HasColumnName("Person Id")
                     .HasColumnOrder(4)
                     .IsRequired();

                model.HasOne(x => x.Person)
                     .WithMany(x => x.Payments)
                     .HasForeignKey(x => x.PersonId);
            });
        }
    }
}
