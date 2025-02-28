using MagicShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop.Entity
{
    public class MagicShopContext : DbContext
    {
        public DbSet<CustomerAccountModel> Customers { get; set; }

        public DbSet<ProductModel> Products { get; set; }

        public DbSet<AccountModel> Users { get; set; }

        public MagicShopContext(DbContextOptions<MagicShopContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountModel>()
                .Property(u => u.Login)
                    .HasMaxLength(128)
                    .IsRequired();

            modelBuilder.Entity<AccountModel>()
                .Property(u => u.Email)
                    .HasMaxLength(150)
                    .IsRequired();

            modelBuilder.Entity<AccountModel>()
                //.Property(u => u.Password).IsRequired();
                .Property(u => u.PasswordHash)
                    .IsRequired();

            modelBuilder.Entity<AccountModel>()
                .Property(u => u.PasswordSalt)
                    .IsRequired();

            modelBuilder.Entity<AccountModel>()
                .HasIndex(u => u.Login)
                .IsUnique()
                .IncludeProperties(u => new { u.PasswordHash, u.PasswordSalt });

            modelBuilder.Entity<CustomerAccountModel>()
                .Property(c => c.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<CustomerAccountModel>()
                .Property(c => c.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<CustomerAccountModel>()
                .Property(c => c.PhoneNumber).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<CustomerAccountModel>()
                .Property(c => c.Address).IsRequired().HasMaxLength(250);

            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Name).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Description).IsRequired(false).HasMaxLength(500);
            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Stock).IsRequired().HasDefaultValue(0);
            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Price).IsRequired().HasDefaultValue(0.0m);
            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Created).IsRequired().HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ProductModel>().OwnsOne(p => p.Characteristics)
                .Property(c => c.Width).HasDefaultValue(0.0);
            modelBuilder.Entity<ProductModel>().OwnsOne(p => p.Characteristics)
                .Property(c => c.Height).HasDefaultValue(0.0);
            modelBuilder.Entity<ProductModel>().OwnsOne(p => p.Characteristics)
                .Property(c => c.Weight).HasDefaultValue(0.0);
            modelBuilder.Entity<ProductModel>().OwnsOne(p => p.Characteristics)
                .Property(c => c.Long).HasDefaultValue(0.0);

            modelBuilder.Entity<ProductModel>().HasIndex(p => p.Name);

            base.OnModelCreating(modelBuilder);
        }
    }
}
