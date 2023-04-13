using Microsoft.EntityFrameworkCore;

namespace EuvicIntern.Entities
{
    public class EuvicDbContext : DbContext
    {
        public EuvicDbContext(DbContextOptions<EuvicDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.FirstName).IsRequired().HasMaxLength(15);
            modelBuilder.Entity<User>().Property(u => u.LastName).IsRequired().HasMaxLength(15);
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(35);
            modelBuilder.Entity<User>().Property(u => u.PhoneNumber).IsRequired();
        }

    }
}
