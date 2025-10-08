using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narivo.Orchestrator.EventStore.Enums;

public enum EventType
{
    CheckoutInitial,
    CheckoutStart,
    CheckoutSuccessful,
    CheckoutFailed,
    CheckoutOverRetryFailed,
    Shipped,
    Delivered,
    CheckoutCanceled,
    RefundRequested,
    RefundProcessed,
    MembershipFailed,
    OrderCompleted
}
