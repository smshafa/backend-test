using Microsoft.EntityFrameworkCore;

namespace backend_test.Repository
{
    public interface IGenericRepository<T, Y> where T : class
    {
        Task<T> Get(int id);
        IQueryable<T> GetAll();
        Task Add(T entity);
        Task<T> AddAsync(T entity);
        void Delete(T entity);
        T Update(T entity);
        DbSet<T> All { get; }
        T Find(Y id);

        IQueryable<T> Set();
    }
}