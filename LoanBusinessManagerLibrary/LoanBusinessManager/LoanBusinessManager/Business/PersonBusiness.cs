using LBMLibrary.Repository;
using JBMDatabase.Business;

namespace LBMLibrary.Business
{
    public class PersonBusiness : BaseBusiness
    {
        private readonly ILBMUoW UoW;

        public PersonBusiness(ILBMUoW uow) : base(uow)
        {
            UoW = uow;
        }
    }
}
