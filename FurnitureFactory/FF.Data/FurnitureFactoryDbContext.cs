using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FF.Data;
using FF.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FF.Models
{
    public class FurnitureFactoryDbContext : IdentityDbContext<ApplicationUser>
    {
        public FurnitureFactoryDbContext(DbContextOptions<FurnitureFactoryDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProduct> OrderedProducts { get; set; }

        public override int SaveChanges()
        {
            PopulateAuditableEntities();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            PopulateAuditableEntities();

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(c => c.Id)
                .IsRequired()
                .HasColumnType("int");

            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .IsRequired()
                .HasColumnType("int");

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal");

            modelBuilder.Entity<Client>()
                .Property(c => c.Id)
                .IsRequired()
                .HasColumnType("int");

            modelBuilder.Entity<Client>()
                .Property(c => c.Name)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            modelBuilder.Entity<Client>()
                .Property(c => c.ResponsiblePerson)
                .IsRequired();

            modelBuilder.Entity<Client>()
                .Property(c => c.Bulstat)
                .IsRequired()
                .HasColumnType("nvarchar(10)");

            modelBuilder.Entity<Order>()
                .Property(o => o.Id)
                .IsRequired()
                .HasColumnType("int");

            modelBuilder.Entity<Order>()
                .Property(o => o.InvoiceNumber)
                .IsRequired()
                .HasColumnType("nvarchar(10)");

            modelBuilder.Entity<OrderProduct>()
                        .HasKey(s => new { s.OrderId, s.ProductId });

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(c => c.Category)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

        private void PopulateAuditableEntities()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

            foreach (var item in entries)
            {
                var auditable = item.Entity as Auditable;
                if (auditable == null)
                {
                    continue;
                }

                if (item.State == EntityState.Added)
                {
                    auditable.CreatedAt = DateTimeOffset.UtcNow;
                    auditable.CreatedBy = "admin";
                }
                else if (item.State == EntityState.Modified)
                {
                    auditable.UpdatedAt = DateTimeOffset.UtcNow;
                    auditable.UpdatedBy = "moderator";
                }
            }
        }
    }
}
