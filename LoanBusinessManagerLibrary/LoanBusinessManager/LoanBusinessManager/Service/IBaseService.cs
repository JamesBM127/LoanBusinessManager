using JBMDatabase;
using JBMDatabase.Business;
using System.Linq.Expressions;

namespace LBMLibrary.Service
{
    public interface IBaseService
    {
        Task<bool> AddAsync<TEntity, TBusiness>(TEntity entity, TBusiness business) where TEntity : BaseEntity
                                                                          where TBusiness : BaseBusiness;

        Task AddAsync<TEntity, TBusiness>(IEnumerable<TEntity> entities, TBusiness business) where TEntity : BaseEntity
                                                                                   where TBusiness : BaseBusiness;
        Task<TEntity> GetAsync<TEntity, TBusiness>(TBusiness business, Expression<Func<TEntity, bool>>? expression = null) where TEntity : BaseEntity
                                                                                                                 where TBusiness : BaseBusiness;
        Task<IReadOnlyCollection<TEntity>> ListAsync<TEntity, TBusiness>(TBusiness business, Expression<Func<TEntity, bool>>? expression = null) where TEntity : BaseEntity
                                                                                                                                       where TBusiness : BaseBusiness;
        bool Update<TEntity, TBusiness>(TEntity entity, TBusiness business) where TEntity : BaseEntity
                                                                  where TBusiness : BaseBusiness;

        bool Delete<TEntity, TBusiness>(TEntity entity, TBusiness business) where TEntity : BaseEntity
                                                                  where TBusiness : BaseBusiness;

        Task<object> DeleteAsync<TEntity, TBusiness>(Guid id, TBusiness business) where TEntity : BaseEntity
                                                                        where TBusiness : BaseBusiness;

        Task<bool> CommitAsync<TBusiness>(TBusiness business, bool commit, bool clearTrack = true) where TBusiness : BaseBusiness;

        Task<List<TEntity>> CommitAsync<TEntity, TBusiness>(TBusiness business, bool commit, bool clearTrack = true) where TBusiness : BaseBusiness
                                                                                                                     where TEntity : BaseEntity;

    }
}
