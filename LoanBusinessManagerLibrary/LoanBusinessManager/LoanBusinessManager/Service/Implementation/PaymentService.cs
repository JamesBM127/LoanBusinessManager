using LBMLibrary.Entity;
using LBMLibrary.Business;
using System.Linq.Expressions;

namespace LBMLibrary.Service.Implementation
{
    public class PaymentService : BaseService
    {
        private readonly PaymentBusiness _paymentBusiness;

        public PaymentService(PaymentBusiness paymentBusiness)
        {
            _paymentBusiness = paymentBusiness;
        }

        public async Task<bool> AddAsync(Payment person)
        {
            return await base.AddAsync(person, _paymentBusiness);
        }

        public async Task<Payment> GetAsync(Expression<Func<Payment, bool>>? expression = null)
        {
            return await base.GetAsync(_paymentBusiness, expression);
        }

        public async Task<IReadOnlyCollection<Payment>> ListAsync(Expression<Func<Payment, bool>>? expression = null)
        {
            return await base.ListAsync(_paymentBusiness, expression);
        }

        public bool Update(Payment person)
        {
            return base.Update(person, _paymentBusiness);
        }

        public async Task<bool> CommitAsync(bool commit, bool clearTrack = false)
        {
            return await base.CommitAsync(_paymentBusiness, commit, clearTrack);
        }
    }
}
