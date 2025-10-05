namespace MyPayNetApi.Models.Dtos
{
    public class PayDto
    {
        public Guid TransactionId { get; set; }
        public PaymentStatus PayStatus { get; set; }
    }
    public class PayRequestDto
    {
        public int UniqueId { get; set; }
        public decimal Total { get; set; }
        public CardInfo CardInfo { get; set; }
    }
    public class CardInfo
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string CVV { get; set; }
        public string Bank { get; set; }
    }
    public class CancelPaymentRequestDto
    {
        public Guid TransactionId { get; set; }
    }
}
