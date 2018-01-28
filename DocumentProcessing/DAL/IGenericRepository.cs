using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.DAL
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetQueryable(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includedProperties = null,
            int? skip = null,
            int? take = null,
            bool asNoTracking = false);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includedProperties = null,
            int? skip = null,
            int? take = null,
            bool asNoTracking = false);

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includedProperties = null,
            int? skip = null,
            int? take = null,
            bool asNoTracking = false);

        TEntity GetFirst(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includedProperties = null,
            bool asNoTracking = false);

        Task<TEntity> GetFirstAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includedProperties = null,
            bool asNoTracking = false);

        TEntity GetById(object id);

        Task<TEntity> GetByIdAsync(object id);

        void Insert(TEntity entity);
        void Insert(ref TEntity entity);

        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        void Reload(ref TEntity entityToReload);
        void Save();
        Task SaveAsync();
    }
}
