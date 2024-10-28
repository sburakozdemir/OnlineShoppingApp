using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Business.Dtos;
using OnlineShoppingApp.Business.Operations.Order.Dtos;
using OnlineShoppingApp.Business.Services;
using OnlineShoppingApp.Business.Types;
using OnlineShoppingApp.Data.Entities;
using OnlineShoppingApp.Data.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace OnlineShoppingApp.Business.Operations.Order
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceMessage> AddOrderAsync(OrderDto orderDto)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                // Yeni sipariş oluştur
                var order = new OrderEntity
                {
                    OrderDate = orderDto.OrderDate,
                    CustomerId = orderDto.CustomerId,
                    OrderProducts = new List<OrderProductEntity>()
                };

                decimal totalAmount = 0;

                foreach (var orderProductDto in orderDto.OrderProducts)
                {
                    var product = await _unitOfWork.Repository<ProductEntity>().GetByIdAsync(orderProductDto.ProductId);
                    if (product == null)
                    {
                        await _unitOfWork.RollBackTransaction();
                        return new ServiceMessage { IsSucceed = false, Message = "Product not found." };
                    }

                    // Stok kontrolü
                    if (product.StockQuantity < orderProductDto.Quantity)
                    {
                        await _unitOfWork.RollBackTransaction();
                        return new ServiceMessage { IsSucceed = false, Message = "Insufficient stock for product: " + product.ProductName };
                    }

                    // Toplam tutara ürün fiyatını ekle
                    totalAmount += product.Price * orderProductDto.Quantity;

                    // Sipariş ürününü ekle
                    order.OrderProducts.Add(new OrderProductEntity
                    {
                        ProductId = orderProductDto.ProductId,
                        Quantity = orderProductDto.Quantity
                    });

                    // Stok miktarını azalt
                    product.StockQuantity -= orderProductDto.Quantity;

                    // Güncellenmiş ürünü kaydet
                    await _unitOfWork.Repository<ProductEntity>().UpdateAsync(product); // Asenkron güncelleme
                }

                order.TotalAmount = totalAmount;

                await _unitOfWork.Repository<OrderEntity>().AddAsync(order);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();

                return new ServiceMessage { IsSucceed = true, Message = "Order added successfully." };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransaction();
                return new ServiceMessage { IsSucceed = false, Message = "An error occurred while adding the order: " + ex.Message };
            }
        }




        public async Task<ServiceMessage<OrderDto>> GetOrderByIdAsync(int orderId)
        {
            var order = await _unitOfWork.Repository<OrderEntity>().GetAll()
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return new ServiceMessage<OrderDto> { IsSucceed = false, Message = "Order not found." };

            var orderDto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                CustomerId = order.CustomerId,
                OrderProducts = order.OrderProducts.Select(op => new OrderProductDto
                {
                    OrderId = op.OrderId,
                    ProductId = op.ProductId,
                    Quantity = op.Quantity
                }).ToList()
            };

            return new ServiceMessage<OrderDto> { IsSucceed = true, Data = orderDto };
        }

        public async Task<ServiceMessage> UpdateOrderAsync(int id, OrderDto orderDto)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                var order = await _unitOfWork.Repository<OrderEntity>().GetAll().FirstOrDefaultAsync(o => o.Id == id);
                if (order == null) return new ServiceMessage { IsSucceed = false, Message = "Order not found." };

                order.OrderDate = orderDto.OrderDate;
                order.TotalAmount = orderDto.TotalAmount;

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();

                return new ServiceMessage { IsSucceed = true, Message = "Order updated successfully." };
            }
            catch
            {
                await _unitOfWork.RollBackTransaction();
                return new ServiceMessage { IsSucceed = false, Message = "An error occurred while updating the order." };
            }
        }

        public async Task<ServiceMessage> DeleteOrderAsync(int orderId)
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                var order = await _unitOfWork.Repository<OrderEntity>().GetAll().FirstOrDefaultAsync(o => o.Id == orderId);
                if (order == null) return new ServiceMessage { IsSucceed = false, Message = "Order not found." };

                await _unitOfWork.Repository<OrderEntity>().DeleteAsync(order);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();

                return new ServiceMessage { IsSucceed = true, Message = "Order deleted successfully." };
            }
            catch
            {
                await _unitOfWork.RollBackTransaction();
                return new ServiceMessage { IsSucceed = false, Message = "An error occurred while deleting the order." };
            }
        }

        public async Task<ServiceMessage<List<OrderDto>>> GetAllOrdersAsync()
        {
            // Eager loading ile OrderProducts'ı dahil et
            var orders = await _unitOfWork.Repository<OrderEntity>()
                .GetAll()
                .Include(o => o.OrderProducts) // OrderProducts'ı dahil et
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return new ServiceMessage<List<OrderDto>> { IsSucceed = false, Message = "No orders found." };
            }

            var orderDtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                CustomerId = o.CustomerId,
                OrderProducts = o.OrderProducts != null && o.OrderProducts.Any()
                    ? o.OrderProducts.Select(op => new OrderProductDto
                    {
                        OrderId = op.OrderId,
                        ProductId = op.ProductId,
                        Quantity = op.Quantity
                    }).ToList()
                    : new List<OrderProductDto>() // Eğer null ise boş liste döndür
            }).ToList();

            return new ServiceMessage<List<OrderDto>> { IsSucceed = true, Data = orderDtos };
        }


        public async Task<ServiceMessage<CancelOrderDto>> CancelOrderAsync(int orderId)
        {
            var order = await _unitOfWork.Repository<OrderEntity>()
                .GetAll(o => o.Id == orderId)
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return new ServiceMessage<CancelOrderDto>
                {
                    IsSucceed = false,
                    Message = "Order not found.",
                    Data = null
                };
            }

            if (order.OrderProducts != null)
            {
                foreach (var orderProduct in order.OrderProducts)
                {
                    var product = await _unitOfWork.Repository<ProductEntity>().GetByIdAsync(orderProduct.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity += orderProduct.Quantity;
                        await _unitOfWork.Repository<ProductEntity>().UpdateAsync(product);
                    }
                }
            }

            order.IsDeleted = true; // Soft delete uygulanıyor
            await _unitOfWork.Repository<OrderEntity>().UpdateAsync(order);

            await _unitOfWork.SaveChangesAsync();

            return new ServiceMessage<CancelOrderDto>
            {
                IsSucceed = true,
                Message = "Order cancelled successfully.",
                Data = new CancelOrderDto
                {
                    OrderId = order.Id,
                    IsCanceled = true,
                    Message = "Order has been cancelled and stock updated."
                }
            };
        }










    }
}
