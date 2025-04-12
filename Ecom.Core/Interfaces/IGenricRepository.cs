using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
   public interface IGenricRepository<T> where T : class
    {
      public  Task<T> GetByIdAsync(int id);
      public  Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includePropiertes);

      public  Task<IReadOnlyList<T>> GetAllAsync();
      public  Task<IReadOnlyList<T>> GetAllAsyncWithModify(Expression<Func<T ,bool>>? expression,params Expression<Func<T, object>>[] includePropiertes);


        public Task<int> CountAsunc();


       public Task AddAsync(T entity);
       public Task UpdateAsync(T entity);
       public Task DeleteAsync(int id);
    }
}
