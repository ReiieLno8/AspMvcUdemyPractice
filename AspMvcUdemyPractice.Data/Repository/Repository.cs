using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AspMvcUdemyPractice.Data.Data;
using AspMvcUdemyPractice.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace AspMvcUdemyPractice.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;   // dependency injection
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>(); // same as _db.Categories. we are using this to access Generic Repository<T>
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        { 
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet; // dbSet already has the record so we have to convert and return back
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
 