using JBMDatabase;
using JBMDatabase.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace LBMLibrary.Service
{
    public abstract class BaseService : IBaseService
    {
        public async Task<bool> AddAsync<TEntity, TBusiness>(TEntity entity, TBusiness business) where TEntity : BaseEntity
                                                                                                 where TBusiness : BaseBusiness
        {
            return await business.AddAsync(entity);
        }

        public async Task AddAsync<TEntity, TBusiness>(IEnumerable<TEntity> entities, TBusiness business)
            where TEntity : BaseEntity
            where TBusiness : BaseBusiness
        {
            await business.AddAsync(entities);
        }

        public async Task<TEntity> GetAsync<TEntity, TBusiness>(TBusiness business, Expression<Func<TEntity, bool>>? expression = null) where TEntity : BaseEntity
                                                                                                                                        where TBusiness : BaseBusiness
        {
            TEntity entity = await business.GetAsync(expression);
            return entity;
        }

        public async Task<IReadOnlyCollection<TEntity>> ListAsync<TEntity, TBusiness>(TBusiness business, Expression<Func<TEntity, bool>>? expression = null) where TEntity : BaseEntity
                                                                                                                                                              where TBusiness : BaseBusiness
        {
            IReadOnlyCollection<TEntity> entities = await business.ListAsync(expression);
            return entities;
        }

        public bool Update<TEntity, TBusiness>(TEntity entity, TBusiness business) where TEntity : BaseEntity
                                                                                                    where TBusiness : BaseBusiness
        {
            return business.Update(entity);
        }

        public bool Delete<TEntity, TBusiness>(TEntity entity, TBusiness business) where TEntity : BaseEntity
                                                                                                    where TBusiness : BaseBusiness
        {
            return business.Delete(entity);
        }

        public async Task<object> DeleteAsync<TEntity, TBusiness>(Guid id, TBusiness business) where TEntity : BaseEntity
                                                                                               where TBusiness : BaseBusiness
        {
            return await business.DeleteAsync<TEntity>(id);
        }

        public async Task<bool> CommitAsync<TBusiness>(TBusiness business, bool commit, bool clearTrack) where TBusiness : BaseBusiness
        {
            return await business.CommitAsync(commit, clearTrack);
        }

        public async Task<List<TEntity>> CommitAsync<TEntity, TBusiness>(TBusiness business, bool commit, bool clearTrack) where TBusiness : BaseBusiness
                                                                                                                                  where TEntity : BaseEntity
        {
            return await business.CommitAsync<TEntity>(commit, clearTrack);
        }

        public static List<TEnum> GetEnumValues<TEnum>() where TEnum : struct
        {
            List<TEnum> enumValuesList = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
            return enumValuesList;
        }

        public IEnumerable<EntityEntry> TrackedItensList<TBusiness>(TBusiness business) where TBusiness : BaseBusiness
        {
            return business.TrackedItens();
        }

        public void ClearTrack<TBusiness>(TBusiness business) where TBusiness : BaseBusiness
        {
            business.ClearTrack();
        }

        public bool ObjectIsUnchanged<TEntity, TBusiness>(TBusiness business, params Guid[] ids) where TEntity : BaseEntity
                                                                                       where TBusiness : BaseBusiness
        {
            IEnumerable<EntityEntry> trackedItens = business.TrackedItens();
            trackedItens = trackedItens.Where(x => x.State == EntityState.Unchanged);
            bool unchangedObj = false;

            foreach (var item in trackedItens)
            {
                TEntity? entity = item.Entity as TEntity ?? null;

                try
                {
                    foreach (Guid idItem in ids)
                    {
                        if (entity.Id == idItem)
                        {
                            unchangedObj = true;
                            break;
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                    continue;
                }
            }

            return unchangedObj;
        }
    }
}
