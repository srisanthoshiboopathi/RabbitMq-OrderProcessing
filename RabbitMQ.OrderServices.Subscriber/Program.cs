using RabbitMQ.OrderServices.Publisher.Services;
using RabbitMQ.OrderServices.Subscriber.Data;
using RabbitMQ.OrderServices.Subscriber.ListenerService;
using RabbitMQ.OrderServices.Subscriber.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MongoDbContext to the DI container
builder.Services.AddSingleton<MongoDbContext>();

// Add RabbitMqService and OrderService to the DI container
builder.Services.AddSingleton<RabbitMqService>(serviceProvider => new RabbitMqService("localhost"));  // Replace with your RabbitMQ host if needed
builder.Services.AddSingleton<OrderService>();

// Add the background service for consuming messages from RabbitMQ
builder.Services.AddHostedService<OrderListenerService>();

builder.Services.AddControllers();

var app = builder.Build();

// Map controllers
app.MapControllers();

app.Run();
