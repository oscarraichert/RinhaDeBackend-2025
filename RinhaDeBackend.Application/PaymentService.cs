using RinhaDeBackend.Domain;
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

        public PaymentService()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpStatusCode> ProcessPaymentAsync(ProcessPaymentDto payment)
        {
            var response = await Client.PostAsync("http://localhost:8001/payments", JsonContent.Create(payment));

            var result = response.StatusCode switch
            {
                HttpStatusCode.OK => HttpStatusCode.OK,
                _ => await ProcessPaymentFallbackAsync(payment),
            };

            return result;
        }

        public async Task<HttpStatusCode> ProcessPaymentFallbackAsync(ProcessPaymentDto payment)
        {
            var response = await Client.PostAsync("http://localhost:8002/payments", JsonContent.Create(payment));

            var result = response.StatusCode switch
            {
                HttpStatusCode.OK => HttpStatusCode.OK,
                _ => response.StatusCode,
            };

            return result;
        }
    }
}
