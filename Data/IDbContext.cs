using Microsoft.EntityFrameworkCore;

namespace backend_test.Data
{
    //https://subscription.packtpub.com/book/application_development/9781785883309/1/ch01lvl1sec16/implementing-the-unit-of-work-pattern
    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;
        int SaveChanges();
        void Dispose();
    }
}