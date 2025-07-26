namespace RinhaDeBackend.Domain
{
    public class ProcessPaymentDto
    {
        public Guid correlationId { get; set; }
        public decimal amount { get; set; }
        public DateTime requestedAt { get; set; }
    }
}
