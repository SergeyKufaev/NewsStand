using System.Linq;
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

        public override Author GetById(int id)
        {
            return _dbContext.Authors
                .Where(a => a.Id == id)
                .Include(a => a.AuthorProducts)
                .ThenInclude(ap => ap.Product)
                .ThenInclude(p => p.Producer)
                .FirstOrDefault();
        }
    }
}
