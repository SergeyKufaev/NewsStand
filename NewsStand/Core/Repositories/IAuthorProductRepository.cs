using System.Collections.Generic;
using NewsStand.Core.Entities;

namespace NewsStand.Core.Repositories
{
    public interface IAuthorProductRepository : IBaseRepository<AuthorProduct>
    {
        IReadOnlyList<Author> GetAuthorsByProductId(int productId);
        void UpdateAuthorsForProduct(int productId, List<int> authorsIds);
        void AddAuthorsForProduct(int productId, List<int> authorsIds);
    }
}
