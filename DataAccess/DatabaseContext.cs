using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class DatabaseContext : DbContext
    {
        //Tracked
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }

        //Not tracked
        public DbSet<Stats> Stats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TestDB;");
        }

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            builder.Entity<Stats>().HasNoKey().ToView("Stats");
        }
    }
}
