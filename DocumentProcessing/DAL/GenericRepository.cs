using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace DocumentProcessing.DAL
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
            where TEntity : class
    {
        private readonly DocumentProcessingEntities _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DocumentProcessingEntities context)
        {
            this._context = context;
            this._dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetQueryable(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includedProperties = null,
            int? skip = null,
            int? take = null,
            bool asNoTracking = false)
        {
            IQueryable<TEntity> query = _dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (includedProperties != null && includedProperties.Any())
            {
                includedProperties.ForEach(include => query = query.Include(include));
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includedProperties = null,
            int? skip = null,
            int? take = null,
            bool asNoTracking = false)
        {
            return GetQueryable(filter, orderBy, includedProperties, skip, take, asNoTracking).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includedProperties = null,
            int? skip = null,
            int? take = null,
            bool asNoTracking = false)
        {
            return await GetQueryable(filter, orderBy, includedProperties, skip, take, asNoTracking).ToListAsync();
        }

        public virtual TEntity GetFirst(
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy = null,
                List<Expression<Func<TEntity, object>>> includedProperties = null,
                bool asNoTracking = false)

        {
            return GetQueryable(filter, orderBy, includedProperties, asNoTracking: asNoTracking).FirstOrDefault();
        }

        public virtual async Task<TEntity> GetFirstAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includedProperties = null,
            bool asNoTracking = false)

        {
            return await GetQueryable(filter, orderBy, includedProperties, asNoTracking: asNoTracking).FirstOrDefaultAsync();
        }

        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Adds entity to the context without saving to DB
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        /// <summary>
        /// Add with immediately saving to db and reloading
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(ref TEntity entity)
        {
            _dbSet.Add(entity);
            Save();
            Reload(ref entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Reload(ref TEntity entityToReload)
        {
            _context.Entry(entityToReload).Reload();
        }

        public virtual void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                ThrowEnhancedValidationException(e);
            }
        }

        public virtual Task SaveAsync()
        {
            try
            {
                return _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                ThrowEnhancedValidationException(e);
            }

            return Task.FromResult(0);
        }

        protected virtual void ThrowEnhancedValidationException(DbEntityValidationException e)
        {
            var errorMessages = e.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            var fullErrorMessage = string.Join("; ", errorMessages);
            var exceptionMessage = string.Concat(e.Message, " The validation errors are: ", fullErrorMessage);
            throw new DbEntityValidationException(exceptionMessage, e.EntityValidationErrors);
        }
    }
}