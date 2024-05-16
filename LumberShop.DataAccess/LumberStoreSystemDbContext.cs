using Microsoft.EntityFrameworkCore;


namespace LumberStoreSystem.DataAccess
{
    public class LumberStoreSystemDbContext : DbContext
    {
        public LumberStoreSystemDbContext() { }

        public LumberStoreSystemDbContext(DbContextOptions<LumberStoreSystemDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.LogTo(Console.WriteLine);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
