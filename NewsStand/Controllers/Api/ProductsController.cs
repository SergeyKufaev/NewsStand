using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsStand.Core;
using NewsStand.Core.Entities;
using NewsStand.Core.ViewModels;

namespace NewsStand.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts(bool includeProducers = true)
        {
            var products = await _unitOfWork.Products.GetAllAsync(includeProducers);

            return Ok(_mapper.Map<IEnumerable<ProductViewModel>>(products));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductViewModel>> GetProduct(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            var productViewModel = _mapper.Map<ProductViewModel>(product);

            productViewModel.AuthorsIds = _unitOfWork.AuthorProducts
                .GetAuthorsByProductIdAsync(product.Id).Result
                .Select(a => a.Id)
                .ToList();

            productViewModel.Authors = product.AuthorProducts
                .Select(ap => _mapper.Map<AuthorViewModel>(ap.Author))
                .ToList();

            return Ok(productViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var product = _mapper.Map<Product>(productViewModel);
            await _unitOfWork.Products.AddAsync(product);

            if (productViewModel.AuthorsIds.Count > 0
                && product.Type is (ProductType.Newspaper or ProductType.Magazine or ProductType.Book))
            {
                await _unitOfWork.AuthorProducts.AddAuthorsForProductAsync(product.Id, productViewModel.AuthorsIds);
            }

            await _unitOfWork.CompleteAsync();

            return Created($"/api/products/{product.Id}", _mapper.Map<ProductViewModel>(product));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            //_mapper.Map(productViewModel, product);
            product.Title = productViewModel.Title;
            product.Type = productViewModel.Type;
            product.Price = productViewModel.Price;
            product.ProducerId = productViewModel.ProducerId;
            product.NumberAvailable = productViewModel.NumberAvailable;
            await _unitOfWork.Products.UpdateAsync(product);

            if (productViewModel.AuthorsIds.Count > 0
                && product.Type is (ProductType.Newspaper or ProductType.Magazine or ProductType.Book))
            {
                await _unitOfWork.AuthorProducts.UpdateAuthorsForProductAsync(product.Id, productViewModel.AuthorsIds);
            }

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            await _unitOfWork.Products.DeleteAsync(product);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
