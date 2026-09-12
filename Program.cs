using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var redisConnection = builder.Configuration.GetConnectionString("room-sync")
    ?? throw new InvalidOperationException(
        "A conexão ConnectionStrings:room-sync não foi configurada.");
var redisOptions = ConfigurationOptions.Parse(redisConnection);
redisOptions.AbortOnConnectFail = false;
redisOptions.ConnectRetry = 5;
redisOptions.ConnectTimeout = 10000;
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
    ConnectionMultiplexer.Connect(redisOptions));
// Add Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.ConfigurationOptions = redisOptions;
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



