using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Afonsoft.EFCore
{
    /// <summary>
    /// Base para um DbSet
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// DbContext
        /// </summary>
        public RepositoryDbContext context { get; private set; }
        /// <summary>
        /// DbSet
        /// </summary>
        public DbSet<TEntity> dbSet { get; private set; }

        /// <summary>
        /// Primary Key Name
        /// </summary>
        public string PrimaryKeyName { get; private set; }

        internal IEntityType _entityType;
        internal IEnumerable<IProperty> _properties;
        internal IModel _model;
        
        /// <summary>
        /// Construtor com o RepositoryDbContext
        /// </summary>
        /// <param name="dbContext"></param>
        public Repository(RepositoryDbContext dbContext)
        {
            context = dbContext;
            dbSet = context.Set<TEntity>();

            _model = context.Model;
            _entityType = _model.FindEntityType(typeof(TEntity));
            _properties = _entityType.GetProperties();
            PrimaryKeyName = _entityType.FindPrimaryKey().Properties.First().Name;
        }

        /// <summary>
        /// Add Element
        /// </summary>
        /// <param name="entity"></param>
        public async void Add(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Add Many Element
        /// </summary>
        /// <param name="entity"></param>
        public async void AddRange(IEnumerable<TEntity> entity)
        {
            await dbSet.AddRangeAsync(entity);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Get All Element
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> Get()
        {
            return dbSet.AsNoTracking(); 
        }

        /// <summary>
        /// Get Element by primaryKey
        /// </summary>
        /// <param name="id">key</param>
        /// <returns></returns>
        public async Task<TEntity> GetById(long id)
        {
            return await (dbSet.FirstOrDefaultAsync(e => (long)e.GetType().GetProperty(PrimaryKeyName).GetValue(e) == id));
        }

        /// <summary>
        /// Método que deleta um objeto no banco de dados. 
        /// </summary>
        /// <param name="entity">item que será deletado</param>
        public async void Delete(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Método que deleta um objeto no banco de dados. 
        /// </summary>
        /// <param name="id">Id da primary key</param>
        public async void DeleteById(long id)
        {
            var entity = await GetById(id);
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }

        /// <summary> 
        /// Método que deleta um ou varios objetos no banco de dados, mediante uma expressão LINQ. 
        /// </summary> 
        public async void DeleteRange(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = dbSet;
            IQueryable<TEntity> deleteItens = query.Where(filter);
            dbSet.RemoveRange(deleteItens);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete Elements
        /// </summary>
        /// <param name="entity"></param>
        public async void DeleteRange(IEnumerable<TEntity> entity)
        {
            dbSet.RemoveRange(entity);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Update Element
        /// </summary>
        /// <param name="entity"></param>
        public async void Update(TEntity entity)
        {
            //pegar o valor da pk do objeto
            var entry = context.Entry(entity);
            var pkey = entity.GetType().GetProperty(PrimaryKeyName)?.GetValue(entity);

            if (entry.State == EntityState.Detached)
            {
                var set = context.Set<TEntity>();
                TEntity attachedEntity = set.Find(pkey);  // access the key 
                if (attachedEntity != null)
                {
                    var attachedEntry = context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified; // attach the entity 
                }
            }

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Update Element By primaryKey
        /// </summary>
        /// <param name="entity">Element</param>
        /// <param name="id">primaryKey</param>
        public async void UpdateById(TEntity entity, long id)
        {
            TEntity attachedEntity = await GetById(id);  // access the key 
            if (attachedEntity != null)
            {
                var attachedEntry = context.Entry(attachedEntity);
                attachedEntry.CurrentValues.SetValues(entity);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Find Element
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <param name="orderBy">OrderBy</param>
        /// <returns></returns>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            return orderBy != null ? orderBy(query) : query;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            dbSet = null;
            context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
