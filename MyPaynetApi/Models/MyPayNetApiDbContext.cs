using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyPayNetApi.Models
{
    public class MyPayNetApiDbContext : DbContext
    {
        public MyPayNetApiDbContext(DbContextOptions<MyPayNetApiDbContext> options)
            : base(options) { }

        public DbSet<Pay> Pays { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Pay>(f => f.Property(e => e.PaymentStatus)
                      .HasConversion<string>()
                      .IsRequired());
        }
    }

    public class Pay
    {
        public int Id { get; set; }

        public int RequestUniqueId { get; set; }

        public Guid TransactionId { get; set; }

        public decimal Total { get; set; }

        [Required, StringLength(16)]
        public string CardNumber { get; set; }

        [Required, StringLength(100)]
        public string CardHolderName { get; set; }

        [Required, StringLength(4)]
        public string Year { get; set; }

        [Required, StringLength(2)]
        public string Month { get; set; }

        [Required, StringLength(4)]
        public string CVV { get; set; }

        [Required, StringLength(10)]
        public string Bank { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public DateTime? CancelTransactionDate { get; set; }
    }

    public enum PaymentStatus
    {
        Pending=1,
        Completed,
        Failed,
        Cancelled
    }
}
