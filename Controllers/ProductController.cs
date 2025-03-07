using System.Threading.Tasks;
using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimoshStore;
using SimoshStoreAPI;

namespace MyApp.Namespace
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
        
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try 
            {
                var products = await _productService.GetProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles="admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDTO product)
        {
            var result = await _productService.CreateProductAsync(product);
            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(product);
        }
        [Authorize(Roles="admin")]
        [HttpDelete("/product/delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
        [Authorize(Roles="admin")]
        [HttpPut("/product/update/{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDTO product,[FromRoute] int id)
        {
            var result = await _productService.UpdateProductAsync(product,id);
            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(product);
        }
        [HttpGet("/product/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if(product is null)
            {
                return NotFound("Product not found");
            }
            return Ok(product);
        }
    }
}
