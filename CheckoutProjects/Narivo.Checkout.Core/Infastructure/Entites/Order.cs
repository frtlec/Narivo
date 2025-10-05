using Narivo.Checkout.Core.Infastructure.Enums;
using Narivo.Shared.Dtos;
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
    public int MemberId { get; set; }

    public int? PaymentId { get; set; }

    public decimal TotalPrice { get; set; }

    public int? InvoiceId { get; set; }
    public virtual ICollection<OrderItem> Items { get; set; }
    public virtual Payment? Payment { get; set; }

    public virtual Invoice? Invoice { get; set; }
}
