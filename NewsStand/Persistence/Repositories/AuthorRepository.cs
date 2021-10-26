using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsStand.Core.Entities;
using NewsStand.Core.Repositories;

namespace NewsStand.Persistence.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(NewsStandDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Author> GetByIdAsync(int id)
        {
            return await _dbContext.Authors
                .Where(a => a.Id == id)
                .Include(a => a.AuthorProducts)
                .ThenInclude(ap => ap.Product)
                .ThenInclude(p => p.Producer)
                .FirstOrDefaultAsync();
        }
    }
}
