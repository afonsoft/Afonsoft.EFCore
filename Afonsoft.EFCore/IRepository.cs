using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Afonsoft.EFCore
{
    /// <summary>
    /// IRepository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        Task<int> AddAsync(TEntity entity);
        void AddRange(IEnumerable<TEntity> entity);
        Task<int> AddRangeAsync(IEnumerable<TEntity> entity);
        IEnumerable<TEntity> Get();
        Task<List<TEntity>> GetAsync();
        TEntity GetById(long id);
        Task<TEntity> GetByIdAsync(long id);
        void Delete(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
        void DeleteById(long id);
        Task<int> DeleteByIdAsync(long id);
        void DeleteRange(Expression<Func<TEntity, bool>> filter);
        Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> filter);
        void DeleteRange(IEnumerable<TEntity> entity);
        Task<int> DeleteRangeAsync(IEnumerable<TEntity> entity);
        void Update(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        void UpdateById(TEntity entity, long id);
        Task<int> UpdateByIdAsync(TEntity entity, long id);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter = null,Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter = null,Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
    }
}
