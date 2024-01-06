using LBMLibrary.Entity;
using LBMLibrary.Business;
using System.Linq.Expressions;

namespace LBMLibrary.Service.Implementation
{
    public class DebtService : BaseService
    {
        private readonly DebtBusiness _debtBusiness;

        public DebtService(DebtBusiness debtBusiness)
        {
            _debtBusiness = debtBusiness;
        }

        public async Task<bool> AddAsync(Debt debt)
        {
            return await base.AddAsync<Debt, DebtBusiness>(debt, _debtBusiness);
        }

        public async Task<Debt> GetAsync(Expression<Func<Debt, bool>>? expression = null)
        {
            return await base.GetAsync<Debt, DebtBusiness>(_debtBusiness, expression);
        }

        public async Task<IReadOnlyCollection<Debt>> ListAsync(Expression<Func<Debt, bool>>? expression = null)
        {
            return await base.ListAsync<Debt, DebtBusiness>(_debtBusiness, expression);
        }

        public bool Update(Debt debt)
        {
            return base.Update(debt, _debtBusiness);
        }

        public async Task<bool> CommitAsync(bool commit, bool clearTrack = false)
        {
            return await base.CommitAsync<DebtBusiness>(_debtBusiness, commit, clearTrack);
        }
    }
}
