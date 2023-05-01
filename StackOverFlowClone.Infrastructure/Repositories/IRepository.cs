using FluentNHibernate.Data;
using StackOverFlowClone.Infrastructure.Entities;
using System.Linq.Expressions;

namespace StackOverFlowClone.Infrastructure.Repositories
{
    public interface IRepository<T, TKey>
        where T : class, IEntity<TKey>
    {
        TKey Add(T entity);

        bool Add(IEnumerable<T> items);

        bool Update(T entity);

        bool Update(IEnumerable<T> items);

        bool Delete(T entity);
        bool DeleteById(TKey id);
        bool Delete(IEnumerable<T> entities);

        IQueryable<T> All();

        IEnumerable<T> FindBy(Expression<Func<T, bool>> expression);

        T FindBy(TKey id);

        Task<(IList<T> data, int total, int totalDisplay)> GetDynamicAsync(
            Expression<Func<T, bool>> filter = null!, string orderBy = null!, int pageIndex = 1, int pageSize = 10);
    }
}
