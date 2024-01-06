using LBMLibrary.Repository;
using JBMDatabase.Business;

namespace LBMLibrary.Business
{
    public class LoanBusiness : BaseBusiness
    {
        private readonly ILBMUoW UoW;

        public LoanBusiness(ILBMUoW uow) : base(uow)
        {
            UoW = uow;
        }
    }
}
