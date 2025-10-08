using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narivo.Checkout.Core.Business.Dtos.RequestDtos;

public class CheckoutRequestDto
{
    public int OrderId { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int SelectedCardId { get; set; }
    public int SelectedAddressId { get; set; }
    public string CorrelationId { get; set; }
}


public class CheckoutSendResultToClient
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public string CorrelationId { get; set; }
}