using Microsoft.EntityFrameworkCore;
//test
namespace DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Keyboard> Keyboards { get; set; }

        public DbSet<Stats> Stats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:dotnetlearning.database.windows.net,1433;Initial Catalog=testDB;Persist Security Info=False;User ID=jaap;Password=Wachtwoord123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"); //Server=(localdb)\\mssqllocaldb;Database=TestDB;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Stats>().HasNoKey().ToView("Stats");

            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Keyboard>().ToTable("Keyboards");
        }
    }
}
