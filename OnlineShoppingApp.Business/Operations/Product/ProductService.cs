using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Business.Dtos;
using OnlineShoppingApp.Business.Services;
using OnlineShoppingApp.Business.Types;
using OnlineShoppingApp.Data.Entities;
using OnlineShoppingApp.Data.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Business.Operations.Product
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork; // UnitOfWork arayüzü

        // Constructor - IUnitOfWork bağımlılığını alır
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Tüm ürünleri al
        public async Task<ServiceMessage<List<ProductDto>>> GetAllProductsAsync()
        {
            // Veritabanından tüm ürünleri al
            var products = await _unitOfWork.Repository<ProductEntity>().GetAll().ToListAsync();
            // Ürünleri DTO'ya dönüştür
            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                StockQuantity = p.StockQuantity
            }).ToList();

            // Başarılı bir sonuç döndür
            return new ServiceMessage<List<ProductDto>> { IsSucceed = true, Data = productDtos };
        }

        // ID ile ürünü al
        public async Task<ServiceMessage<ProductDto>> GetProductByIdAsync(int productId)
        {
            // Veritabanından ürünü al
            var product = await _unitOfWork.Repository<ProductEntity>().GetByIdAsync(productId);
            if (product == null) // Eğer ürün bulunamazsa
            {
                return new ServiceMessage<ProductDto> { IsSucceed = false, Message = "Product not found." };
            }

            // Ürünü DTO'ya dönüştür
            var productDto = new ProductDto
            {
                Id = productId,
                ProductName = product.ProductName,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };

            // Başarılı bir sonuç döndür
            return new ServiceMessage<ProductDto> { IsSucceed = true, Data = productDto };
        }

        // Yeni ürün ekle
        public async Task<ServiceMessage> AddProductAsync(AddProductDto productDto)
        {
            await _unitOfWork.BeginTransaction(); // Transaction başlat

            try
            {
                // Yeni bir ürün nesnesi oluştur
                var product = new ProductEntity
                {
                    ProductName = productDto.ProductName,
                    Price = productDto.Price,
                    StockQuantity = productDto.StockQuantity
                };

                // Ürünü veritabanına ekle
                await _unitOfWork.Repository<ProductEntity>().AddAsync(product);
                await _unitOfWork.SaveChangesAsync(); // Değişiklikleri kaydet
                await _unitOfWork.CommitTransaction(); // Transaction'ı onayla

                return new ServiceMessage { IsSucceed = true, Message = "Product added successfully." };
            }
            catch
            {
                await _unitOfWork.RollBackTransaction(); // Hata durumunda transaction'ı geri al
                return new ServiceMessage { IsSucceed = false, Message = "An error occurred while adding the product." };
            }
        }

        // Ürünü güncelle
        public async Task<ServiceMessage> UpdateProductAsync(UpdateProductDto productDto)
        {
            await _unitOfWork.BeginTransaction(); // Transaction başlat

            try
            {
                // Ürünü veritabanından al
                var product = await _unitOfWork.Repository<ProductEntity>().GetByIdAsync(productDto.Id);
                if (product == null) // Eğer ürün bulunamazsa
                {
                    return new ServiceMessage { IsSucceed = false, Message = "Product not found." };
                }

                // Ürünün özelliklerini güncelle
                product.ProductName = productDto.ProductName;
                product.Price = productDto.Price;
                product.StockQuantity = productDto.StockQuantity;

                await _unitOfWork.SaveChangesAsync(); // Değişiklikleri kaydet
                await _unitOfWork.CommitTransaction(); // Transaction'ı onayla

                return new ServiceMessage { IsSucceed = true, Message = "Product updated successfully." };
            }
            catch
            {
                await _unitOfWork.RollBackTransaction(); // Hata durumunda transaction'ı geri al
                return new ServiceMessage { IsSucceed = false, Message = "An error occurred while updating the product." };
            }
        }

        // Ürünü sil
        public async Task<ServiceMessage> DeleteProductAsync(int productId)
        {
            await _unitOfWork.BeginTransaction(); // Transaction başlat

            try
            {
                // Ürünü veritabanından al
                var product = await _unitOfWork.Repository<ProductEntity>().GetByIdAsync(productId);
                if (product == null) // Eğer ürün bulunamazsa
                {
                    return new ServiceMessage { IsSucceed = false, Message = "Product not found." };
                }

                // Ürünü sil
                _unitOfWork.Repository<ProductEntity>().Delete(product);
                await _unitOfWork.SaveChangesAsync(); // Değişiklikleri kaydet
                await _unitOfWork.CommitTransaction(); // Transaction'ı onayla

                return new ServiceMessage { IsSucceed = true, Message = "Product deleted successfully." };
            }
            catch
            {
                await _unitOfWork.RollBackTransaction(); // Hata durumunda transaction'ı geri al
                return new ServiceMessage { IsSucceed = false, Message = "An error occurred while deleting the product." };
            }
        }

        // Ürün stok miktarını ayarla
        public async Task<ServiceMessage> AdjustProductStock(int id, int newStock)
        {
            // Ürünü ID ile veritabanından al
            var product = await _unitOfWork.Repository<ProductEntity>().GetByIdAsync(id);
            if (product == null) // Eğer ürün bulunamazsa
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu ID ile eşleşen ürün bulunamadı."
                };
            }

            // Ürünün stok miktarını güncelle
            product.StockQuantity = newStock;
            _unitOfWork.Repository<ProductEntity>().Update(product); // Ürünü güncelle

            try
            {
                await _unitOfWork.SaveChangesAsync(); // Değişiklikleri kaydet
            }
            catch (Exception)
            {
                throw new Exception("Stok sayısı güncellenirken bir hata oluştu."); // Hata durumunda özel bir hata fırlat
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Stok sayısı başarıyla güncellendi."
            };
        }
    }
}
