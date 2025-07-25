using RinhaDeBackend.Application;
using RinhaDeBackend.Domain;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<PaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/alive", () => "Yes");

app.MapPost("/payments", async (ProcessPaymentDto payment, PaymentService service) =>
{
    return await service.ProcessPaymentAsync(payment) switch
    {
        HttpStatusCode.OK => Results.Ok(),
        HttpStatusCode.UnprocessableEntity => Results.UnprocessableEntity(),
        _ => Results.Problem()
    };
})
.WithName("ProcessPayment");

app.Run();
