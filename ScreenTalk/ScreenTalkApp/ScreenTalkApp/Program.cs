using ScreenTalkApp.Client.Pages;
using ScreenTalkApp.Components;
using Microsoft.AspNetCore.SignalR;

// Resources
// https://learn.microsoft.com/en-us/aspnet/core/signalr/dotnet-client?view=aspnetcore-9.0&tabs=visual-studio


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
//builder.Services
//    .AddRazorComponents()
//    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://biffkittz.screenconnect.com")
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseWebAssemblyDebugging();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapStaticAssets();
//app.MapRazorComponents<App>()
//    .AddInteractiveWebAssemblyRenderMode()
//    .AddAdditionalAssemblies(typeof(ScreenTalkApp.Client._Imports).Assembly);

app.UseCors();

app.MapRazorPages();
app.MapHub<ChatHub>("/chatHub");
app.Run();
