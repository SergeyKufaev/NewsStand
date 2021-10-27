using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsStand.Core.Services.Interfaces;
using NewsStand.Core.ViewModels;

namespace NewsStand.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts(bool includeProducers = true)
        {
            var productViewModels = await _productService.GetProductsAsync(includeProducers);

            return Ok(productViewModels);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductViewModel>> GetProduct(int id)
        {
            var productViewModel = await _productService.GetProductByIdAsync(id);

            return productViewModel != null ? Ok(productViewModel) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            productViewModel = await _productService.CreateProductAsync(productViewModel);

            return Created($"/api/products/{productViewModel.Id}", productViewModel);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var result = await _productService.UpdateProductAsync(id, productViewModel);

            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            return result ? NoContent() : NotFound();
        }
    }
}
