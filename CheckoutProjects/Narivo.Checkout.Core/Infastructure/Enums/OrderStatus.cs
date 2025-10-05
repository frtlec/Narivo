using System.ComponentModel;

namespace Narivo.Checkout.Core.Infastructure.Enums;

public enum OrderStatus
{
    [Description("Beklemede")]
    Pending = 1,
    [Description("Ödendi")]
    Paid = 2,
    [Description("Kargoda")]
    Shipped = 3,
    [Description("Teslim Edildi")]
    Completed = 4,
    [Description("İptal Edildi")]
    Cancelled = 5
}
