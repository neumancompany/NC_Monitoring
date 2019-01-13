using Microsoft.EntityFrameworkCore;
using NC_Monitoring.Data.Interfaces;
using NC_Monitoring.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NC_Monitoring.Data.Repositories
{

    public abstract class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        protected readonly ApplicationDbContext context;
        protected readonly DbSet<TEntity> dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = this.context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return dbSet.AsQueryable();
        }

        public virtual IQueryable<TEntity> GetAllOrderBy(Expression<Func<TEntity, object>> expression)
        {
            return dbSet.AsQueryable().OrderBy(expression);
        }

        public virtual TEntity FindById(TKey id)
        {
            return dbSet.Find(id);
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            entity.Id = GenerateNewKey();
            dbSet.Add(entity);
#if DEBUG
            try
            {
#endif
                await context.SaveChangesAsync();
#if DEBUG
            }
            catch { throw; }
#endif
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (dbSet.Contains(entity))
            {
                dbSet.Update(entity);
            }
            else
            {
                dbSet.Add(entity);
            }

            await context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            TEntity entity = dbSet.Find(id);
            try
            {
                dbSet.Remove(entity);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                context.Entry(entity).State = EntityState.Unchanged;
                throw;
            }
        }

        public virtual TKey GenerateNewKey()
        {
            if (typeof(TKey) == typeof(Guid))
            {
                return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(Guid.NewGuid().ToString());
            }

            return default(TKey);
        }
    }
}