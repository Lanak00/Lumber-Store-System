using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace LumberStoreSystem.DataAccess
{
    public class LumberStoreSystemContextFactory : IDesignTimeDbContextFactory<LumberStoreSystemDbContext>
    {
        public LumberStoreSystemDbContext CreateDbContext(string[] args)
        {
            string connectionString = args[0];
            var optionsBuilder = new DbContextOptionsBuilder<LumberStoreSystemDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            var context = new LumberStoreSystemDbContext(optionsBuilder.Options);

            return context;
        }
    }
}
