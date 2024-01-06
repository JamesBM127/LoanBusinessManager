using LBMLibrary.Entity;
using JBMDatabase.Data.DataSettings;
using Microsoft.EntityFrameworkCore;

namespace LBMLibrary.Data.DataSettings
{
    public static class DebtDataSettings
    {
        public static void DebtModelBuilder(this ModelBuilder modelBuilder)
        {
            modelBuilder.BaseHistoryEntityBuilder<Debt>();

            modelBuilder.Entity<Debt>(model =>
            {
                model.ToTable("Debt");

                model.Property(x => x.AmountRaw)
                     .HasColumnName("Amount Raw")
                     .HasColumnOrder(2)
                     .IsRequired();

                model.Property(x => x.InterestPerMonth)
                     .HasColumnName("Interest Per Month")
                     .HasColumnOrder(3)
                     .IsRequired();

                model.Property(x => x.DayOfMonthDueDate)
                     .HasColumnName("Day Of Month Due Date")
                     .HasColumnOrder(4)
                     .IsRequired(false);

                model.Property(x => x.PersonId)
                     .HasColumnName("Person Id")
                     .HasColumnOrder(5)
                     .IsRequired();

                model.Property(x => x.InterestType)
                     .HasColumnName("Interest Type")
                     .IsRequired();

                model.HasOne(x => x.Person)
                     .WithOne(x => x.Debt)
                     .HasForeignKey<Person>(x => x.DebtId);
            });
        }
    }
}
