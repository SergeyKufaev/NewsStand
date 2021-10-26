using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsStand.Core.Entities;
using NewsStand.Core.Repositories;

namespace NewsStand.Persistence.Repositories
{
    public class PurchaseProductRepository : BaseRepository<PurchaseProduct>, IPurchaseProductRepository
    {
        public PurchaseProductRepository(NewsStandDbContext dbContext) : base(dbContext)
        {
        }

        public async Task UpdateProductsForPurchaseAsync(int purchaseId, IEnumerable<PurchaseProduct> purchaseProducts)
        {
            var purchaseProductsOld = await _dbContext.PurchaseProducts
                .Where(pp => pp.PurchaseId == purchaseId)
                .ToListAsync();

            var productsIdsOld = purchaseProductsOld
                .Select(pp => pp.ProductId)
                .ToList();

            var productsIdsNew = purchaseProducts
                .Select(pp => pp.ProductId)
                .ToList();

            var purchaseProductsToRemove = purchaseProductsOld
                .Where(pp => !productsIdsNew.Contains(pp.ProductId))
                .ToList();

            _dbContext.PurchaseProducts.RemoveRange(purchaseProductsToRemove);

            var productsIdsToUpdate = productsIdsOld
                .Except(purchaseProductsToRemove.Select(pp => pp.ProductId))
                .ToList();

            var purchaseProductsToUpdateOld = purchaseProductsOld
                .Where(pp => productsIdsToUpdate.Contains(pp.ProductId))
                .ToList();

            var purchaseProductsToUpdateNew = purchaseProducts
                .Where(pp => productsIdsToUpdate.Contains(pp.ProductId))
                .ToList();

            for (int i = 0; i < purchaseProductsToUpdateOld.Count; i++)
            {
                purchaseProductsToUpdateOld[i].Quantity = purchaseProductsToUpdateNew[i].Quantity;
            }

            foreach (var purchaseProduct in purchaseProducts.Except(purchaseProductsToUpdateNew))
            {
                await _dbContext.PurchaseProducts.AddAsync(purchaseProduct);
            }
        }
    }
}
