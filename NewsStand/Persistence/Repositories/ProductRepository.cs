using System.Collections.Generic;
using System.Linq;
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

        public override Product GetById(int id)
        {
            return _dbContext.Products
                .Where(p => p.Id == id)
                .Include(p => p.Producer)
                .Include(p => p.AuthorProducts)
                .SingleOrDefault();
        }

        public IReadOnlyList<Product> GetAll(bool includeProducers)
        {
            if (includeProducers)
            {
                return _dbContext.Products
                    .Include(p => p.Producer)
                    .ToList();
            }
            else
            {
                return _dbContext.Products
                    .ToList();
            }
        }
    }
}
