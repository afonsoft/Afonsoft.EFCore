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
        public DbContext Context { get; }

        /// <summary>
        /// DbSet
        /// </summary>
        public DbSet<TEntity> DbSet { get; private set; }

        /// <summary>
        /// Primary Key Name
        /// </summary>
        public string PrimaryKeyName { get; }

        internal IEntityType _entityType;
        internal IEnumerable<IProperty> _properties;
        internal IModel _model;

        /// <summary>
        /// Construtor com o RepositoryDbContext
        /// </summary>
        /// <param name="dbContext"></param>
        public Repository(DbContext dbContext)
        {
            Context = dbContext;
            DbSet = Context.Set<TEntity>();

            _model = Context.Model;
            _entityType = _model.FindEntityType(typeof(TEntity));
            _properties = _entityType.GetProperties();
            PrimaryKeyName = _entityType.FindPrimaryKey().Properties.First().Name;
        }

        /// <summary>
        /// Add Element
        /// </summary>
        /// <param name="entity"></param>
        public virtual async void Add(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Add Many Element
        /// </summary>
        /// <param name="entity"></param>
        public virtual async void AddRange(IEnumerable<TEntity> entity)
        {
            await DbSet.AddRangeAsync(entity);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Get All Element
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get() => DbSet.AsNoTracking();

        /// <summary>
        /// Get Element by primaryKey
        /// </summary>
        /// <param name="id">key</param>
        /// <returns></returns>
        public virtual TEntity GetById(long id) =>
            (DbSet.FirstOrDefault(e => (long) e.GetType().GetProperty(PrimaryKeyName).GetValue(e) == id));

        /// <summary>
        /// Método que deleta um objeto no banco de dados. 
        /// </summary>
        /// <param name="entity">item que será deletado</param>
        public virtual async void Delete(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Método que deleta um objeto no banco de dados. 
        /// </summary>
        /// <param name="id">Id da primary key</param>
        public virtual async void DeleteById(long id)
        {
            var entity = GetById(id);
            if (entity == null)
                throw new KeyNotFoundException($"Id: {id} not found");
            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
        }

        /// <summary> 
        /// Método que deleta um ou varios objetos no banco de dados, mediante uma expressão LINQ. 
        /// </summary> 
        public virtual async void DeleteRange(Expression<Func<TEntity, bool>> filter)
        {
            DbSet.RemoveRange(DbSet.Where(filter));
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete Elements
        /// </summary>
        /// <param name="entity"></param>
        public virtual async void DeleteRange(IEnumerable<TEntity> entity)
        {
            DbSet.RemoveRange(entity);
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Update Element
        /// </summary>
        /// <param name="entity"></param>
        public virtual async void Update(TEntity entity)
        {
            //pegar o valor da pk do objeto
            var entry = Context.Entry(entity);
            var pkey = entity.GetType().GetProperty(PrimaryKeyName)?.GetValue(entity);

            if (entry.State == EntityState.Detached)
            {
                var set = Context.Set<TEntity>();
                TEntity attachedEntity = set.Find(pkey); // access the key 
                if (attachedEntity != null)
                {
                    var attachedEntry = Context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified; // attach the entity 
                }
            }

            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Update Element By primaryKey
        /// </summary>
        /// <param name="entity">Element</param>
        /// <param name="id">primaryKey</param>
        public virtual async void UpdateById(TEntity entity, long id)
        {
            TEntity attachedEntity = GetById(id); // access the key 
            if (attachedEntity != null)
            {
                var attachedEntry = Context.Entry(attachedEntity);
                attachedEntry.CurrentValues.SetValues(entity);
                attachedEntry.State = EntityState.Modified;
                await Context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Find Element
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <param name="orderBy">OrderBy</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
                query = query.Where(filter);

            return orderBy != null ? orderBy(query) : query;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            DbSet = null;
            Context?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Add
        /// </summary>
        public virtual Task<int> AddAsync(TEntity entity)
        {
            DbSet.AddAsync(entity).Wait();
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// add
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task<int> AddRangeAsync(IEnumerable<TEntity> entity)
        {
            DbSet.AddRangeAsync(entity).Wait();
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// get
        /// </summary>
        /// <returns></returns>
        public virtual Task<List<TEntity>> GetAsync() => DbSet.ToListAsync();

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<TEntity> GetByIdAsync(long id) =>
            (DbSet.FirstOrDefaultAsync(e => (long) e.GetType().GetProperty(PrimaryKeyName).GetValue(e) == id));

        /// <summary>
        /// delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteByIdAsync(long id)
        {
            var entity = GetById(id);
            if (entity == null)
                throw new KeyNotFoundException($"Id: {id} not found");
            DbSet.Remove(entity);
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> filter)
        {
            DbSet.RemoveRange(DbSet.Where(filter));
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteRangeAsync(IEnumerable<TEntity> entity)
        {
            DbSet.RemoveRange(entity);
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Udapte
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateAsync(TEntity entity)
        {
            var entry = Context.Entry(entity);
            var pkey = entity.GetType().GetProperty(PrimaryKeyName)?.GetValue(entity);

            if (entry.State == EntityState.Detached)
            {
                var set = Context.Set<TEntity>();
                TEntity attachedEntity = set.Find(pkey); // access the key 
                if (attachedEntity != null)
                {
                    var attachedEntry = Context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified; // attach the entity 
                }
            }

            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateByIdAsync(TEntity entity, long id)
        {
            TEntity attachedEntity = GetById(id) ?? entity;

            if (entity == null || attachedEntity == null)
                throw new KeyNotFoundException($"Id: {id} not found");

            var attachedEntry = Context.Entry(attachedEntity);
            attachedEntry.CurrentValues.SetValues(entity);
            attachedEntry.State = EntityState.Modified;

            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public virtual Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null) => orderBy != null
            ? orderBy(DbSet.Where(filter)).ToListAsync()
            : DbSet.Where(filter).ToListAsync();

        /// <summary>
        /// GetPagination
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pagina"></param>
        /// <param name="qtdRegistros"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetPagination(Expression<Func<TEntity, bool>> filter, 
            int pagina = 1,
            int qtdRegistros = 10) => DbSet.Where(filter).Skip((pagina - 1) * qtdRegistros).Take(qtdRegistros);

        /// <summary>
        /// GetPagination
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="pagina"></param>
        /// <param name="qtdRegistros"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetPagination(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, 
            int pagina = 1, 
            int qtdRegistros = 10) =>
            orderBy(DbSet.Where(filter).Skip((pagina - 1) * qtdRegistros).Take(qtdRegistros));

    }
}