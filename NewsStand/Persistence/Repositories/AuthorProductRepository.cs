using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsStand.Core.Entities;
using NewsStand.Core.Repositories;

namespace NewsStand.Persistence.Repositories
{
    public class AuthorProductRepository : BaseRepository<AuthorProduct>, IAuthorProductRepository
    {
        public AuthorProductRepository(NewsStandDbContext dbContext) : base(dbContext)
        {
        }

        public async Task AddAuthorsForProductAsync(int productId, List<int> authorsIds)
        {
            foreach (int authorId in authorsIds)
            {
                var authorProduct = new AuthorProduct
                {
                    ProductId = productId,
                    AuthorId = authorId,
                };
                await _dbContext.AuthorProducts.AddAsync(authorProduct);
            }
        }

        public async Task<IReadOnlyList<Author>> GetAuthorsByProductIdAsync(int productId)
        {
            return await _dbContext.AuthorProducts
                .Where(ap => ap.ProductId == productId)
                .Select(ap => ap.Author)
                .ToListAsync();
        }

        public async Task UpdateAuthorsForProductAsync(int productId, List<int> authorsIds)
        {
            var authorProductsOld = await _dbContext.AuthorProducts
                .Where(ap => ap.ProductId == productId)
                .ToListAsync();

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
                await _dbContext.AuthorProducts.AddAsync(authorProduct);
            }
        }
    }
}
