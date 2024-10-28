using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OnlineShoppingApp.Business.Operations.Order.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        [JsonIgnore]
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public List<OrderProductDto> OrderProducts { get; set; }
    }
}
