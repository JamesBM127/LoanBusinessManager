using LBMLibrary.Repository;
using JBMDatabase.Business;

namespace LBMLibrary.Business
{
    public class DebtBusiness : BaseBusiness
    {
        private readonly ILBMUoW UoW;

        public DebtBusiness(ILBMUoW uow) : base(uow)
        {
            UoW = uow;
        }
    }
}
