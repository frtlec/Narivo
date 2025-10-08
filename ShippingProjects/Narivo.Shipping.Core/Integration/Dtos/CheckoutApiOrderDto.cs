using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narivo.Shipping.Core.Integration.Dtos;
public class CheckoutApiOrderDto
{
    public CheckoutApiOrderStatus Status { get; set; }
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int? SelectedCardId { get; set; }
    public int? SelectedAddressId { get; set; }
}

public enum CheckoutApiOrderStatus
{
    Pending = 1,
    Paid = 2,
    Completed = 3
}
