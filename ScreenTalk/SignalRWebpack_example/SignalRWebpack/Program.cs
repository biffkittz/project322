using SignalRWebpack.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();
app.UseDefaultFiles();
app.UseStaticFiles();

// app.MapGet("/log", (
//     [FromQuery(Name = "u")] string userName,
//     [FromQuery(Name = "ip")] string ipAddress,
//     [FromServices] HubService hubService
// ) => {
    
// });

app.MapHub<ChatHub>("/hub");

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/log")
    {
        var hubContext = context.RequestServices
            .GetRequiredService<IHubContext<ChatHub>>();
        var userName = context.Request.Query["u"].ToString();
        var ipAddress = context.Request.Query["ip"].ToString();
        await hubContext.Clients.All.SendAsync("messageReceived", userName, ipAddress);
        context.Response.StatusCode = StatusCodes.Status200OK;
        return;
    }

    if (next != null)
    {
        await next.Invoke();
    }
});

app.Run();