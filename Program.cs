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
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Servers = new List<Microsoft.OpenApi.Models.OpenApiServer>
        {
            new Microsoft.OpenApi.Models.OpenApiServer { Url = "http://localhost:5057" }
        };
        return Task.CompletedTask;
    });
});

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
            .WithTitle("API del Microservicio de Usuarios")
            .WithTheme(ScalarTheme.Mars)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
