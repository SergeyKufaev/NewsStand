using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NewsStand.Persistence
{
    public class NewsStandSeeder
    {
        private readonly NewsStandDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public NewsStandSeeder(NewsStandDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _dbContext.Database.EnsureCreated();

            IdentityUser user = await _userManager.FindByEmailAsync("admin@newsstand.com");

            if (user == null)
            {
                user = new IdentityUser()
                {
                    Email = "admin@newsstand.com",
                    UserName = "admin@newsstand.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder");
                }
            }

            _dbContext.SaveChanges();
        }
    }
}

