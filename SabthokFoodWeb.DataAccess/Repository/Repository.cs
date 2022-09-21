using Microsoft.EntityFrameworkCore;
using SabthokFoodWeb.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SabthokFoodWeb.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbset;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this.dbset = _context.Set<T>();
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeproperties = null)
        {
            IQueryable<T> query = dbset;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            if (includeproperties != null)
            {
                foreach(var includeprop in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T,bool>> filter, string? includeproperties = null)
        {
            IQueryable<T> query = dbset;
            query = query.Where(filter);
            if (includeproperties != null)
            {
                foreach (var includeprop in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbset.RemoveRange(entity);
        }
    }
}
