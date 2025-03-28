using RabbitMQ.OrderServices.Publisher.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Register RabbitMqService to DI container
builder.Services.AddSingleton<RabbitMqService>(serviceProvider => new RabbitMqService("localhost")); // Replace "localhost" with your RabbitMQ host if needed

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();
