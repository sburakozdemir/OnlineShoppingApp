using OnlineShoppingApp.Business.Operations.Order.Dtos;
using OnlineShoppingApp.Business.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Business.Services
{
    public interface IOrderService
    {
        Task<ServiceMessage> AddOrderAsync(OrderDto orderDto);
        Task<ServiceMessage<OrderDto>> GetOrderByIdAsync(int orderId);
        Task<ServiceMessage> UpdateOrderAsync(int id, OrderDto orderDto);
        Task<ServiceMessage> DeleteOrderAsync(int orderId);
        Task<ServiceMessage<List<OrderDto>>> GetAllOrdersAsync();
        Task<ServiceMessage<CancelOrderDto>> CancelOrderAsync(int orderId);
    }
}
