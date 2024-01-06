using LBMLibrary.Entity;
using LBMLibrary.Business;
using System.Linq.Expressions;

namespace LBMLibrary.Service.Implementation
{
    public class DescriptionHistoryService : BaseService
    {
        private readonly DescriptionHistoryBusiness _descriptionHistoryBusiness;

        public DescriptionHistoryService(DescriptionHistoryBusiness descriptionHistoryBusiness)
        {
            _descriptionHistoryBusiness = descriptionHistoryBusiness;
        }

        public async Task<bool> AddAsync(DescriptionHistory descriptionHistory)
        {
            return await base.AddAsync(descriptionHistory, _descriptionHistoryBusiness);
        }

        public async Task<DescriptionHistory> GetAsync(Expression<Func<DescriptionHistory, bool>>? expression = null)
        {
            return await base.GetAsync(_descriptionHistoryBusiness, expression);
        }

        public async Task<IReadOnlyCollection<DescriptionHistory>> ListAsync(Expression<Func<DescriptionHistory, bool>>? expression = null)
        {
            return await base.ListAsync(_descriptionHistoryBusiness, expression);
        }

        public bool Update(DescriptionHistory descriptionHistory)
        {
            return base.Update(descriptionHistory, _descriptionHistoryBusiness);
        }

        public async Task<bool> CommitAsync(bool commit, bool clearTrack = false)
        {
            return await base.CommitAsync(_descriptionHistoryBusiness, commit, clearTrack);
        }
    }
}
