using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narivo.Shipping.Core.Dtos;

public class CreateShipmentRequestDto
{
    public int OrderId { get; set; }
    public string TrackingCode { get; set; }
    public string CorrelationId { get; set; }
}
