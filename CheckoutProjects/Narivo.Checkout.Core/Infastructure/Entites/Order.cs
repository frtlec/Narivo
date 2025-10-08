using Narivo.Checkout.Core.Infastructure.Enums;
using Narivo.Shared.Dtos;
using Narivo.Shared.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Narivo.Checkout.Core.Infastructure.Entites;

public class Order : BaseEntity
{
    public Order()
    {
        this.Items = new HashSet<OrderItem>();
    }

    [Required]
    public OrderStatus Status { get; set; }
    [Required, StringLength(10)]
    public string StatusText => Status.ToDescription();
    [Required]
    public int MemberId { get; set; }

    public int? PaymentId { get; set; }
    [Required]
    public decimal TotalPrice { get; set; }
    public int SelectedAddressId { get; set; }
    public int? InvoiceId { get; set; }

    public string? ShipmentTrackingCode { get; set; }
    public virtual ICollection<OrderItem> Items { get; set; }
    public virtual Payment? Payment { get; set; }

    public virtual Invoice? Invoice { get; set; }
}
