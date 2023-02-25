#nullable disable

using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Models;
using WebApplication1.Controllers;
using WebApplication1.Models;

namespace WebApplication1.Data;

public sealed class WebApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<SalePoint> SalePoints { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<FileData> FilesData { get; set; }

    public WebApplicationDbContext(DbContextOptions<WebApplicationDbContext> options)
        : base(options)
    {
        if (Database.GetPendingMigrations().Any())
            Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NumberProductCharacteristic>();
        modelBuilder.Entity<StringProductCharacteristic>();

        modelBuilder.Entity<Product>().HasQueryFilter(x => x.IsDeleted == null || x.IsDeleted == false);
        modelBuilder.Entity<SalePoint>().HasQueryFilter(x => x.IsDeleted == null || x.IsDeleted == false);
        modelBuilder.Entity<Manufacturer>().HasQueryFilter(x => x.IsDeleted == null || x.IsDeleted == false);
        modelBuilder.Entity<Customer>().HasQueryFilter(x => x.IsDeleted == null || x.IsDeleted == false);
        modelBuilder.Entity<Order>().HasQueryFilter(x => x.IsDeleted == null || x.IsDeleted == false);
    }
    
    public override int SaveChanges()
    {
        var insertedEntries = this.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity);

        foreach(var insertedEntry in insertedEntries)
        {
            if (insertedEntry is not DomainModel domainModelEntity)
            {
                continue;
            }

            domainModelEntity.CreatedDateTimeUtc = DateTime.UtcNow;
        }

        var modifiedEntries = this.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity);

        foreach (var modifiedEntry in modifiedEntries)
        {
            if (modifiedEntry is not DomainModel domainModelEntity)
            {
                continue;
            }
            
            domainModelEntity.UpdatedDateTimeUtc = DateTime.UtcNow;
        }

        return base.SaveChanges();
    }
}
