using System.Linq;
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

        public override Customer GetById(int id)
        {
            return _dbContext.Customers
                .Where(c => c.Id == id)
                .Include(c => c.Purchases)
                .ThenInclude(p => p.PurchaseProducts)
                .ThenInclude(pp => pp.Product)
                .FirstOrDefault();
        }
    }
}
