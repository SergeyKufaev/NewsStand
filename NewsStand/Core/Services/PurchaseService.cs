using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NewsStand.Core.Entities;
using NewsStand.Core.Services.Interfaces;
using NewsStand.Core.ViewModels;

namespace NewsStand.Core.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PurchaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PurchaseViewModel>> GetPurchasesAsync()
        {
            var purchases = await _unitOfWork.Purchases.GetAllAsync();
            return _mapper.Map<IEnumerable<PurchaseViewModel>>(purchases);
        }

        public async Task<PurchaseViewModel> GetPurchaseByIdAsync(int id)
        {
            var purchase = await _unitOfWork.Purchases.GetByIdAsync(id);

            if (purchase == null)
                return null;

            var purchaseViewModel = _mapper.Map<PurchaseViewModel>(purchase);

            return purchaseViewModel;
        }

        public async Task<PurchaseViewModel> CreatePurchaseAsync(PurchaseViewModel purchaseViewModel)
        {
            var purchase = _mapper.Map<Purchase>(purchaseViewModel);
            await _unitOfWork.Purchases.AddAsync(purchase);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<PurchaseViewModel>(purchase);
        }

        public async Task<bool> UpdatePurchaseAsync(int id, PurchaseViewModel purchaseViewModel)
        {
            var purchase = await _unitOfWork.Purchases.GetByIdAsync(id);

            if (purchase == null)
                return false;

            //_mapper.Map(purchaseViewModel, purchase);
            purchase.CustomerId = purchaseViewModel.CustomerId;
            await _unitOfWork.Purchases.UpdateAsync(purchase);

            var purchaseProducts = purchaseViewModel.PurchaseProducts
                .Select(vm => _mapper.Map<PurchaseProduct>(vm))
                .ToList();

            await _unitOfWork.PurchaseProducts.UpdateProductsForPurchaseAsync(purchase.Id, purchaseProducts);

            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeletePurchaseAsync(int id)
        {
            var purchase = await _unitOfWork.Purchases.GetByIdAsync(id);

            if (purchase == null)
                return false;

            await _unitOfWork.Purchases.DeleteAsync(purchase);

            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
