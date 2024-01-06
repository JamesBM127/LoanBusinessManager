using LBMLibrary.Data.DataSettings;
using LBMLibrary.Data.DataSettings;
using Microsoft.EntityFrameworkCore;

namespace LBMLibrary.Data
{
    public class LBMContext : DbContext
    {
        public LBMContext(DbContextOptions<LBMContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.LoanModelBuilder();
            modelBuilder.RentModelBuilder();
            modelBuilder.DebtModelBuilder();
            modelBuilder.PaymentModelBuilder();
            modelBuilder.PersonModelBuilder();
            modelBuilder.PhoneModelBuilder();
            modelBuilder.DescriptionHistoryModelBuilder();
        }
    }
}
