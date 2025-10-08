using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace FakeShippingCompanyApi.Data;

public class FakeShippingCompanyDbContext : DbContext
{
    public FakeShippingCompanyDbContext(DbContextOptions<FakeShippingCompanyDbContext> options)
        : base(options) { }


    public DbSet<Shipment> Shipments { get; set; }
}

public class Shipment
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(150)]
    public string SenderCompany { get; set; }
    [Required, StringLength(2000)]
    public string SenderCompanyAdddress { get; set; }
    [Required, StringLength(2000)]
    public string SenderEmail { get; set; }
    [Required, StringLength(11)]
    public string SenderPhone { get; set; }
    [Required, StringLength(2000)]
    public string DeliveryTargetAddress { get; set; }
    [Required, StringLength(150)]
    public string DeliveryTargetFullName { get; set; }
    [Required, StringLength(100)]
    public string DeliveryTargetEmail { get; set; }
    [Required, StringLength(13)]
    public string DeliveryPhone { get; set; }
    public string TrackingNumber { get; set; } = string.Empty;
    public ShipmentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public enum ShipmentStatus
{
    Pending,
    InTransit,
    Delivered,
    Failed
}