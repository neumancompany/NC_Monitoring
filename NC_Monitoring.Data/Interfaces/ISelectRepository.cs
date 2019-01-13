using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NC_Monitoring.Data.Interfaces
{
    public interface ISelectRepository<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAllOrderBy(Expression<Func<TEntity, object>> expression);

        TEntity FindById(TKey id);

    }
}