using System.Collections.Generic;
using System.Linq;
using NewsStand.Core.Entities;
using NewsStand.Core.Repositories;

namespace NewsStand.Persistence.Repositories
{
    public class PurchaseProductRepository : BaseRepository<PurchaseProduct>, IPurchaseProductRepository
    {
        public PurchaseProductRepository(NewsStandDbContext dbContext) : base(dbContext)
        {
        }

        public void UpdateProductsForPurchase(int purchaseId, IEnumerable<PurchaseProduct> purchaseProducts)
        {
            var purchaseProductsOld = _dbContext.PurchaseProducts
                .Where(pp => pp.PurchaseId == purchaseId)
                .ToList();

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
                _dbContext.PurchaseProducts.Add(purchaseProduct);
            }
        }
    }
}
