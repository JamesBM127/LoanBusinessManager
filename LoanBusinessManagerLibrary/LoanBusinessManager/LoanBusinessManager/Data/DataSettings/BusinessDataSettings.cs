using LBMLibrary.Entity;
using JBMDatabase.Data.DataSettings;
using Microsoft.EntityFrameworkCore;

namespace LBMLibrary.Data.DataSettings
{
    public static class BusinessDataSettings
    {
        public static void BusinessModelBuilder<TEntity>(this ModelBuilder modelBuilder) where TEntity : BusinessClass
        {
            modelBuilder.BaseHistoryEntityBuilder<TEntity>();

            modelBuilder.Entity<TEntity>(model =>
            {
                model.Property(x => x.Amount)
                     .HasColumnName("Amount")
                     .HasColumnOrder(2)
                     .IsRequired();

                model.Property(x => x.StartDate)
                     .HasColumnName("Start Date")
                     .HasColumnOrder(3)
                     .IsRequired();

                model.Property(x => x.PersonId)
                     .HasColumnName("Person Id")
                     .HasColumnOrder(4)
                     .IsRequired();
            });
        }
    }
}
