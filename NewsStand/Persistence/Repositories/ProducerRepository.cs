using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsStand.Core.Entities;
using NewsStand.Core.Repositories;

namespace NewsStand.Persistence.Repositories
{
    public class ProducerRepository : BaseRepository<Producer>, IProducerRepository
    {
        public ProducerRepository(NewsStandDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Producer> GetByIdAsync(int id)
        {
            return await _dbContext.Producers
                .Where(p => p.Id == id)
                .Include(p => p.Products)
                .ThenInclude(p => p.AuthorProducts)
                .ThenInclude(ap => ap.Author)
                .SingleOrDefaultAsync();
        }
    }
}
