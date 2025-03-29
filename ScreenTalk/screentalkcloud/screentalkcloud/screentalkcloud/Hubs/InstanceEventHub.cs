using Microsoft.AspNetCore.SignalR;

namespace screentalkcloud.Hubs;

public class InstanceEventHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}