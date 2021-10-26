using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsStand.Core.Entities;
using NewsStand.Core.Repositories;

namespace NewsStand.Persistence.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(NewsStandDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Customer> GetByIdAsync(int id)
        {
            return await _dbContext.Customers
                .Where(c => c.Id == id)
                .Include(c => c.Purchases)
                .ThenInclude(p => p.PurchaseProducts)
                .ThenInclude(pp => pp.Product)
                .FirstOrDefaultAsync();
        }
    }
}
