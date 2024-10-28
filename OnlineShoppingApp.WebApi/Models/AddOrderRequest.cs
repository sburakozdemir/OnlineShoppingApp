namespace OnlineShoppingApp.WebApi.Models
{
    public class AddOrderRequest
    {
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public List<OrderProductRequest> OrderProducts { get; set; }
    }
}
