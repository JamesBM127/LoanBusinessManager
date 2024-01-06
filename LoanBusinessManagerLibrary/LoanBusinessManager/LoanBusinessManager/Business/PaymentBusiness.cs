using LBMLibrary.Repository;
using JBMDatabase.Business;

namespace LBMLibrary.Business
{
    public class PaymentBusiness : BaseBusiness
    {
        private readonly ILBMUoW UoW;

        public PaymentBusiness(ILBMUoW uow) : base(uow)
        {
            UoW = uow;
        }
    }
}
