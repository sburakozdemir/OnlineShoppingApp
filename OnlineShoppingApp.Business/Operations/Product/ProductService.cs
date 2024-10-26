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
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Tüm ürünleri al
        public async Task<ServiceMessage<List<ProductDto>>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Repository<ProductEntity>().GetAll().ToListAsync();
            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                StockQuantity = p.StockQuantity
            }).ToList();

            return new ServiceMessage<List<ProductDto>> { IsSucceed = true, Data = productDtos };
        }

        // ID ile ürünü al
        public async Task<ServiceMessage<ProductDto>> GetProductByIdAsync(int productId)
        {
            var product = await _unitOfWork.Repository<ProductEntity>().GetByIdAsync(productId);
            if (product == null)
            {
                return new ServiceMessage<ProductDto> { IsSucceed = false, Message = "Product not found." };
            }

            var productDto = new ProductDto
            {
                Id = productId,
                ProductName = product.ProductName,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };

            return new ServiceMessage<ProductDto> { IsSucceed = true, Data = productDto };
        }

        // Yeni ürün ekle
        public async Task<ServiceMessage> AddProductAsync(AddProductDto productDto)
        {
            await _unitOfWork.BeginTransaction();

            try
            {
                var product = new ProductEntity
                {
                    ProductName = productDto.ProductName,
                    Price = productDto.Price,
                    StockQuantity = productDto.StockQuantity
                };

                await _unitOfWork.Repository<ProductEntity>().AddAsync(product);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();

                return new ServiceMessage { IsSucceed = true, Message = "Product added successfully." };
            }
            catch
            {
                await _unitOfWork.RollBackTransaction();
                return new ServiceMessage { IsSucceed = false, Message = "An error occurred while adding the product." };
            }
        }

        // Ürünü güncelle
        public async Task<ServiceMessage> UpdateProductAsync(UpdateProductDto productDto)
        {
            await _unitOfWork.BeginTransaction();

            try
            {
                var product = await _unitOfWork.Repository<ProductEntity>().GetByIdAsync(productDto.Id);
                if (product == null)
                {
                    return new ServiceMessage { IsSucceed = false, Message = "Product not found." };
                }

                product.ProductName = productDto.ProductName;
                product.Price = productDto.Price;
                product.StockQuantity = productDto.StockQuantity;

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();

                return new ServiceMessage { IsSucceed = true, Message = "Product updated successfully." };
            }
            catch
            {
                await _unitOfWork.RollBackTransaction();
                return new ServiceMessage { IsSucceed = false, Message = "An error occurred while updating the product." };
            }
        }

        // Ürünü sil
        public async Task<ServiceMessage> DeleteProductAsync(int productId)
        {
            await _unitOfWork.BeginTransaction();

            try
            {
                var product = await _unitOfWork.Repository<ProductEntity>().GetByIdAsync(productId);
                if (product == null)
                {
                    return new ServiceMessage { IsSucceed = false, Message = "Product not found." };
                }

                _unitOfWork.Repository<ProductEntity>().Delete(product);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();

                return new ServiceMessage { IsSucceed = true, Message = "Product deleted successfully." };
            }
            catch
            {
                await _unitOfWork.RollBackTransaction();
                return new ServiceMessage { IsSucceed = false, Message = "An error occurred while deleting the product." };
            }

        }
        public async Task<ServiceMessage> AdjustProductStock(int id, int newStock)
        {
            var product = _unitOfWork.Repository<ProductEntity>().GetById(id);
            if (product == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu ID ile eşleşen ürün bulunamadı."
                };
            }

            product.StockQuantity = newStock;
            _unitOfWork.Repository<ProductEntity>().Update(product);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Stok sayısı güncellenirken bir hata oluştu.");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Stok sayısı başarıyla güncellendi."
            };
        }


    }
}
