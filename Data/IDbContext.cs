using Microsoft.EntityFrameworkCore;

namespace backend_test.Data
{
    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;
        int SaveChanges();
        void Dispose();
    }
}