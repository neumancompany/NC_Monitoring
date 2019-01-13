using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NC_Monitoring.Data.Interfaces
{
    public interface IRepository<TEntity, TKey> : ISelectRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        Task InsertAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TKey id);
    }
}