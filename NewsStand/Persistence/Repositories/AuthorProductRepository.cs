using System.Collections.Generic;
using System.Linq;
using NewsStand.Core.Entities;
using NewsStand.Core.Repositories;

namespace NewsStand.Persistence.Repositories
{
    public class AuthorProductRepository : BaseRepository<AuthorProduct>, IAuthorProductRepository
    {
        public AuthorProductRepository(NewsStandDbContext dbContext) : base(dbContext)
        {
        }

        public void AddAuthorsForProduct(int productId, List<int> authorsIds)
        {
            foreach (int authorId in authorsIds)
            {
                var authorProduct = new AuthorProduct
                {
                    ProductId = productId,
                    AuthorId = authorId,
                };
                _dbContext.AuthorProducts.Add(authorProduct);
            }
        }

        public IReadOnlyList<Author> GetAuthorsByProductId(int productId)
        {
            return _dbContext.AuthorProducts
                .Where(ap => ap.ProductId == productId)
                .Select(ap => ap.Author)
                .ToList();
        }

        public void UpdateAuthorsForProduct(int productId, List<int> authorsIds)
        {
            var authorProductsOld = _dbContext.AuthorProducts
                .Where(ap => ap.ProductId == productId)
                .ToList();

            var authorIdsOld = authorProductsOld
                .Select(ap => ap.AuthorId)
                .ToList();

            var authorProductsToRemove = authorProductsOld
                .Where(ap => !authorsIds.Contains(ap.AuthorId))
                .ToList();

            _dbContext.AuthorProducts.RemoveRange(authorProductsToRemove);

            foreach (int authorId in authorsIds.Except(authorIdsOld))
            {
                var authorProduct = new AuthorProduct
                {
                    AuthorId = authorId,
                    ProductId = productId
                };
                _dbContext.AuthorProducts.Add(authorProduct);
            }
        }
    }
}
