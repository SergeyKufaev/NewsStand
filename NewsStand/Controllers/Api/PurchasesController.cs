using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsStand.Core.Services.Interfaces;
using NewsStand.Core.ViewModels;

namespace NewsStand.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchasesController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseViewModel>>> GetPurchases()
        {
            var purchaseViewModels = await _purchaseService.GetPurchasesAsync();

            return Ok(purchaseViewModels);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PurchaseViewModel>> GetPurchase(int id)
        {
            var purchaseViewModel = await _purchaseService.GetPurchaseByIdAsync(id);

            return purchaseViewModel != null ? Ok(purchaseViewModel) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> CreatePurchase([FromBody] PurchaseViewModel purchaseViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            purchaseViewModel = await _purchaseService.CreatePurchaseAsync(purchaseViewModel);

            return Created($"/api/purchases/{purchaseViewModel.Id}", purchaseViewModel);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdatePurchase(int id, [FromBody] PurchaseViewModel purchaseViewModel)
        {
            if (id != purchaseViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var result = await _purchaseService.UpdatePurchaseAsync(id, purchaseViewModel);

            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeletePurchase(int id)
        {
            var result = await _purchaseService.DeletePurchaseAsync(id);

            return result ? NoContent() : NotFound();
        }
    }
}
