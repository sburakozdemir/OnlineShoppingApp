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
    // OrderService, sipariş işlemleri için gerekli metotları içeren sınıftır
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor, bir IUnitOfWork nesnesi alır
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Yeni bir sipariş eklemek için asenkron metot
        public async Task<ServiceMessage> AddOrderAsync(OrderDto orderDto)
        {
            await _unitOfWork.BeginTransaction(); // İşlem başlat
            try
            {
                // Yeni sipariş oluştur
                var order = new OrderEntity
                {
                    OrderDate = orderDto.OrderDate, // Sipariş tarihi
                    CustomerId = orderDto.CustomerId, // Müşteri kimliği
                    OrderProducts = new List<OrderProductEntity>() // Sipariş ürünleri listesi
                };

                decimal totalAmount = 0; // Toplam tutar

                // Sipariş ürünlerini ekle
                foreach (var orderProductDto in orderDto.OrderProducts)
                {
                    // Ürünü veritabanından al
                    var product = await _unitOfWork.Repository<ProductEntity>().GetByIdAsync(orderProductDto.ProductId);
                    if (product == null)
                    {
                        await _unitOfWork.RollBackTransaction(); // Hata durumunda işlemi geri al
                        return new ServiceMessage { IsSucceed = false, Message = "Product not found." }; // Ürün bulunamadı mesajı
                    }

                    // Stok kontrolü
                    if (product.StockQuantity < orderProductDto.Quantity)
                    {
                        await _unitOfWork.RollBackTransaction(); // Hata durumunda işlemi geri al
                        return new ServiceMessage { IsSucceed = false, Message = "Insufficient stock for product: " + product.ProductName }; // Yetersiz stok mesajı
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

                order.TotalAmount = totalAmount; // Toplam tutarı ayarla

                await _unitOfWork.Repository<OrderEntity>().AddAsync(order); // Siparişi ekle
                await _unitOfWork.SaveChangesAsync(); // Değişiklikleri kaydet
                await _unitOfWork.CommitTransaction(); // İşlemi onayla

                return new ServiceMessage { IsSucceed = true, Message = "Order added successfully." }; // Başarılı ekleme mesajı
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransaction(); // Hata durumunda işlemi geri al
                return new ServiceMessage { IsSucceed = false, Message = "An error occurred while adding the order: " + ex.Message }; // Hata mesajı
            }
        }

        // Sipariş ID'sine göre siparişi almak için asenkron metot
        public async Task<ServiceMessage<OrderDto>> GetOrderByIdAsync(int orderId)
        {
            var order = await _unitOfWork.Repository<OrderEntity>().GetAll()
                .Include(o => o.OrderProducts) // Sipariş ürünlerini dahil et
                .ThenInclude(op => op.Product) // Ürünleri dahil et
                .FirstOrDefaultAsync(o => o.Id == orderId); // Belirtilen ID'ye göre siparişi bul

            if (order == null) return new ServiceMessage<OrderDto> { IsSucceed = false, Message = "Order not found." }; // Sipariş bulunamadı mesajı

            // Siparişi DTO'ya dönüştür
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

            return new ServiceMessage<OrderDto> { IsSucceed = true, Data = orderDto }; // Başarılı durum mesajı ile sipariş DTO'sunu döndür
        }

        // Siparişi güncellemek için asenkron metot
        public async Task<ServiceMessage> UpdateOrderAsync(int id, OrderDto orderDto)
        {
            await _unitOfWork.BeginTransaction(); // İşlem başlat
            try
            {
                var order = await _unitOfWork.Repository<OrderEntity>().GetAll().FirstOrDefaultAsync(o => o.Id == id); // Belirtilen ID'ye göre siparişi bul
                if (order == null) return new ServiceMessage { IsSucceed = false, Message = "Order not found." }; // Sipariş bulunamadı mesajı

                order.OrderDate = orderDto.OrderDate; // Sipariş tarihini güncelle
                order.TotalAmount = orderDto.TotalAmount; // Toplam tutarı güncelle

                await _unitOfWork.SaveChangesAsync(); // Değişiklikleri kaydet
                await _unitOfWork.CommitTransaction(); // İşlemi onayla

                return new ServiceMessage { IsSucceed = true, Message = "Order updated successfully." }; // Başarılı güncelleme mesajı
            }
            catch
            {
                await _unitOfWork.RollBackTransaction(); // Hata durumunda işlemi geri al
                return new ServiceMessage { IsSucceed = false, Message = "An error occurred while updating the order." }; // Hata mesajı
            }
        }

        // Siparişi silmek için asenkron metot
        public async Task<ServiceMessage> DeleteOrderAsync(int orderId)
        {
            await _unitOfWork.BeginTransaction(); // İşlem başlat
            try
            {
                var order = await _unitOfWork.Repository<OrderEntity>().GetAll().FirstOrDefaultAsync(o => o.Id == orderId); // Belirtilen ID'ye göre siparişi bul
                if (order == null) return new ServiceMessage { IsSucceed = false, Message = "Order not found." }; // Sipariş bulunamadı mesajı

                await _unitOfWork.Repository<OrderEntity>().DeleteAsync(order); // Siparişi sil
                await _unitOfWork.SaveChangesAsync(); // Değişiklikleri kaydet
                await _unitOfWork.CommitTransaction(); // İşlemi onayla

                return new ServiceMessage { IsSucceed = true, Message = "Order deleted successfully." }; // Başarılı silme mesajı
            }
            catch
            {
                await _unitOfWork.RollBackTransaction(); // Hata durumunda işlemi geri al
                return new ServiceMessage { IsSucceed = false, Message = "An error occurred while deleting the order." }; // Hata mesajı
            }
        }

        // Tüm siparişleri almak için asenkron metot
        public async Task<ServiceMessage<List<OrderDto>>> GetAllOrdersAsync()
        {
            // Eager loading ile OrderProducts'ı dahil et
            var orders = await _unitOfWork.Repository<OrderEntity>()
                .GetAll()
                .Include(o => o.OrderProducts) // OrderProducts'ı dahil et
                .ToListAsync(); // Tüm siparişleri listele

            if (orders == null || !orders.Any())
            {
                return new ServiceMessage<List<OrderDto>> { IsSucceed = false, Message = "No orders found." }; // Sipariş bulunamadı mesajı
            }

            // Siparişleri DTO'ya dönüştür
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

            return new ServiceMessage<List<OrderDto>> { IsSucceed = true, Data = orderDtos }; // Başarılı durum mesajı ile sipariş DTO'larını döndür
        }

        // Siparişi iptal etmek için asenkron metot
        public async Task<ServiceMessage<CancelOrderDto>> CancelOrderAsync(int orderId)
        {
            var order = await _unitOfWork.Repository<OrderEntity>()
                .GetAll(o => o.Id == orderId)
                .Include(o => o.OrderProducts) // Sipariş ürünlerini dahil et
                .FirstOrDefaultAsync();

            if (order == null)
                return new ServiceMessage<CancelOrderDto> { IsSucceed = false, Message = "Order not found." }; // Sipariş bulunamadı mesajı

            // Siparişi iptal et
            foreach (var orderProduct in order.OrderProducts)
            {
                var product = await _unitOfWork.Repository<ProductEntity>().GetByIdAsync(orderProduct.ProductId);
                if (product != null)
                {
                    // İade edilen miktarı stok miktarına ekle
                    product.StockQuantity += orderProduct.Quantity;
                    await _unitOfWork.Repository<ProductEntity>().UpdateAsync(product); // Ürünü güncelle
                }
            }

            await _unitOfWork.Repository<OrderEntity>().DeleteAsync(order); // Siparişi sil
            await _unitOfWork.SaveChangesAsync(); // Değişiklikleri kaydet

            return new ServiceMessage<CancelOrderDto> { IsSucceed = true, Message = "Order canceled successfully." }; // Başarılı iptal mesajı
        }
    }
}
