using backend_test.Data;
using Microsoft.EntityFrameworkCore;

namespace backend_test.Repository
{
    public abstract class GenericRepository<T, Y> : IGenericRepository<T, Y> where T : class
    {
        protected readonly AirlineDbContext _context;

        protected GenericRepository(AirlineDbContext context)
        {
            _context = context;
        }

        public async Task<T> Get(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }

        public virtual DbSet<T> All
        {
            get { return _context.Set<T>(); }
        }

        public virtual T Find(Y id)
        {
            return _context.Set<T>().Find(id);
        }

        public IQueryable<T> Set()
        {
            return _context.Set<T>();
        }
    }
}