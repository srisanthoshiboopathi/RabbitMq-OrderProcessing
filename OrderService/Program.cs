using OrderServices.Data;
using OrderServices.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MongoDbContext to the DI container
builder.Services.AddSingleton<MongoDbContext>();

// Add OrderService to the DI container
builder.Services.AddScoped<OrderService>();

// Add controllers and configure Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
