using Narivo.Shared.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Narivo.Checkout.Core.Infastructure.Entites;

public class Payment : BaseEntity
{
    [ForeignKey(nameof(Order))]
    public int OrderId { get; set; }

    public bool IsSuccess { get; set; }

    [StringLength(500)]
    public string ErrorMessage { get; set; }
    [StringLength(50)]
    public string? TransactionId { get; set; }

    public int CardId { get; set; }

    public virtual Order Order { get; set; }
}
