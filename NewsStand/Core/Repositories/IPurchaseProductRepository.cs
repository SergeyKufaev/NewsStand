using System.Collections.Generic;
using NewsStand.Core.Entities;

namespace NewsStand.Core.Repositories
{
    public interface IPurchaseProductRepository : IBaseRepository<PurchaseProduct>
    {
        void UpdateProductsForPurchase(int purchaseId, IEnumerable<PurchaseProduct> purchaseProducts);
    }
}
