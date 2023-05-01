using FluentNHibernate.Data;
using NHibernate;
using NHibernate.Linq;
using StackOverFlowClone.Infrastructure.Entities;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace StackOverFlowClone.Infrastructure.Repositories
{
    public abstract class Repository<T, TKey>
        : IRepository<T, TKey>
        where T : class, IEntity<TKey>
    {
        private readonly ISession _session;

        public Repository(ISession session)
        {
            _session = session;
        }

        public TKey Add(T entity)
        {
            _session.Save(entity);

            return entity.Id;
        }

        public bool Add(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                _session.Save(item);
            }

            return true;
        }

        public bool Update(T entity)
        {
            _session.Update(entity);

            return true;
        }

        public bool Update(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                _session.Update(item);
            }

            return true;
        }

        public bool Delete(T entity)
        {
            _session.Delete(entity);

            return true;
        }

        public bool DeleteById(TKey id)
        {
            T entity = FindBy(id);
            return Delete(entity);
        }

        public async Task<(IList<T> data, int total, int totalDisplay)> GetDynamicAsync(
            Expression<Func<T, bool>> filter = null!, string orderBy = null!, int pageIndex = 1, int pageSize = 10)
        {
            IQueryable<T> query = _session.Query<T>();
            var total = query.Count();
            var totalDisplay = query.Count();

            if (filter != null)
            {
                query = query.Where(filter);
                totalDisplay = query.Count();
            }

            return (query.ToList(), total, totalDisplay);
            //if (orderBy != null)
            //{
            //    var result = query.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            //    return ( result.ToList(), total, totalDisplay);
            //}
            //else
            //{
            //    var result = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            //    return (result.ToList(), total, totalDisplay);
            //}
        }
        public bool Delete(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                _session.Delete(entity);
            }

            return true;
        }

        public IQueryable<T> All()
        {
            return _session.Query<T>();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> expression)
        {
            return _session.Query<T>().Where(expression);
        }

        public T FindBy(TKey id)
        {
            return _session.Get<T>(id);
        }
    }
}
