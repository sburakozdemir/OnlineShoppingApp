using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Business.Dtos;
using OnlineShoppingApp.Business.Operations.Order.Dtos;
using OnlineShoppingApp.Business.Services;
using OnlineShoppingApp.WebApi.Models;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;

namespace OnlineShoppingApp.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService; // Sipariş servisinin referansı

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService; // Sipariş servisinin ataması
        }

        // Yeni sipariş eklemek için endpoint
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequest request)
        {
            if (!ModelState.IsValid) // Modelin geçerli olup olmadığını kontrol et
                return BadRequest(ModelState); // Hatalıysa BadRequest döndür

            var orderDto = new OrderDto
            {
                OrderDate = request.OrderDate,
                CustomerId = request.CustomerId,
                OrderProducts = request.OrderProducts.Select(p => new OrderProductDto
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                }).ToList()
            };

            var result = await _orderService.AddOrderAsync(orderDto);
            // Sipariş ekleme sonucuna göre uygun yanıtı döndür
            if (!result.IsSucceed)
                return BadRequest(result.Message);

            return Ok(result); // Başarılıysa sonucu döndür
        }

        // Belirli bir siparişi almak için endpoint
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            // Sipariş bulunamazsa NotFound döndür
            if (!result.IsSucceed)
                return NotFound(result.Message);

            return Ok(result.Data); // Başarılıysa siparişi döndür
        }

        // Siparişi güncellemek için endpoint
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto orderDto)
        {
            var result = await _orderService.UpdateOrderAsync(id, orderDto);
            // Güncelleme işlemi başarısızsa NotFound döndür
            if (!result.IsSucceed)
                return NotFound(result.Message);

            return Ok(result); // Başarılıysa sonucu döndür
        }

        // Siparişi silmek için endpoint
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            // Silme işlemi başarısızsa NotFound döndür
            if (!result.IsSucceed)
                return NotFound(result.Message);

            return Ok(result); // Başarılıysa sonucu döndür
        }

        // Tüm siparişleri almak için endpoint
        [HttpGet("All")]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrdersAsync();

            // result.Data null ise, NotFound döndür
            if (!result.IsSucceed || result.Data == null)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data); // Başarılıysa sipariş listesini döndür
        }

        // Belirli bir siparişi iptal etmek için endpoint
        [HttpPost("{id}/Cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var result = await _orderService.CancelOrderAsync(id);

            // İptal işlemi başarısızsa BadRequest döndür
            if (!result.IsSucceed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data); // Başarılıysa iptal sonucunu döndür
        }
    }
}
