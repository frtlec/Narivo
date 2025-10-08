using Narivo.Orchestrator.SyncHttpClient.Dtos;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narivo.Orchestrator.Sync;

public interface ICheckoutApiClient
{
    [Post("/api/checkout/SendResultToClient")]
    public Task SendResultToClient([Body] CheckoutSendResultToClient request);
}
