using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.DataAccesPatterns
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected DatabaseContext _context;

        public GenericRepository(DatabaseContext context)
        {
            _context = context;
        }

        public T Add(T entity)
        {
            return _context.Add(entity).Entity;
        }

        public virtual IEnumerable<T> All()
        {
            var a = _context.Set<T>()
                .ToList();
            return a;
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>()
                .AsQueryable()
                .Where(predicate);
        }

        public T Get(Guid id)
        {
            return _context.Find<T>(id);
        }

        public T Update(T entity)
        {
            return _context.Update(entity).Entity;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

    }
}
