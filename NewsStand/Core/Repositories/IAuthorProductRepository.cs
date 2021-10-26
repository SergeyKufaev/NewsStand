using System.Collections.Generic;
using System.Threading.Tasks;
using NewsStand.Core.Entities;

namespace NewsStand.Core.Repositories
{
    public interface IAuthorProductRepository : IBaseRepository<AuthorProduct>
    {
        Task<IReadOnlyList<Author>> GetAuthorsByProductIdAsync(int productId);
        Task UpdateAuthorsForProductAsync(int productId, List<int> authorsIds);
        Task AddAuthorsForProductAsync(int productId, List<int> authorsIds);
    }
}
