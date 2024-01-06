using LBMLibrary.Repository;
using JBMDatabase.Business;

namespace LBMLibrary.Business
{
    public class PhoneBusiness : BaseBusiness
    {
        private readonly ILBMUoW UoW;

        public PhoneBusiness(ILBMUoW uow) : base(uow)
        {
            UoW = uow;
        }
    }
}
