using LBMLibrary.Data;
using JBMDatabase.UnitOfWork;

namespace LBMLibrary.Repository
{
    public class LBMUoW : UoW, ILBMUoW
    {
        public LBMUoW(LBMContext context) : base(context)
        {

        }
    }
}
