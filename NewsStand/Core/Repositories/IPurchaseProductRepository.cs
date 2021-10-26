using System.Collections.Generic;
using System.Threading.Tasks;
using NewsStand.Core.Entities;

namespace NewsStand.Core.Repositories
{
    public interface IPurchaseProductRepository : IBaseRepository<PurchaseProduct>
    {
        Task UpdateProductsForPurchaseAsync(int purchaseId, IEnumerable<PurchaseProduct> purchaseProducts);
    }
}
