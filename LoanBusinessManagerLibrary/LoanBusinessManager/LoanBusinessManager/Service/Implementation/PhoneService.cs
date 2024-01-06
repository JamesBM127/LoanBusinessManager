using LBMLibrary.Entity;
using JBMDatabase;
using LBMLibrary.Business;
using System.Linq.Expressions;

namespace LBMLibrary.Service.Implementation
{
    public class PhoneService : BaseService
    {
        private readonly PhoneBusiness _phoneBusiness;

        public PhoneService(PhoneBusiness phoneBusiness)
        {
            _phoneBusiness = phoneBusiness;
        }

        public async Task<bool> AddAsync(Phone phone)
        {
            return await base.AddAsync<Phone, PhoneBusiness>(phone, _phoneBusiness);
        }

        public async Task AddAsync(IEnumerable<Phone> phones)
        {
            await base.AddAsync<Phone, PhoneBusiness>(phones, _phoneBusiness);
        }

        public async Task<Phone> GetAsync(Expression<Func<Phone, bool>>? expression = null)
        {
            return await base.GetAsync<Phone, PhoneBusiness>(_phoneBusiness, expression);
        }

        public async Task<IReadOnlyCollection<Phone>> ListAsync(Expression<Func<Phone, bool>>? expression = null)
        {
            return await base.ListAsync<Phone, PhoneBusiness>(_phoneBusiness, expression);
        }

        public bool Update<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            return base.Update(entity, _phoneBusiness);
        }

        public async Task<bool> CommitAsync(bool commit, bool clearTrack = false)
        {
            return await base.CommitAsync<PhoneBusiness>(_phoneBusiness, commit, clearTrack);
        }

        public async Task<List<Phone>> CommitReturnFailedItensAsync(bool commit, bool clearTrack = false)
        {
            return await base.CommitAsync<Phone, PhoneBusiness>(_phoneBusiness, commit, clearTrack);
        }

        public bool ObjectIsUnchanged(List<Guid> phonesIds)
        {
            return base.ObjectIsUnchanged<Phone, PhoneBusiness>(_phoneBusiness, phonesIds[0], phonesIds[1]);
        }

        public bool PhoneIsUnchanged(Phone phoneOriginal, Phone phoneEdited)
        {
            return (phoneOriginal.IsWhatsapp == phoneEdited.IsWhatsapp) && (phoneOriginal.PhoneNumber == phoneEdited.PhoneNumber);
        }
    }
}
