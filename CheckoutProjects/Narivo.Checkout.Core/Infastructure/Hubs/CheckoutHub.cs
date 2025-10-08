using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narivo.Checkout.Core.Infastructure.Hubs;


public class CheckoutHub : Hub
{
    // Client'ın gruba katılması için
    public async Task JoinGroup(string correlationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, correlationId);
    }

    // Opsiyonel: client ayrılırsa
    public async Task LeaveGroup(string correlationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, correlationId);
    }
}


public class SimpleHub : Hub
{
    public async Task SendMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty");

        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}