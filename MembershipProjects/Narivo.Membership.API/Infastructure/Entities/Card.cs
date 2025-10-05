using Narivo.Shared.Constants.Enums;
using Narivo.Shared.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Narivo.Membership.API.Infastructure.Entities;

public class Card : BaseEntity
{
    [ForeignKey(nameof(Member))]
    public int MemberId { get; set; }

    [Required, StringLength(100)]
    public string HolderName { get; set; }

    [Required, StringLength(16)]
    public string No { get; set; }

    [Required, StringLength(4)]
    public string CVV { get; set; }

    [Required, StringLength(4)]
    public string Year { get; set; }

    [Required, StringLength(2)]
    public string Month { get; set; }

    [Required, StringLength(10)]
    public Bank Bank { get; set; }

    public bool IsDefault { get; set; }
    public virtual Member Member { get; set; }
}
