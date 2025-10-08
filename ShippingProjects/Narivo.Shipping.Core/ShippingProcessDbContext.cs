using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Narivo.Shipping.Core
{
    public class ShippingProcessDbContext : DbContext
    {
        public ShippingProcessDbContext(DbContextOptions<ShippingProcessDbContext> options)
            : base(options) { }

        public DbSet<Order> Orders => Set<Order>();
    }
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public string? ShippingCode { get; set; } // Kargo firmasının verdiği kod
        public ShippingStatus Status { get; set; } = ShippingStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
    public enum ShippingStatus
    {
        Pending,
        SentToCarrier,
        InTransit,
        Delivered,
        Failed
    }
    // DbContext
   
}
