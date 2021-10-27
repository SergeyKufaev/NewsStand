using System.Collections.Generic;
using System.Threading.Tasks;
using NewsStand.Core.ViewModels;

namespace NewsStand.Core.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetProductsAsync(bool includeProducers);
        Task<ProductViewModel> GetProductByIdAsync(int id);
        Task<ProductViewModel> CreateProductAsync(ProductViewModel productViewModel);
        Task<bool> UpdateProductAsync(int id, ProductViewModel productViewModel);
        Task<bool> DeleteProductAsync(int id);
    }
}
