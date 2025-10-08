using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narivo.Orchestrator.SyncHttpClient.Dtos;

public class CheckoutSendResultToClient
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public string CorrelationId { get; set; }
}