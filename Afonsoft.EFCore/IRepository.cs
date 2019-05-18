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
    /// <typeparam name="TEntity">TEntity</typeparam>
    public interface IRepository<TEntity> where TEntity : class 
    {
        /// <summary>
        /// Add
        /// </summary>
        void Add(TEntity entity);
        /// <summary>
        /// Add Async
        /// </summary>
        /// <returns></returns>
        Task<int> AddAsync(TEntity entity);
        /// <summary>
        /// Add Range
        /// </summary>
        void AddRange(IEnumerable<TEntity> entity);
        /// <summary>
        /// Add Range Async
        /// </summary>
        Task<int> AddRangeAsync(IEnumerable<TEntity> entity);
        /// <summary>
        /// Get
        /// </summary>
        IEnumerable<TEntity> Get();
        /// <summary>
        /// Get Async
        /// </summary>
        Task<List<TEntity>> GetAsync();
        /// <summary>
        /// Get By Id (Primary Key)
        /// </summary>
        /// <param name="id">(Primary Key)</param>
        TEntity GetById(int id);
        /// <summary>
        /// Get By Id (Primary Key)
        /// </summary>
        /// <param name="id">(Primary Key)</param>
        Task<TEntity> GetByIdAsync(int id);
        /// <summary>
        /// Delete
        /// </summary>
        void Delete(TEntity entity);
        /// <summary>
        /// Delete Async
        /// </summary>
        Task<int> DeleteAsync(TEntity entity);
        /// <summary>
        /// Delete By Id (Primary Key)
        /// </summary>
        void DeleteById(int id);
        /// <summary>
        /// Delete By Id (Primary Key)
        /// </summary>
        Task<int> DeleteByIdAsync(int id);
        /// <summary>
        /// Delete Range
        /// </summary>
        void DeleteRange(Expression<Func<TEntity, bool>> filter);
        /// <summary>
        /// Delete Range Async
        /// </summary>
        Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> filter);
        /// <summary>
        /// Delete Range
        /// </summary>
        void DeleteRange(IEnumerable<TEntity> entity);
        /// <summary>
        /// Delete Range Async
        /// </summary>
        Task<int> DeleteRangeAsync(IEnumerable<TEntity> entity);
        /// <summary>
        /// Update
        /// </summary>
        void Update(TEntity entity);
        /// <summary>
        /// Update Async
        /// </summary>
        Task<int> UpdateAsync(TEntity entity);
        /// <summary>
        /// Update by Id (Primary Key)
        /// </summary>
        /// <param name="id">(Primary Key)</param>
        /// <param name="entity">TEntity</param>
        void UpdateById(TEntity entity, int id);
        /// <summary>
        /// Update by Id (Primary Key)
        /// </summary>
        /// <param name="id">(Primary Key)</param>
        /// <param name="entity">TEntity</param>
        Task<int> UpdateByIdAsync(TEntity entity, int id);
        /// <summary>
        /// Get
        /// </summary>
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> orderBy = null);
        /// <summary>
        /// Get Async
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> orderBy = null);
        /// <summary>
        /// GetPagination
        /// </summary>
        IEnumerable<TEntity> GetPagination(Expression<Func<TEntity, bool>> filter, int page = 1, int count = 10);
        /// <summary>
        /// GetPagination
        /// </summary>
        IEnumerable<TEntity> GetPagination(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, object>> orderBy, int page = 1, int count = 10);
       

    }
}
