using Microsoft.EntityFrameworkCore;
using Narivo.Catalog.API.Infastructure.Entities;

namespace Narivo.Catalog.API.Infastructure.Persistence;

public class CatalogDbContext:DbContext
{
    public DbSet<Product> Product { get; set; }

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
    : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Product>(f => f.Property(e => e.ProductType)
                      .HasConversion<string>()
                      .IsRequired());
    }
}
