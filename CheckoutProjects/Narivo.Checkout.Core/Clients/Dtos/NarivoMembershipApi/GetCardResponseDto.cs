using Narivo.Shared.Constants.Enums;
using System.ComponentModel.DataAnnotations;
namespace Narivo.Checkout.Core.Clients.Dtos.NarivoCatalogApiDtos;
public record GetCardResponseDto
{
    public int MemberId { get; set; }
    public string HolderName { get; set; }
    public string No { get; set; }
    public string CVV { get; set; }
    public string Year { get; set; }
    public string Month { get; set; }
    public Bank Bank { get; set; }
}
