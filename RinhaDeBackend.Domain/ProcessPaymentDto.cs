namespace RinhaDeBackend.Domain
{
    public class ProcessPaymentDto
    {
        public Guid correlationId { get; }
        public decimal amount { get; }
        public DateTime requestedAt { get; }
    }
}
