using Microsoft.EntityFrameworkCore;
using Narivo.Membership.API.Infastructure.Entities;

namespace Narivo.Membership.API.Infastructure.Persistence;

public class MemberDbContext : DbContext
{
    public DbSet<Member> Members { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public MemberDbContext(DbContextOptions<MemberDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Card>(f => f.Property(e => e.Bank)
                      .HasConversion<string>()
                      .IsRequired());
    }
}
