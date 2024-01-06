using LBMLibrary.Entity;
using LBMLibrary.Business;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace LBMLibrary.Service.Implementation
{
    public class LoanService : BaseService
    {
        private readonly LoanBusiness _loanBusiness;

        public LoanService(LoanBusiness loanBusiness)
        {
            _loanBusiness = loanBusiness;
        }

        public async Task<bool> AddAsync(Loan loan)
        {
            return await base.AddAsync<Loan, LoanBusiness>(loan, _loanBusiness);
        }

        public async Task<Loan> GetAsync(Expression<Func<Loan, bool>>? expression = null)
        {
            return await base.GetAsync<Loan, LoanBusiness>(_loanBusiness, expression);
        }

        public async Task<IReadOnlyCollection<Loan>> ListAsync(Expression<Func<Loan, bool>>? expression = null)
        {
            return await base.ListAsync<Loan, LoanBusiness>(_loanBusiness, expression);
        }

        public bool Update(Loan loan)
        {
            return base.Update(loan, _loanBusiness);
        }

        public async Task<bool> CommitAsync(bool commit, bool clearTrack = false)
        {
            return await base.CommitAsync<LoanBusiness>(_loanBusiness, commit, clearTrack);
        }

        public IEnumerable<EntityEntry> Track()
        {
            return _loanBusiness.TrackedItens();
        }
    }
}
