namespace Narivo.Checkout.Core.Business.Dtos.RequestDtos;

public class CreateOrderRequestDto
{
    public int MemberId { get; set; }
    public List<Item> Items { get; set; }

    public class Item
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}