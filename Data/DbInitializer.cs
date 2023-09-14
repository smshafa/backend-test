using backend_test.Models;

namespace backend_test.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AirlineDbContext context)
        {
            context.Database.EnsureCreated();

            /*
            // Look for any students.
            if (context.Routes.Any())
            {
                return;   // DB has been seeded
            }

            var routes = new Route[]
            {
                new Route(),
                new Route(),
                new Route()
            };
            foreach (Route s in routes)
            {
                context.Routes.Add(s);
            }
            context.SaveChanges();
            */
        }
    }
}