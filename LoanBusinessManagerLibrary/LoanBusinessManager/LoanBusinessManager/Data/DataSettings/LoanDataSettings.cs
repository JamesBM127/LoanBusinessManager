using LBMLibrary.Entity;
using Microsoft.EntityFrameworkCore;

namespace LBMLibrary.Data.DataSettings
{
    public static class LoanDataSettings
    {
        public static void LoanModelBuilder(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Loan>(model =>
            {
                model.ToTable("Loan");

                model.Property(x => x.Interest)
                     .HasColumnName("Interest")
                     .IsRequired();

                model.Ignore(x => x.Counter);
            });

            //modelBuilder.BusinessModelBuilder<Loan>();
        }
    }
}
