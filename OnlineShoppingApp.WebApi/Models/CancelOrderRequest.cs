using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.WebApi.Models
{
    public class CancelOrderRequest
    {
        [Required]
        public int OrderId { get; set; }
    }
}
