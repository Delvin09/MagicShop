using MagicShop.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace MagicShop.Entity
{
    public class MagicShopContext : DbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public MagicShopContext(DbContextOptions opt)
            : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>()
                .Property(ua => ua.Login)
                .HasMaxLength(100);

            modelBuilder.Entity<UserAccount>()
                .HasIndex(ua => ua.Login)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
