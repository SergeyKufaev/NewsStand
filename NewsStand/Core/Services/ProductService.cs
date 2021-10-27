using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NewsStand.Core.Entities;
using NewsStand.Core.Services.Interfaces;
using NewsStand.Core.ViewModels;

namespace NewsStand.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductViewModel>> GetProductsAsync(bool includeProducers)
        {
            var products = await _unitOfWork.Products.GetAllAsync(includeProducers);

            return _mapper.Map<IEnumerable<ProductViewModel>>(products);
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
                return null;

            var productViewModel = _mapper.Map<ProductViewModel>(product);

            productViewModel.AuthorsIds = _unitOfWork.AuthorProducts
                .GetAuthorsByProductIdAsync(product.Id).Result
                .Select(a => a.Id)
                .ToList();

            productViewModel.Authors = product.AuthorProducts
                .Select(ap => _mapper.Map<AuthorViewModel>(ap.Author))
                .ToList();

            return productViewModel;
        }

        public async Task<ProductViewModel> CreateProductAsync(ProductViewModel productViewModel)
        {
            var product = _mapper.Map<Product>(productViewModel);
            await _unitOfWork.Products.AddAsync(product);

            if (productViewModel.AuthorsIds.Count > 0
                && product.Type is (ProductType.Newspaper or ProductType.Magazine or ProductType.Book))
            {
                await _unitOfWork.AuthorProducts.AddAuthorsForProductAsync(product.Id, productViewModel.AuthorsIds);
            }

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ProductViewModel>(product);
        }

        public async Task<bool> UpdateProductAsync(int id, ProductViewModel productViewModel)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
                return false;

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

            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
                return false;

            await _unitOfWork.Products.DeleteAsync(product);

            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
