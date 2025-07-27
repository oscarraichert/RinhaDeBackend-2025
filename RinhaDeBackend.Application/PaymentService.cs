using Microsoft.Extensions.Configuration;
using RinhaDeBackend.Domain;
using RinhaDeBackend.Infra.Repositories;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RinhaDeBackend.Application
{
    public class PaymentService
    {
        public HttpClient Client { get; }
        public PaymentRepository Repository { get; }
        private readonly IConfiguration Config;

        public PaymentService(IConfiguration configuration, PaymentRepository repository)
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Repository = repository;
            Config = configuration;
        }

        public async Task HandleProccessPayment(Payment payment)
        {
            var processingResult = await ProcessPayment(payment);

            if (processingResult.IsSuccess)
            {
                await Repository.InsertAsync(processingResult.Value!);
            }
        }

        private async Task<Result<Payment, string>> ProcessPayment(Payment payment)
        {
            var response = await Client.PostAsync(Config["PROCESSOR_DEFAULT_URL"], JsonContent.Create(payment));

            var result = response.StatusCode switch
            {
                HttpStatusCode.OK => payment,
                _ => await ProcessPaymentFallback(payment),
            };

            return result;
        }

        private async Task<Result<Payment, string>> ProcessPaymentFallback(Payment payment)
        {
            var response = await Client.PostAsync(Config["PROCESSOR_FALLBACK_URL"], JsonContent.Create(payment));

            payment.processedOnFallback = true;

            return response.StatusCode switch
            {
                HttpStatusCode.OK => payment,
                _ => response.ReasonPhrase!,
            };
        }

        public Result<PaymentSummary, string> PaymentSummary()
        {
            var payments = Repository.ReadAll();

            if (!payments.IsSuccess)
            {
                return payments.ErrorValue!;
            }

            (int Requests, decimal Amount) defaultPayments = (0, 0);
            (int Requests, decimal Amount) fallbackPayments = (0, 0);

            payments.Value!.ForEach(payment =>
            {
                _ = payment.processedOnFallback switch
                {
                    true => fallbackPayments = (fallbackPayments.Requests + 1, fallbackPayments.Amount + payment.amount),
                    false => defaultPayments = (defaultPayments.Requests + 1, defaultPayments.Amount + payment.amount),
                };
            });

            var defaultProcessor = new ProcessorSummary
            {
                TotalRequests = defaultPayments.Requests,
                TotalAmount = defaultPayments.Amount
            };

            var fallbackProcessor = new ProcessorSummary
            {
                TotalRequests = fallbackPayments.Requests,
                TotalAmount = fallbackPayments.Amount
            };

            return new PaymentSummary { Default = defaultProcessor, Fallback = fallbackProcessor };
        }
    }
}
