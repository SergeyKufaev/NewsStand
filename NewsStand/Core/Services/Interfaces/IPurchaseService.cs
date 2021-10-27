using System.Collections.Generic;
using System.Threading.Tasks;
using NewsStand.Core.ViewModels;

namespace NewsStand.Core.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<IEnumerable<PurchaseViewModel>> GetPurchasesAsync();
        Task<PurchaseViewModel> GetPurchaseByIdAsync(int id);
        Task<PurchaseViewModel> CreatePurchaseAsync(PurchaseViewModel purchaseViewModel);
        Task<bool> UpdatePurchaseAsync(int id, PurchaseViewModel purchaseViewModel);
        Task<bool> DeletePurchaseAsync(int id);
    }
}
