using Microsoft.EntityFrameworkCore;
using RinhaDeBackend.Application;
using RinhaDeBackend.Domain;
using RinhaDeBackend.Infra;
using RinhaDeBackend.Infra.Repositories;
using System;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<PaymentService>();
builder.Services.AddTransient<AppDbContext>();
builder.Services.AddTransient<PaymentRepository>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("RinhaDeBackend.Infra"))
           .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/alive", () => "Yes");

app.MapPost("/payments", async (Payment payment, PaymentService service) =>
{
    var result = await service.HandleProccessPayment(payment);

    return result.IsSuccess switch
    {
        true => Results.Ok(result.Value),
        false => Results.Problem(result.ErrorValue),
    };
})
.WithName("ProcessPayment");

app.MapGet("payments-summary", (PaymentService service) =>
{
    var result = service.PaymentSummary();

    return result.IsSuccess switch
    {
        true => Results.Ok(result.Value),
        false => Results.Problem(result.ErrorValue),
    };
})
.WithName("PaymentsSummary");

app.Run();
