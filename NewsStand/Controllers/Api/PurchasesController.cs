using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsStand.Core;
using NewsStand.Core.Entities;
using NewsStand.Core.ViewModels;

namespace NewsStand.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PurchasesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseViewModel>>> GetPurchases()
        {
            var purchases = await _unitOfWork.Purchases.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<PurchaseViewModel>>(purchases));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PurchaseViewModel>> GetPurchase(int id)
        {
            var purchase = await _unitOfWork.Purchases.GetByIdAsync(id);

            if (purchase == null)
                return NotFound();

            return Ok(_mapper.Map<PurchaseViewModel>(purchase));
        }

        [HttpPost]
        public async Task<ActionResult> CreatePurchase([FromBody] PurchaseViewModel purchaseViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var purchase = _mapper.Map<Purchase>(purchaseViewModel);
            await _unitOfWork.Purchases.AddAsync(purchase);

            await _unitOfWork.CompleteAsync();

            return Created($"/api/purchases/{purchase.Id}", _mapper.Map<PurchaseViewModel>(purchase));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdatePurchase(int id, [FromBody] PurchaseViewModel purchaseViewModel)
        {
            if (id != purchaseViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var purchase = await _unitOfWork.Purchases.GetByIdAsync(id);

            if (purchase == null)
                return NotFound();

            //_mapper.Map(purchaseViewModel, purchase);
            purchase.CustomerId = purchaseViewModel.CustomerId;
            await _unitOfWork.Purchases.UpdateAsync(purchase);

            var purchaseProducts = purchaseViewModel.PurchaseProducts
                .Select(vm => _mapper.Map<PurchaseProduct>(vm))
                .ToList();

            await _unitOfWork.PurchaseProducts.UpdateProductsForPurchaseAsync(purchase.Id, purchaseProducts);

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeletePurchase(int id)
        {
            var purchase = await _unitOfWork.Purchases.GetByIdAsync(id);

            if (purchase == null)
                return NotFound();

            await _unitOfWork.Purchases.DeleteAsync(purchase);

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
