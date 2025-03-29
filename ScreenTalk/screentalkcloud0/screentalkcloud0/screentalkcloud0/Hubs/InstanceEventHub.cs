using Microsoft.AspNetCore.SignalR;

namespace screentalkcloud0.Hubs;

public class InstanceEventHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}