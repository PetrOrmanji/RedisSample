using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var redisConnString = builder.Configuration["Redis:ConnectionString"];
if(string.IsNullOrEmpty(redisConnString))
{
    throw new Exception("Redis connection string not found");
}

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(redisConnString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v2");
        options.RoutePrefix = string.Empty;
    });
}

app.MapControllers();
app.Run();
