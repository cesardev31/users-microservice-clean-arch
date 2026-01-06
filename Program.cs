using Scalar.AspNetCore;
using FluentValidation;
using testing.API.Middleware;
using testing.Core.Application.Services;
using testing.Core.Domain.Interfaces;
using testing.Infrastructure.Messaging;
using testing.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Infrastructure - MongoDB
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Infrastructure - Message Bus
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

// Application Services
builder.Services.AddScoped<IUserService, UserService>();

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Global Exception Handling
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => 
    {
        options
            .WithTitle("Users Microservice API")
            .WithTheme(ScalarTheme.Mars)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
