using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Business.Services;
using OnlineShoppingApp.Business.Dtos;
using System.Threading.Tasks;
using OnlineShoppingApp.WebApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // Ürün ekleme
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.AddProductAsync(new AddProductDto
            {
                ProductName = request.ProductName,
                Price = request.Price,
                StockQuantity = request.StockQuantity
            });

            return result.IsSucceed ? Ok(result.Message) : BadRequest(result.Message);
        }

        // ID ile ürün getirme
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);

            return result.IsSucceed ? Ok(result.Data) : NotFound(result.Message);
        }

        // Tüm ürünleri getirme
        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProductsAsync();

            return result.IsSucceed ? Ok(result.Data) : NotFound(result.Message);
        }

        // Ürün güncelleme
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct(UpdateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.UpdateProductAsync(new UpdateProductDto
            {
                Id = request.Id,
                ProductName = request.ProductName,
                Price = request.Price,
                StockQuantity = request.StockQuantity
            });

            return result.IsSucceed ? Ok(result.Message) : BadRequest(result.Message);
        }

        // Ürün silme
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            return result.IsSucceed ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> AdjustProductStock(int id, int newStock)
        {
            var result = await _productService.AdjustProductStock(id, newStock);
            if (!result.IsSucceed)
                return NotFound(result.Message);
            return Ok(result.Message);
        }
    }
}
