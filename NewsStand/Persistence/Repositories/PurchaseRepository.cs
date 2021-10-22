using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NewsStand.Core.Entities;
using NewsStand.Core.Repositories;

namespace NewsStand.Persistence.Repositories
{
    public class PurchaseRepository : BaseRepository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(NewsStandDbContext dbContext) : base(dbContext)
        {
        }

        public override IReadOnlyList<Purchase> GetAll()
        {
            return _dbContext.Purchases
                .Include(p => p.Customer)
                .Include(p => p.PurchaseProducts)
                .ToList();
        }

        public override Purchase GetById(int id)
        {
            return _dbContext.Purchases
                .Where(p => p.Id == id)
                .Include(p => p.Customer)
                .Include(p => p.PurchaseProducts)
                .ThenInclude(pp => pp.Product)
                .SingleOrDefault();
        }
    }
}
