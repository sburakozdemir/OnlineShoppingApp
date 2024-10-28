using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Business.Dtos;
using OnlineShoppingApp.Business.Operations.Order.Dtos;
using OnlineShoppingApp.Business.Services;
using OnlineShoppingApp.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
            if (!result.IsSucceed)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            if (!result.IsSucceed)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto orderDto)
        {
            var result = await _orderService.UpdateOrderAsync(id, orderDto);
            if (!result.IsSucceed)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result.IsSucceed)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrdersAsync();

            // result.Data null ise, NotFound döndür
            if (!result.IsSucceed || result.Data == null)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost("{id}/Cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var result = await _orderService.CancelOrderAsync(id);

            if (!result.IsSucceed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

    }
}
