using System.Collections.Generic;
using System.Linq;
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
        public ActionResult<IEnumerable<PurchaseViewModel>> GetPurchases()
        {
            var purchases = _unitOfWork.Purchases.GetAll();

            return Ok(_mapper.Map<IEnumerable<PurchaseViewModel>>(purchases));
        }

        [HttpGet("{id:int}")]
        public ActionResult<PurchaseViewModel> GetPurchase(int id)
        {
            var purchase = _unitOfWork.Purchases.GetById(id);

            if (purchase == null)
                return NotFound();

            return Ok(_mapper.Map<PurchaseViewModel>(purchase));
        }

        [HttpPost]
        public ActionResult CreatePurchase([FromBody] PurchaseViewModel purchaseViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var purchase = _mapper.Map<Purchase>(purchaseViewModel);
            _unitOfWork.Purchases.Add(purchase);

            _unitOfWork.Complete();

            return Created($"/api/purchases/{purchase.Id}", _mapper.Map<PurchaseViewModel>(purchase));
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdatePurchase(int id, [FromBody] PurchaseViewModel purchaseViewModel)
        {
            if (id != purchaseViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var purchase = _unitOfWork.Purchases.GetById(id);

            if (purchase == null)
                return NotFound();

            //_mapper.Map(purchaseViewModel, purchase);
            purchase.CustomerId = purchaseViewModel.CustomerId;
            _unitOfWork.Purchases.Update(purchase);

            var purchaseProducts = purchaseViewModel.PurchaseProducts
                .Select(vm => _mapper.Map<PurchaseProduct>(vm))
                .ToList();

            _unitOfWork.PurchaseProducts.UpdateProductsForPurchase(purchase.Id, purchaseProducts);

            _unitOfWork.Complete();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeletePurchase(int id)
        {
            var purchase = _unitOfWork.Purchases.GetById(id);

            if (purchase == null)
                return NotFound();

            _unitOfWork.Purchases.Delete(purchase);

            _unitOfWork.Complete();

            return NoContent();
        }
    }
}
