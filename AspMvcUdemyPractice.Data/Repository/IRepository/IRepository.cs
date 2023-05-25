using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AspMvcUdemyPractice.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class // for explanation > section 5 > 69.repository pattern
    {
        // T - Category
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
 
    }
}
