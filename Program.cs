using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var redisConnection = builder.Configuration.GetConnectionString("room-sync")
    ?? builder.Configuration["REDIS_URL"]
    ?? (builder.Environment.IsDevelopment() ? "localhost:6379,abortConnect=false" : null)
    ?? throw new InvalidOperationException(
        "Configure ConnectionStrings__room-sync ou REDIS_URL no ambiente da aplicação.");
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Add services to the container.

// Registre as dependências na ordem correta
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBroadCastMenssagenAsync, BroadCastMenssagenAsync>();
builder.Services.AddScoped<IRoomAppService, RoomAppService>();
builder.Services.AddScoped<IAcceptWebSocket, AcceptWebSocket>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(redisConnection));
// Add Redis
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = redisConnection;
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseWebSockets();
app.MapControllers();
app.Run();



