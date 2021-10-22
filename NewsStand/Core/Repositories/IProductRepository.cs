using System.Collections.Generic;
using NewsStand.Core.Entities;

namespace NewsStand.Core.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        IReadOnlyList<Product> GetAll(bool includeProducers);
    }
}
