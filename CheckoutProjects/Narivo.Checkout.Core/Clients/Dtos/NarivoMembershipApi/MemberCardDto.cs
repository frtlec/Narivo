using Narivo.Shared.Constants.Enums;
using Narivo.Shared.Dtos;

namespace Narivo.Checkout.Core.Clients.Dtos.NarivoMembershipApi;

public class MemberCardDto:DtoBase
{
    public string HolderName { get; set; }
    public string No { get; set; }
    public string CVV { get; set; }
    public string Year { get; set; }
    public string Month { get; set; }
    public Bank Bank { get; set; }
    public bool IsDefault { get; set; }
}
