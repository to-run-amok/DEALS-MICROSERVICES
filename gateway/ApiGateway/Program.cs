var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

Console.WriteLine(
    builder.Configuration
        .GetSection("ReverseProxy")
        .Exists());

var app = builder.Build();

app.MapReverseProxy();

app.Run();