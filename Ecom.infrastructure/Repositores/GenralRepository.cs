using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositores
{
    public class GenralRepository<T> : IGenricRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenralRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsunc()=> await _context.Set<T>().CountAsync();


        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();

        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
            =>await _context.Set<T>().AsNoTracking().ToListAsync();
        

        public async Task<IReadOnlyList<T>> GetAllAsyncWithModify(Expression<Func<T, bool>>? expression, params Expression<Func<T, object>>[] includePropiertes)
        {
            var query=_context.Set<T>().AsQueryable();
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (includePropiertes != null)
            {
                foreach(var property in includePropiertes)
                {
                    query = query.Include(property);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        =>await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includePropiertes)
        {

            var query = _context.Set<T>().AsQueryable();

            if (includePropiertes != null)
            {
                foreach (var property in includePropiertes)
                {
                    query = query.Include(property);
                }
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);

        }

        public async Task UpdateAsync(T entity)
        {
         _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
