using LBMLibrary.Repository;
using JBMDatabase.Business;

namespace LBMLibrary.Business
{
    public class DescriptionHistoryBusiness : BaseBusiness
    {
        private readonly ILBMUoW UoW;

        public DescriptionHistoryBusiness(ILBMUoW uow) : base(uow)
        {
            UoW = uow;
        }
    }
}
