using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public override async Task<IReadOnlyList<Purchase>> GetAllAsync()
        {
            return await _dbContext.Purchases
                .Include(p => p.Customer)
                .Include(p => p.PurchaseProducts)
                .ToListAsync();
        }

        public override async Task<Purchase> GetByIdAsync(int id)
        {
            return await _dbContext.Purchases
                .Where(p => p.Id == id)
                .Include(p => p.Customer)
                .Include(p => p.PurchaseProducts)
                .ThenInclude(pp => pp.Product)
                .SingleOrDefaultAsync();
        }
    }
}
