using RinhaDeBackend.Domain;
using RinhaDeBackend.Infra.Repositories;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;

namespace RinhaDeBackend.Application
{
    public class PaymentService
    {
        public HttpClient Client { get; }
        public PaymentRepository Repository { get; }

        public PaymentService(PaymentRepository repository)
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Repository = repository;
        }

        public async Task<Result<ProcessPaymentDto, string>> HandleProccessPayment(ProcessPaymentDto payment)
        {
            var processingResult = await ProcessPayment(payment);

            if (processingResult.IsSuccess)
            {
                var queryResult = await Repository.InsertAsync(processingResult.Value!);

                return queryResult;
            }

            return processingResult;
        }

        private async Task<Result<ProcessPaymentDto, string>> ProcessPayment(ProcessPaymentDto payment)
        {
            var response = await Client.PostAsync("http://localhost:8001/payments", JsonContent.Create(payment));

            var result = response.StatusCode switch
            {
                HttpStatusCode.OK => payment,
                _ => await ProcessPaymentFallback(payment),
            };

            return result;
        }

        private async Task<Result<ProcessPaymentDto, string>> ProcessPaymentFallback(ProcessPaymentDto payment)
        {
            var response = await Client.PostAsync("http://localhost:8002/payments", JsonContent.Create(payment));

            return response.StatusCode switch
            {
                HttpStatusCode.OK => payment,
                _ => response.ReasonPhrase!,
            };
        }
    }
}
