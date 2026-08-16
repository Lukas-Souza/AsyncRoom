var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IRoomAppService, RoomAppService>();

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

