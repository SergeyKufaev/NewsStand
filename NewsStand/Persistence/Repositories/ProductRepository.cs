using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsStand.Core.Entities;
using NewsStand.Core.Repositories;

namespace NewsStand.Persistence.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(NewsStandDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Product> GetByIdAsync(int id)
        {
            return await _dbContext.Products
                .Where(p => p.Id == id)
                .Include(p => p.Producer)
                .Include(p => p.AuthorProducts)
                .SingleOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync(bool includeProducers)
        {
            if (includeProducers)
            {
                return await _dbContext.Products
                    .Include(p => p.Producer)
                    .ToListAsync();
            }
            else
            {
                return await _dbContext.Products
                    .ToListAsync();
            }
        }
    }
}
