using System.Collections.Generic;
using System.Threading.Tasks;
using NewsStand.Core.Entities;

namespace NewsStand.Core.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<IReadOnlyList<Product>> GetAllAsync(bool includeProducers);
    }
}
