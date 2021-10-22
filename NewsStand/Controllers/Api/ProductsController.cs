using System.Collections.Generic;
using System.Linq;
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
        public ActionResult<IEnumerable<ProductViewModel>> GetProducts(bool includeProducers = true)
        {
            var products = _unitOfWork.Products.GetAll(includeProducers);

            return Ok(_mapper.Map<IEnumerable<ProductViewModel>>(products));
        }

        [HttpGet("{id:int}")]
        public ActionResult<ProductViewModel> GetProduct(int id)
        {
            var product = _unitOfWork.Products.GetById(id);

            if (product == null)
                return NotFound();

            var productViewModel = _mapper.Map<ProductViewModel>(product);

            productViewModel.AuthorsIds = _unitOfWork.AuthorProducts
                .GetAuthorsByProductId(product.Id)
                .Select(a => a.Id)
                .ToList();

            productViewModel.Authors = product.AuthorProducts
                .Select(ap => _mapper.Map<AuthorViewModel>(ap.Author))
                .ToList();

            return Ok(productViewModel);
        }

        [HttpPost]
        public ActionResult CreateProduct([FromBody] ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var product = _mapper.Map<Product>(productViewModel);
            _unitOfWork.Products.Add(product);

            if (productViewModel.AuthorsIds.Count > 0
                && product.Type is (ProductType.Newspaper or ProductType.Magazine or ProductType.Book))
            {
                _unitOfWork.AuthorProducts.AddAuthorsForProduct(product.Id, productViewModel.AuthorsIds);
            }

            _unitOfWork.Complete();

            return Created($"/api/products/{product.Id}", _mapper.Map<ProductViewModel>(product));
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdateProduct(int id, [FromBody] ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var product = _unitOfWork.Products.GetById(id);

            if (product == null)
                return NotFound();

            //_mapper.Map(productViewModel, product);
            product.Title = productViewModel.Title;
            product.Type = productViewModel.Type;
            product.Price = productViewModel.Price;
            product.ProducerId = productViewModel.ProducerId;
            product.NumberAvailable = productViewModel.NumberAvailable;
            _unitOfWork.Products.Update(product);

            if (productViewModel.AuthorsIds.Count > 0
                && product.Type is (ProductType.Newspaper or ProductType.Magazine or ProductType.Book))
            {
                _unitOfWork.AuthorProducts.UpdateAuthorsForProduct(product.Id, productViewModel.AuthorsIds);
            }

            _unitOfWork.Complete();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteProduct(int id)
        {
            var product = _unitOfWork.Products.GetById(id);

            if (product == null)
                return NotFound();

            _unitOfWork.Products.Delete(product);
            _unitOfWork.Complete();

            return NoContent();
        }
    }
}
