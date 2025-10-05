using Narivo.Shared.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Narivo.Checkout.Core.Infastructure.Entites;

public class Invoice : BaseEntity
{
    [ForeignKey(nameof(Order))]
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }
    [Required]
    public DateTime InvoiceDate { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required, StringLength(50)]
    public string BillingAddress { get; set; }
    [StringLength(50)]
    public string TaxNumber { get; set; }

}