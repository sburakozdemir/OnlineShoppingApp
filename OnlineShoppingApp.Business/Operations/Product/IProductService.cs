using OnlineShoppingApp.Business.Dtos;
using OnlineShoppingApp.Business.Types;
using OnlineShoppingApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Business.Services
{
    public interface IProductService
    {
        Task<ServiceMessage> AddProductAsync(AddProductDto productDto);
        Task<ServiceMessage<ProductDto>> GetProductByIdAsync(int productId);
        Task<ServiceMessage> UpdateProductAsync(UpdateProductDto productDto);
        Task<ServiceMessage> DeleteProductAsync(int productId);
        Task<ServiceMessage<List<ProductDto>>> GetAllProductsAsync();
        Task<ServiceMessage> AdjustProductStock(int id, int newStock);

    }
}
