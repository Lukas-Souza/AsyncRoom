using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Add services to the container.

// Registre as dependências na ordem correta
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBroadCastMenssagenAsync, BroadCastMenssagenAsync>();
builder.Services.AddScoped<IRoomAppService, RoomAppService>();
builder.Services.AddScoped<IAcceptWebSocket, AcceptWebSocket>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false"));
// Add Redis
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = builder.Configuration.GetConnectionString("room-sync");
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseWebSockets();
app.MapControllers();
app.Run();



