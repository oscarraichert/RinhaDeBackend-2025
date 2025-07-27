using Microsoft.AspNetCore.Mvc;
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
builder.Services.AddSingleton<PaymentService>();
builder.Services.AddSingleton<PaymentRepository>();

builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    var connectionString = builder.Configuration["CONNECTION_STRING"];
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

app.MapPost("/payments", async (PaymentRequest payment, PaymentService service) =>
{
    await service.HandleProccessPayment(payment);
})
.WithName("ProcessPayment");

app.MapGet("payments-summary", async (PaymentService service, [FromQuery] DateTime? from, [FromQuery] DateTime? to) =>
{
    var result = await service.PaymentSummary(from, to);

    return result.IsSuccess switch
    {
        true => Results.Ok(result.Value),
        false => Results.Problem(result.ErrorValue),
    };
})
.WithName("PaymentsSummary");

app.Run();
